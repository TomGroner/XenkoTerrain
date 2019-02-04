using Xenko.Physics;
using Xenko.Physics.Shapes;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileColliderBuilder
  {
    public StaticColliderComponent BuildCollider(TerrainHeightDataRepository geometryData, float size, float maxHeight)
    {
      var n = 0;
      var heights = new float[geometryData.Columns * geometryData.Rows];      

      for (var y = 0; y < geometryData.Rows; y++)
      {
        for (var x = 0; x < geometryData.Columns; x++)
        {
          heights[n++] = geometryData.GetTerrainHeight(x, y, maxHeight);
        }
      }      

      var collider = new StaticColliderComponent();
      var heightFieldData = UnmanagedArrayBuilder.New(heights);
      collider.ColliderShapes.Add(new BoxColliderShapeDesc());
      collider.ColliderShape = new HeightfieldColliderShape(geometryData.Columns, geometryData.Rows, heightFieldData, 1f, -maxHeight, maxHeight, false);
      return collider;
    }
  }
}
