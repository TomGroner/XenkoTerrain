using System.Collections.Generic;
using Xenko.Core.Annotations;
using Xenko.Engine;
using Xenko.Extensions;
using Xenko.Games;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileProcessor : EntityProcessor<TerrainTileComponent, TerrainTileRenderObject>, IEntityComponentRenderProcessor
  {
    public VisibilityGroup VisibilityGroup { get; set; }

    protected override bool IsAssociatedDataValid([NotNull] Entity entity, [NotNull] TerrainTileComponent component, [NotNull] TerrainTileRenderObject associatedData)
    {
      return component.RenderGroup == associatedData.RenderGroup;
    }

    protected override TerrainTileRenderObject GenerateComponentData([NotNull] Entity entity, [NotNull] TerrainTileComponent component)
    {
      return new TerrainTileRenderObject()
      {
        Size = component.Size,
        Enabled = component.Enabled,
        RenderGroup = component.RenderGroup,
        HeightMap = component.HeightMap,
        World = entity.Transform.WorldMatrix
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

          if (renderObject.Geometry != null && !component.IsGeometryProcessed)
          {
            if (component.Entity.Get<ModelComponent>() is ModelComponent existing)
            {
              component.Entity.Remove(existing);
            }            

            var meshDraw = renderObject.Geometry.ToMeshDraw();           
            var terrainMesh = new Mesh(meshDraw, component.Material.Passes[0].Parameters);
            var terrainModel = new Model() { Meshes = new List<Mesh>(new[] { terrainMesh }), };
            var terrainModelComponent = new ModelComponent(terrainModel);           
            component.Entity.Add(terrainModelComponent);
            terrainModelComponent.Materials.Add(0, renderObject.Material);
            component.IsGeometryProcessed = true;
          }
        }
      }

      base.Update(time);
    }

    public override void Draw(RenderContext context)
    {
      foreach (var data in ComponentDatas)
      {
        var component = data.Key;
        var renderObject = data.Value;

        if (component.Enabled)
        {
          VisibilityGroup.RenderObjects.Add(renderObject);

          if (component.Entity.Get<ModelComponent>() is ModelComponent existing)
          {
            //component.Material.Passes[0].Parameters.Set(MaterialSur)
          }

        }
        else
        {
          VisibilityGroup.RenderObjects.Remove(renderObject);
        }
      }
    }
  }
}