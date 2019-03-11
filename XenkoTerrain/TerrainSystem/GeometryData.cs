using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class GeometryData : GeometricMeshData<VertexPositionNormalTexture>
  {
    public GeometryData(float size, float tessellationX, float tessellationY, VertexPositionNormalTexture[] vertices, int[] indices, bool isLeftHanded) : base(vertices, indices, isLeftHanded)
    {
      Data = new GeometricMeshData<VertexPositionNormalTexture>(vertices, indices, isLeftHanded);
      Size = size;
      TessellationX = tessellationX;
      TessellationY = tessellationY;
    }

    public GeometricMeshData<VertexPositionNormalTexture> Data { get; set; }

    public float Size { get; set; }

    public float TessellationX { get; set; }

    public float TessellationY { get; set; }
  }
}