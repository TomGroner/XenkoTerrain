using Xenko.Graphics;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainHeightSourceRenderObject : RenderObject
  {
    public Texture HeightMap;
    public TerrainHeightDataRepository HeightData;

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

    protected bool TryGetHeightMapImageData(CommandList commandList, out TerrainHeightDataRepository pixels)
    {
      if (HeightMap?.Width > 0)
      {
        pixels = new TerrainHeightDataRepository(HeightMap.GetDataAsImage(commandList).PixelBuffer[0]);     
      }
      else
      {
        pixels = default;
      }

      return pixels != null;
    }
  }
}