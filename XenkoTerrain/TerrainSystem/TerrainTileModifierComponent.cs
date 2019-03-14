using System;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Graphics;
using Xenko.Input;
using Xenko.Physics;
using XenkoTerrain.Services;

// Special thank you to profan for his sample on updating geometry on the fly: https://github.com/profan/XenkoByteSized
// His code was the basis for this class, with modifications (different geometry structure for example) but otherwise
// his technique entirely :)
namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileModifierComponent : SyncScript
  {
    const float UNITS_PER_SECOND = 2.0f;

    public enum ModificationCommand
    {
      None,
      Raise,
      Lower,
      Smoothen,
      Flatten,
      SaveToFile,
      IncreaseRadius,
      DecreaseRadius
    }

    public CameraComponent CurrentCamera { get; set; }

    public TerrainTileComponent TerrainTile { get; set; }

    public float Radius { get; set; } = 4.0f;

    public override void Start()
    {
      if (TerrainTile != null && !TerrainTile.Entity.Has<StaticColliderComponent>())
      {
        var colliderComponent = new StaticColliderComponent();
        var planeShape = new StaticPlaneColliderShape(new Vector3(0.0f, 1.0f, 0.0f), TerrainTile.MaxHeight / 2);
        colliderComponent.ColliderShapes.Add(new StaticPlaneColliderShapeDesc() { Normal = new Vector3(0.0f, 1.0f, 0.0f) });
        colliderComponent.ColliderShape = planeShape;
        TerrainTile.Entity.Add(colliderComponent);
      }
    }

    public override void Update()
    {
      if (TerrainTile != null && TerrainTile.IsSet && TryGetModifyCommand(out var command))
      {
        var dt = (float)Game.TargetElapsedTime.TotalSeconds;
        var worldPosHit = Utils.ScreenPositionToWorldPositionRaycast(Input.MousePosition, CurrentCamera, this.GetSimulation());

        if (worldPosHit.Succeeded)
        {
          var pickPosition = Entity.Transform.WorldToLocal(worldPosHit.Point).XZ();
          Modify(pickPosition, command, UNITS_PER_SECOND * dt);
        }
      }
    }

    private void FixNormals(VertexPositionNormalTexture[] vertices)
    {
      int x = 0;
      int y = 0;
      var wrapAtX = TerrainTile.CurrentGeometryData.HeightSource.Columns + 1;
      var stopAtY = TerrainTile.CurrentGeometryData.HeightSource.Rows + 1;

      // This generates something between standard cross product per triangle and full quality smoothing. The normals should
      // look decent unless each vertex covers a lot of space, then it will start to look a bit choppy.
      for (int i = 0; i < vertices.Length; ++i)
      {
        var heightL = x > 0 && i - 1 > 0 ? vertices[i - 1].Position.Y : 0.0f;
        var heightR = x < wrapAtX && i + 1 < vertices.Length ? vertices[i + 1].Position.Y : 0.0f;
        var heightU = y > 0 && i + wrapAtX < vertices.Length ? vertices[i + wrapAtX].Position.Y : 0.0f;
        var heightD = y < stopAtY && i - wrapAtX > 0 ? vertices[i - wrapAtX].Position.Y : 0.0f;
        var newNormal = new Vector3(heightL - heightR, 2.0f, heightD - heightU);
        newNormal.Normalize();

        ref var normal = ref vertices[i].Normal;
        normal.X = newNormal.X;
        normal.Y = newNormal.Y;
        normal.Z = newNormal.Z;
        x++;

        if (x == wrapAtX)
        {
          x = 0;
          y++;
        }
      }
    }

    public void Modify(Vector2 pickPosition, ModificationCommand mode, float delta)
    {
      var data = TerrainTile.CurrentGeometryData;

      switch (mode)
      {
        case ModificationCommand.IncreaseRadius: Radius++; return;
        case ModificationCommand.DecreaseRadius: Radius = Math.Max(Radius - 1, 1); return;
        case ModificationCommand.SaveToFile: Save(); return;
        case ModificationCommand.Raise: Raise(pickPosition, delta, data.Vertices); break;
        case ModificationCommand.Lower: Lower(pickPosition, delta, data.Vertices); break;
        case ModificationCommand.Smoothen: Smoothen(pickPosition, delta, data.Vertices); break;
        case ModificationCommand.Flatten: Flatten(pickPosition, delta, data.Vertices); break;
        default: return;
      }

      FixNormals(data.Vertices);
      TerrainTile.CurrentMeshDraw.VertexBuffers[0].Buffer.SetData(Game.GraphicsContext.CommandList, data.Vertices);
    }

    private async void Save()
    {
      if (Game.Services.TryGetService<SaveTerrainService>(out var saveService))
      {
        await saveService.SaveAsync(TerrainTile.CurrentGeometryData);
      }
    }

    private void AdjustHeight(Vector2 pos, float delta, VertexPositionNormalTexture[] vertices)
    {
      for (int i = 0; i < vertices.Length; ++i)
      {
        ref var position = ref vertices[i].Position;
        var dist = (position.XZ() - pos).Length();
        var factor = Radius - dist;

        if (factor >= 0.0f)
        {
          position.Y += factor * delta;
        }
      }
    }

    void Raise(Vector2 pickPosition, float delta, VertexPositionNormalTexture[] vertices)
    {
      AdjustHeight(pickPosition, delta, vertices);
    }

    void Lower(Vector2 pickPosition, float delta, VertexPositionNormalTexture[] vertices)
    {
      AdjustHeight(pickPosition, -delta, vertices);
    }

    void Smoothen(Vector2 pickPosition, float delta, VertexPositionNormalTexture[] vertices)
    {
      float totalHeights = 0.0f;
      for (int i = 0; i < vertices.Length; ++i)
      {
        ref var v = ref vertices[i].Position;
        var dist = (v.XZ() - pickPosition).Length();
        if (dist <= Radius)
        {
          totalHeights += v.Y;
        }
      }

      float avgHeight = totalHeights / vertices.Length;

      for (int i = 0; i < vertices.Length; ++i)
      {
        ref var v = ref vertices[i].Position;
        var dist = (v.XZ() - pickPosition).Length();
        if (dist <= Radius)
        {
          v.Y += (avgHeight - v.Y) * (Radius - dist) * delta;
        }
      }
    }

    float Flatten(Vector2 pickPosition, float delta, VertexPositionNormalTexture[] vertices)
    {
      float closestVertVal = float.MaxValue;
      float closestVertHeight = 0.0f;

      for (int i = 0; i < vertices.Length; ++i)
      {
        ref var v = ref vertices[i].Position;
        float dist = (v.XZ() - pickPosition).Length();
        if (dist <= Radius && dist <= closestVertVal)
        {
          closestVertVal = dist;
          closestVertHeight = v.Y;
        }
      }

      for (int i = 0; i < vertices.Length; ++i)
      {
        ref var v = ref vertices[i].Position;
        var dist = (v.XZ() - pickPosition).Length();
        if (dist <= Radius)
        {
          v.Y = closestVertHeight;
        }
      }

      return closestVertVal;
    }

    private bool TryGetModifyCommand(out ModificationCommand mode)
    {
      mode = ModificationCommand.None;

      // radius here

      if (Input.IsKeyDown(Keys.LeftCtrl) && Input.IsKeyDown(Keys.S))
      {
        mode = ModificationCommand.SaveToFile;
      }
      else if (Input.IsMouseButtonDown(MouseButton.Left) && Input.IsKeyDown(Keys.LeftCtrl))
      {
        mode = ModificationCommand.Flatten;
      }
      else if (Input.IsMouseButtonDown(MouseButton.Left) && Input.IsKeyDown(Keys.LeftShift))
      {
        mode = ModificationCommand.Smoothen;
      }
      else if (Input.IsMouseButtonDown(MouseButton.Left))
      {
        mode = ModificationCommand.Raise;
      }
      else if (Input.IsMouseButtonDown(MouseButton.Middle))
      {
        mode = ModificationCommand.Lower;
      }

      return mode != ModificationCommand.None;
    }
  }
}
