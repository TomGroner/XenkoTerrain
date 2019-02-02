using Xenko.Graphics;
using Xenko.Rendering;
using XenkoTerrain.Graphics;

// Other idea for things to break out
// 1. A component just to hold the terrain texture2
// 1b. Or instead, A component that has a full material to use instead of building it?

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainHeightSourceRenderObject : RenderObject
  {
    public Texture HeightMap;
    public RgbPixelRepository HeightData;

    public void Prepare(RenderDrawContext context)
    {
      if (HeightDataNeedsRebuilt() && TryGetHeightMapImageData(context.CommandList, out var heightData))
      {
        HeightData = heightData;
      }
    }

    private bool HeightDataNeedsRebuilt()
    {
      return HeightMap != null && HeightData == null;
    }

    protected bool TryGetHeightMapImageData(CommandList commandList, out RgbPixelRepository pixels)
    {
      if (HeightMap?.Width > 0)
      {
        pixels = new RgbPixelRepository(HeightMap.GetDataAsImage(commandList).PixelBuffer[0]);     
      }
      else
      {
        pixels = default;
      }

      return pixels != null;
    }
  }
}