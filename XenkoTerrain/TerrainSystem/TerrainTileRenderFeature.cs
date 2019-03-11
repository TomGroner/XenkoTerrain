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
        if (renderObject is TerrainTileRenderObject tile)
        {
          tile.Prepare(context);

          if (tile.Material != null && tile.Material.Passes.Count > 0)
          {
            Context.StreamingManager?.StreamResources(tile.Material.Passes[0].Parameters);
          }
        }
      }
    }
  }
}