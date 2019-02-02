using System;
using Xenko.Rendering;
using XenkoTerrain.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileRenderFeature : RootRenderFeature
  {
    public override Type SupportedRenderObjectType => typeof(TerrainTileRenderObject);   

    public override void Prepare(RenderDrawContext context)
    {
      base.Prepare(context);

      foreach (var renderObject in RenderObjects)
      {
        if (renderObject is TerrainTileRenderObject terrainTile)
        {
          if (terrainTile.Enabled && terrainTile.TerrainGeometry == null && terrainTile.HeightData != null)
          {
            var geometryBuilder = new TerrainGeometryBuilder(context.GraphicsDevice, terrainTile.Size, terrainTile.HeightData, terrainTile.MaxHeight);
            var materialBuilder = new TerrainMaterialBuilder(context.GraphicsDevice);
            terrainTile.TerrainGeometry = geometryBuilder.BuildTerrain();
            terrainTile.TerrainMaterial = materialBuilder.BuildTerrainMaterial(terrainTile);
          }
        }
      }
    }
  }
}