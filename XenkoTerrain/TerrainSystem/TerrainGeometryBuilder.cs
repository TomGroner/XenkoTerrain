using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainGeometryBuilder
  {
    private float?[] heightCache;
    private TerrainHeightDataRepository heightData;

    public TerrainGeometryBuilder(TerrainHeightDataRepository data)
    {
      heightData = data;
      heightCache = new float?[data.Rows * data.Columns];
    }

    public virtual GeometricPrimitive BuildTerrainGeometricPrimitive(GraphicsDevice graphicsDevice, float size, float maxHeight)
    {
      var data = BuildTerrainData(size, maxHeight, false);      
      return new GeometricPrimitive(graphicsDevice, data);
    }

    public virtual GeometricMeshData<VertexPositionNormalTexture> BuildTerrainData(float size, float maxHeight, bool generateBackFace)
    {
      var tessellationX = heightData.Columns;
      var tessellationY = heightData.Rows;
      var lineWidth = (tessellationX + 1);
      var lineHeight = (tessellationY + 1);
      var vertices = new VertexPositionNormalTexture[lineWidth * lineHeight * (generateBackFace ? 2 : 1)];
      var indices = new int[tessellationX * tessellationY * 6 * (generateBackFace ? 2 : 1)];
      var deltaX = size / tessellationX;
      var deltaY = size / tessellationY;

      size /= 2.0f;

      var vertexCount = 0;
      var indexCount = 0;

      // Create vertices
      var uv = new Vector2(1f, 1f);

      for (var y = 0; y < (tessellationY + 1); y++)
      {
        for (var x = 0; x < (tessellationX + 1); x++)
        {
          var height = x == 0 || y == 0 ? 0.0f : GetHeight(x, y, maxHeight); // TODO: replace 0.0f default with a minheight
          var position = new Vector3(-size + deltaX * x, height, -size + deltaY * y);
          var normal = GetNormal(x, y, maxHeight);
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
          var vbase = lineWidth * y + x;
          indices[indexCount++] = (vbase + 1);
          indices[indexCount++] = (vbase + 1 + lineWidth);
          indices[indexCount++] = (vbase + lineWidth);
          indices[indexCount++] = (vbase + 1);
          indices[indexCount++] = (vbase + lineWidth);
          indices[indexCount++] = (vbase);
        }
      }

      if (generateBackFace)
      {
        var numVertices = lineWidth * lineHeight;

        for (var y = 0; y < (tessellationY + 1); y++)
        {
          for (var x = 0; x < (tessellationX + 1); x++)
          {
            var baseVertex = vertices[vertexCount - numVertices];
            var position = new Vector3(baseVertex.Position.X, baseVertex.Position.Y, baseVertex.Position.Z);
            var texCoord = new Vector2(uv.X * x / tessellationX, uv.Y * y / tessellationY);
            var normal = baseVertex.Normal;
            vertices[vertexCount++] = new VertexPositionNormalTexture(position, -normal, texCoord);
          }
        }

        for (var y = 0; y < tessellationY; y++)
        {
          for (var x = 0; x < tessellationX; x++)
          {
            var vbase = lineWidth * (y + tessellationY + 1) + x;
            indices[indexCount++] = (vbase + 1);
            indices[indexCount++] = (vbase + lineWidth);
            indices[indexCount++] = (vbase + 1 + lineWidth);
            indices[indexCount++] = (vbase + 1);
            indices[indexCount++] = (vbase);
            indices[indexCount++] = (vbase + lineWidth);
          }
        }
      }

      return new GeometricMeshData<VertexPositionNormalTexture>(vertices, indices, false) { Name = "Terrain" };
    }

    private Vector3 GetNormal(int x, int y, float maxHeight)
    {
      // TODO: Pre-calculate all of these, or cache as they are calculated, and re-use.
      var heightL = GetHeight(x - 1, y, maxHeight);
      var heightR = GetHeight(x + 1, y, maxHeight);
      var heightD = GetHeight(x, y - 1, maxHeight);
      var heightU = GetHeight(x, y + 1, maxHeight);
      var normal = new Vector3(heightL - heightR, 2f, heightD - heightU);
      normal.Normalize();
      return normal;
    }

    private float GetHeight(int x, int y, float maxHeight) => heightData.GetTerrainHeight(x, y, maxHeight);
  }
}