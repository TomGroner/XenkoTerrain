using System;
using Xenko.Core.Mathematics;
using Xenko.Physics;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileColliderBuilder
  {
    public StaticColliderComponent BuildCollider(TerrainHeightDataRepository geometryData, float size, float maxHeight)
    {
      var collisionData = geometryData.Simplify(4);
      var collider = new StaticColliderComponent();
      var boxColliderSize = size / collisionData.Rows;         

      // have issue with using the size of the tile vs size of the map, usually get 0.5-1.5 units off it seems. think over in concept later

      for (var y = 0; y < collisionData.Rows; y++)
      {
        for (var x = 0; x < collisionData.Columns; x++)
        {
          var height = Math.Max(collisionData.GetTerrainHeight(x, y, maxHeight), 0.1f);

          collider.ColliderShapes.Add(new BoxColliderShapeDesc()
          {
            Size = new Vector3(boxColliderSize, height, boxColliderSize),
            LocalOffset = new Vector3(size / 2 - x * boxColliderSize, 0, size / 2 - y * boxColliderSize)
          });
        }
      }

      return collider;
    }
  }
}
