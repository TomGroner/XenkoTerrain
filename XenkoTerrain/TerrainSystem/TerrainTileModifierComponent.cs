using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Physics;
using XenkoTerrain.Services;

namespace XenkoTerrain.TerrainSystem
{
  public partial class TerrainTileModifierComponent : SyncScript, ITerrainTileModifier
  {
    public CameraComponent CurrentCamera { get; set; }

    public TerrainTileComponent TerrainTile { get; set; }

    [DataMemberIgnore]
    public ModificationCommandCollection Commands { get; set; }

    public float Radius { get; set; } = 4.0f;

    public override void Start()
    {
      DebugText.Visible = true;
      Commands = ModificationCommandSourceFactoryProvider.Factory?.GetCommands();
      Commands.DebugText = DebugText;

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
      var context = new ModificationCommandContext
      {
        Modifier = this,
        DataModified = false,
        Input = Input,
        Services = Game.Services,
        Data = TerrainTile.CurrentGeometryData,
        DeltaTime = (float)Game.TargetElapsedTime.TotalSeconds,
        WorldMatrix = Entity.Transform.WorldMatrix,
        Hit = Utils.ScreenPositionToWorldPositionRaycast(Input.MousePosition, CurrentCamera, this.GetSimulation())
      };

      Commands.Update(context);

      if (context.DataModified)
      {
        TerrainTile.CurrentMeshDraw.VertexBuffers[0].Buffer.SetData(Game.GraphicsContext.CommandList, TerrainTile.CurrentGeometryData.Vertices);
      }
    }
  }
}