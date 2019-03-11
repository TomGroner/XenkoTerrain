using System;
using System.Collections.Generic;
using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class HeightDataSource : IDisposable
  {
    public const float DefaultMaxPixelColor = 256 * 256 * 256;

    private float[] heights;

    public HeightDataSource(int columns, int rows, float[] data)
    {
      Columns = columns;
      Rows = rows;
      heights = data;
    }

    public HeightDataSource(PixelBuffer pixelBuffer)
    {
      Columns = pixelBuffer.Width;
      Rows = pixelBuffer.Height;
      heights = new float[pixelBuffer.Width * pixelBuffer.Height];

      foreach (var coord in GetCoordinates())
      {
        heights[coord.index] = pixelBuffer.GetPixel<Color>(coord.x, coord.y).ToRgb();
      }
    }

    public HeightDataSource(int columns, int rows, VertexPositionNormalTexture[] vertices)
    {
      Columns = columns;
      Rows = rows;
      heights = new float[columns * rows];

      foreach (var coord in GetCoordinates())
      {
        heights[coord.index] = vertices[coord.index].Position.Y;
      }
    }

    public float MaxPixelColor { get; set; } = DefaultMaxPixelColor;

    public int Columns { get; set; }

    public int Rows { get; set; }

    public float GetHeight(int x, int y)
    {
      return heights[GetHeightIndex(x, y)];
    }

    public Vector3 GetNormal(int x, int y, float maxHeight)
    {
      var heightL = GetTerrainHeight(x - 1, y, maxHeight);
      var heightR = GetTerrainHeight(x + 1, y, maxHeight);
      var heightD = GetTerrainHeight(x, y - 1, maxHeight);
      var heightU = GetTerrainHeight(x, y + 1, maxHeight);

      var normal = new Vector3(heightL - heightR, 2.0f, heightD - heightU);
      normal.Normalize();
      return normal / 3;
    }

    public bool IsValidCoordinate(int x, int y)
    {
      return x >= 0 && x < Columns && y >= 0 && y < Rows;
    }

    public int GetHeightIndex(int x, int y)
    {
      return y * Rows + x;
    }

    public float GetTerrainHeight(int x, int y, float maxHeight)
    {
      if (!IsValidCoordinate(x, y))
      {
        return 0.0f;
      }

      return heights[GetHeightIndex(x, y)] / MaxPixelColor * maxHeight + maxHeight;
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

    private IEnumerable<(int x, int y, int index)> GetCoordinates()
    {
      for (var y = 0; y < Rows; y++)
      {
        for (var x = 0; x < Columns; x++)
        {
          yield return (x, y, GetHeightIndex(x, y));
        }
      }
    }

    public void Dispose()
    {
      heights = null;
    }
  }
}