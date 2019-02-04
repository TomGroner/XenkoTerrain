using System;
using Xenko.Rendering;

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
            var geometryBuilder = new TerrainGeometryBuilder(terrainTile.HeightData);
            var materialBuilder = new TerrainMaterialBuilder();
            terrainTile.TerrainGeometry = geometryBuilder.BuildTerrainGeometricPrimitive(context.GraphicsDevice, terrainTile.Size, terrainTile.MaxHeight);
            terrainTile.TerrainMaterial = materialBuilder.BuildTerrainMaterial(context.GraphicsDevice, terrainTile);
          }
        }
      }
    }
  }
}