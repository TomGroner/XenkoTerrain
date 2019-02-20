using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainGeometryData : GeometricMeshData<VertexPositionNormalTexture>
  {
    public TerrainGeometryData(float size, float tessellationX, float tessellationY, VertexPositionNormalTexture[] vertices, int[] indices, bool isLeftHanded) : base(vertices, indices, isLeftHanded)
    {
      Size = size;
      TessellationX = tessellationX;
      TessellationY = tessellationY;
    }

    public float Size { get; set; }

    public float TessellationX { get; set; }

    public float TessellationY { get; set; }
  }
}