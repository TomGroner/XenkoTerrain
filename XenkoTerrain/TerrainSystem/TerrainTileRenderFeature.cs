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
        if (renderObject is TerrainTileRenderObject tile && tile.HeightMap?.Width > 0)
        {
          tile.Build(context);
        }
      }
    }
  }
}