using Xenko.Core.Mathematics;
using Xenko.Graphics;
using XenkoTerrain.Extensions;

namespace XenkoTerrain.Graphics
{
  public class RgbPixelRepository
  {
    private readonly float[] pixels;
    
    public RgbPixelRepository(PixelBuffer pixelBuffer)
    {
      Width = pixelBuffer.Width;
      Height = pixelBuffer.Height;
      pixels = new float[pixelBuffer.Width * pixelBuffer.Height];
      Initialize(pixelBuffer);
    }

    public int Width { get; set; }

    public int Height { get; set; }

    private void Initialize(PixelBuffer pixelBuffer)
    {
      for (var y = 0; y < Height; y++)
      {
        for (var x = 0; x < Width; x++)
        {
          SavePixel(x, y, pixelBuffer.GetPixel<Color>(x, y).ToRgb());
        }
      }
    }

    private int GetPixelIndex(int x, int y)
    {
      return y * Height + x;
    }

    public bool HaveData(int x, int y)
    {
      return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public float GetPixel(int x, int y)
    {
      return pixels[GetPixelIndex(x, y)];
    }

    public void SavePixel(int x, int y, float pixel)
    {
      pixels[GetPixelIndex(x, y)] = pixel;
    }
  }
}