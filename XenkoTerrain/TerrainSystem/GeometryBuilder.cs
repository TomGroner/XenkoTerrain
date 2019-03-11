using System.Drawing;
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;

namespace XenkoTerrain.TerrainSystem
{

  public class GeometryBuilder
  {
    private HeightDataSource heightData;

    public GeometryBuilder(HeightDataSource data)
    {
      heightData = data;
    }

    public virtual GeometricPrimitive BuildTerrainGeometricPrimitive(GraphicsDevice graphicsDevice, float size, float maxHeight, Vector2 uv)
    {
      var data = BuildTerrainData(size, maxHeight, uv);
      var terrainGeometry = new GeometricPrimitive(graphicsDevice, data);
      return terrainGeometry;
    }

    public virtual GeometryData BuildTerrainData(float size, float maxHeight, Vector2 uv)
    {
      var tessellationX = heightData.Columns;
      var tessellationY = heightData.Rows;
      var columnCount = (tessellationX + 1);
      var rowCount = (tessellationY + 1);
      var vertices = new VertexPositionNormalTexture[columnCount * rowCount];
      var indices = new int[tessellationX * tessellationY * 6];
      var deltaX = size / tessellationX;
      var deltaY = size / tessellationY;

      size /= 2.0f;

      var vertexCount = 0;
      var indexCount = 0;

      for (var y = 0; y < (tessellationY + 1); y++)
      {
        for (var x = 0; x < (tessellationX + 1); x++)
        {
          var height = heightData.GetTerrainHeight(x, y, maxHeight);
          var position = new Vector3(-size + deltaX * x, height, -size + deltaY * y);
          var normal = heightData.GetNormal(x, y, maxHeight);
          var texCoord = new Vector2(uv.X * x / tessellationX, uv.Y * y / tessellationY);
          vertices[vertexCount++] = new VertexPositionNormalTexture(position, normal, texCoord);
        }
      }

      // Create indices
      for (var y = 0; y < tessellationY; y++)
      {
        for (var x = 0; x < tessellationX; x++)
        {
          // Six indices (two triangles) per face.
          var vbase = columnCount * y + x;
          indices[indexCount++] = (vbase + 1);
          indices[indexCount++] = (vbase + 1 + columnCount);
          indices[indexCount++] = (vbase + columnCount);
          indices[indexCount++] = (vbase + 1);
          indices[indexCount++] = (vbase + columnCount);
          indices[indexCount++] = (vbase);
        }
      }

      return new GeometryData(size * 2, rowCount, columnCount, vertices, indices, false)
      {
        Name = "Terrain",
        HeightSource = heightData
      };
    }
  }
}