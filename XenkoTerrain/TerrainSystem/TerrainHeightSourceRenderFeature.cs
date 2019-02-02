using System;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainHeightSourceRenderFeature : RootRenderFeature
  {
    public override Type SupportedRenderObjectType
    {
      get => typeof(TerrainHeightSourceRenderObject);
    }

    public override void Prepare(RenderDrawContext context)
    {
      base.Prepare(context);

      foreach (var renderObject in RenderObjects)
      {
        if (renderObject is TerrainHeightSourceRenderObject terainHeightSourceRenderObject)
        {
          terainHeightSourceRenderObject.Prepare(context);
        }
      }
    }
  }
}