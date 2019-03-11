using System;
using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class HeightDataSource : IDisposable
  {
    public const float DefaultMaxPixelColor = 256 * 256 * 256;

    private float[] pixels;

    public HeightDataSource(PixelBuffer pixelBuffer)
    {
      Columns = pixelBuffer.Width;
      Rows = pixelBuffer.Height;
      pixels = new float[pixelBuffer.Width * pixelBuffer.Height];
      Initialize(pixelBuffer);
    }

    public HeightDataSource(int columns, int rows, float[] data)
    {
      Columns = columns;
      Rows = rows;
      pixels = data;
    }

    public int Columns { get; set; }

    public int Rows { get; set; }

    public float MaxPixelColor { get; set; } = DefaultMaxPixelColor;

    private int GetPixelIndex(int x, int y)
    {
      return y * Rows + x;
    }

    public bool HaveData(int x, int y)
    {
      return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }

    public float[] GetAllHeightData()
    {
      return pixels;
    }

    public float GetTerrainHeight(int x, int y, float maxHeight)
    {
      return !HaveData(x, y) ? 0.0f : GetHeightData(x, y) / MaxPixelColor * maxHeight + maxHeight;
    }

    public float GetHeightData(int x, int y)
    {
      return pixels[GetPixelIndex(x, y)];
    }

    private void Initialize(PixelBuffer pixelBuffer)
    {
      for (var y = 0; y < Rows; y++)
      {
        for (var x = 0; x < Columns; x++)
        {
          SavePixel(x, y, pixelBuffer.GetPixel<Color>(x, y).ToRgb());
        }
      }

      void SavePixel(int x, int y, float pixel) => pixels[GetPixelIndex(x, y)] = pixel;
    }

    public float[] GetAllTerrainHeights(float maxHeight)
    {
      var n = 0;
      var heights = new float[Columns * Rows];

      for (var y = 0; y < Rows; y++)
      {
        for (var x = 0; x < Columns; x++)
        {
          heights[n++] = GetTerrainHeight(x, y, maxHeight);
        }
      }

      return heights;
    }

    public HeightDataSource Simplify(int factor)
    {
      var simplifiedColumnCount = Columns / factor;
      var simplifiedColumnRows = Rows / factor;
      var simplifiedData = new float[simplifiedColumnCount * simplifiedColumnRows];
      var n = 0;

      for (var y = 0; y < Rows; y += factor)
      {
        for (var x = 0; x < Columns; x += factor)
        {
          var gatheredHeight = 0.0f;
          var gatheredPointCount = 0;

          for (var gatherAtY = y; gatherAtY < y + factor; gatherAtY++)
          {
            for (var gatherAtX = x; gatherAtX < x + factor; gatherAtX++)
            {
              gatheredHeight += GetHeightData(x, y);
              gatheredPointCount++;
            }
          }

          simplifiedData[n++] = gatheredHeight / gatheredPointCount;
        }
      }

      return new HeightDataSource(simplifiedColumnCount, simplifiedColumnRows, simplifiedData);
    }

    public void Dispose()
    {
      pixels = null;
    }
  }
}