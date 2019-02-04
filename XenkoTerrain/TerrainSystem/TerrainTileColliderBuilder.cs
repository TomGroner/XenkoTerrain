using System;
using Xenko.Core.Mathematics;
using Xenko.Physics;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileColliderBuilder
  {
    // TODO: use height field, or make it work with both?
    public StaticColliderComponent BuildCollider(TerrainHeightDataRepository geometryData, float size, float maxHeight)
    {
      var collisionData = geometryData.Simplify(4);
      var collider = new StaticColliderComponent();
      var boxColliderWidthHeight = size / collisionData.Rows;
      var halfBoxColliderWidthHeight = boxColliderWidthHeight / 2;
      var halfSize = size / 2 - halfBoxColliderWidthHeight;      

      for (var y = 0; y < collisionData.Rows; y++)
      {
        for (var x = 0; x < collisionData.Columns; x++)
        {
          var height = Math.Max(collisionData.GetTerrainHeight(x, y, maxHeight), 0.1f);
          var halfHeight = height / 2;
          var boxColliderSize = new Vector3(boxColliderWidthHeight, height, boxColliderWidthHeight);
          var offset = new Vector3(-halfSize + x * boxColliderWidthHeight, halfHeight, -halfSize + y * boxColliderWidthHeight);
          collider.ColliderShapes.Add(new BoxColliderShapeDesc() { Size = boxColliderSize, LocalOffset = offset });          
        }
      }

      return collider;
    }
  }
}
