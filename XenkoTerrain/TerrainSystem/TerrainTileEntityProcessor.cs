using System.Collections.Generic;
using Xenko.Core.Annotations;
using Xenko.Engine;
using Xenko.Extensions;
using Xenko.Games;
using Xenko.Rendering;
using XenkoTerrain.Graphics;

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
          if (renderObject.Enabled != component.Enabled)
          {
            renderObject.Enabled = component.Enabled;
          }

          if(renderObject.AllowTerrainTransparency != component.AllowTerrainTransparency)
          {
            renderObject.AllowTerrainTransparency = component.AllowTerrainTransparency;
          }

          if (renderObject.BlendMap != component.BlendMap)
          {
            renderObject.BlendMap = component.BlendMap;
          }

          if (renderObject.SandTexture != component.SandTexture)
          {
            renderObject.SandTexture = component.SandTexture;
          }

          if (renderObject.DirtTexture != component.DirtTexture)
          {
            renderObject.DirtTexture = component.DirtTexture;
          }

          if (renderObject.GrassTexture != component.GrassTexture)
          {
            renderObject.GrassTexture = component.GrassTexture;
          }

          if (renderObject.RockTexture != component.RockTexture)
          {
            renderObject.RockTexture = component.RockTexture;
          }

          if (renderObject.RenderGroup != component.RenderGroup)
          {
            renderObject.RenderGroup = component.RenderGroup;
          }

          if (renderObject.Size != component.Size)
          {
            component.IsGeometryProcessed = false;
            renderObject.TerrainGeometry = null;
            renderObject.Size = component.Size;
          }

          if (renderObject.MaxHeight != component.MaxHeight)
          {
            component.IsGeometryProcessed = false;
            renderObject.TerrainGeometry = null;
            renderObject.MaxHeight = component.MaxHeight;
          }

          if (component?.HeightSource?.HeightData is RgbPixelRepository heightData && renderObject.HeightData != heightData)
          {
            component.IsGeometryProcessed = false;
            renderObject.TerrainGeometry = null;
            renderObject.HeightData = heightData;
          }

          if (renderObject.TerrainGeometry != null && !component.IsGeometryProcessed)
          {
            if (component.Entity.Get<ModelComponent>() is ModelComponent existingTerrainModelComponent)
            {
              component.Entity.Remove(existingTerrainModelComponent);
            }

            var terrainMesh = new Mesh(renderObject.TerrainGeometry.ToMeshDraw(), new ParameterCollection());
            var terrainModel = new Model() { Meshes = new List<Mesh>(new[] { terrainMesh }) };
            var terrainModelComponent = new ModelComponent(terrainModel);

            terrainModelComponent.Materials.Add(0, renderObject.TerrainMaterial);
            component.Entity.Add(terrainModelComponent);
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
          if (component.Entity.Get<ModelComponent>() is ModelComponent terrainModel && terrainModel.Materials.Count > 0)
          {
            var parameters = terrainModel.Materials[0].Passes[0].Parameters;
            parameters.Set(TerrainTileShaderKeys.BlendMap, renderObject.BlendMap);
            parameters.Set(TerrainTileShaderKeys.SandTexture, renderObject.SandTexture);
            parameters.Set(TerrainTileShaderKeys.DirtTexture, renderObject.DirtTexture);
            parameters.Set(TerrainTileShaderKeys.GrassTexture, renderObject.GrassTexture);
            parameters.Set(TerrainTileShaderKeys.RockTexture, renderObject.RockTexture);
            parameters.Set(TerrainTileShaderKeys.TextureScale, renderObject.Size);
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