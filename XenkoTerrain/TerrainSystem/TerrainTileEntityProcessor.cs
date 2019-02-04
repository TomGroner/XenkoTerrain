using System.Collections.Generic;
using Xenko.Core.Annotations;
using Xenko.Engine;
using Xenko.Extensions;
using Xenko.Games;
using Xenko.Physics;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileEntityProcessor : EntityProcessor<TerrainTileEntityComponent, TerrainTileRenderObject>, IEntityComponentRenderProcessor
  {    
    public VisibilityGroup VisibilityGroup { get; set; }

    protected override bool IsAssociatedDataValid([NotNull] Entity entity, [NotNull] TerrainTileEntityComponent component, [NotNull] TerrainTileRenderObject associatedData)
    {
      return component.RenderGroup == associatedData.RenderGroup;
    }

    protected override TerrainTileRenderObject GenerateComponentData([NotNull] Entity entity, [NotNull] TerrainTileEntityComponent component)
    {
      return new TerrainTileRenderObject()
      {
        Size = component.Size,
        AllowTerrainTransparency = component.AllowTerrainTransparency,
        BlendMap = component.BlendMap,
        DirtTexture = component.DirtTexture,
        GrassTexture = component.GrassTexture,
        Enabled = component.Enabled,
        RenderGroup = component.RenderGroup,
        RockTexture = component.RockTexture,
        SandTexture = component.SandTexture,
        HeightData = component?.HeightSource?.HeightData
      };
    }

    public override void Update(GameTime time)
    {
      foreach (var data in ComponentDatas)
      {
        var component = data.Key;
        var renderObject = data.Value;

        if (component.Enabled)
        {
          renderObject.Update(component);          

          if (renderObject.TerrainGeometry != null )
          {
            if (!component.IsGeometryProcessed)
            {
              BuildTerrainMesh(component.Entity, component, renderObject);
              component.IsGeometryProcessed = true;
            }

            if (!component.IsColliderProcessed)
            {
              BuildTerrainCollider(component.Entity, component, renderObject);
              component.IsColliderProcessed = true;
            }
          }
        }
      }

      base.Update(time);
    }

    protected virtual void BuildTerrainMesh(Entity entity, TerrainTileEntityComponent component, TerrainTileRenderObject renderObject)
    {
      if (component.Entity.Get<ModelComponent>() is ModelComponent existingTerrainModelComponent)
      {
        component.Entity.Remove(existingTerrainModelComponent);
      }

      var terrainMesh = new Mesh(renderObject.TerrainGeometry.ToMeshDraw(), new ParameterCollection());
      var terrainModel = new Model() { Meshes = new List<Mesh>(new[] { terrainMesh }) };
      var terrainModelComponent = new ModelComponent(terrainModel);

      terrainModelComponent.Materials.Add(0, renderObject.TerrainMaterial);
      entity.Add(terrainModelComponent);      
    }

    protected virtual void BuildTerrainCollider(Entity entity, TerrainTileEntityComponent component, TerrainTileRenderObject renderObject)
    {
      if (component.Entity.Get<StaticColliderComponent>() is StaticColliderComponent existingCollider)
      {
        component.Entity.Remove(existingCollider);
      }

      entity.Add(new TerrainTileColliderBuilder().BuildCollider(component.HeightSource.HeightData, component.Size, component.MaxHeight));
    }

    public override void Draw(RenderContext context)
    {
      foreach (var data in ComponentDatas)
      {
        var component = data.Key;
        var renderObject = data.Value;

        if (component.Enabled)
        {
          if (component.Entity.Get<ModelComponent>() is ModelComponent terrainModel && terrainModel.Materials.Count > 0)
          {
            renderObject.Draw(terrainModel.Materials[0].Passes[0].Parameters);
          }

          VisibilityGroup.RenderObjects.Add(renderObject);
        }
        else
        {
          VisibilityGroup.RenderObjects.Remove(renderObject);
        }
      }
    }
  }
}