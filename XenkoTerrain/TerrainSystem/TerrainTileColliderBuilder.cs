using System;
using System.Linq;
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
      var boxColliderWidthHeight = size / collisionData.Rows;
      var halfBoxColliderWidthHeight = boxColliderWidthHeight / 2;
      var halfSize = size / 2 - halfBoxColliderWidthHeight;

      for (var y = 0; y < collisionData.Rows; y++)
      {
        for (var x = 0; x < collisionData.Columns; x++)
        {
          var height = GetAverageHeightAt(x, y);
          var halfHeight = height / 2;
          var boxColliderSize = new Vector3(boxColliderWidthHeight, height, boxColliderWidthHeight);
          var offset = new Vector3(-halfSize + x * boxColliderWidthHeight, halfHeight, -halfSize + y * boxColliderWidthHeight);
          collider.ColliderShapes.Add(new BoxColliderShapeDesc() { Size = boxColliderSize, LocalOffset = offset });
        }
      }

      return collider;

      float GetAverageHeightAt(int x, int y)
      {    
        var heightsInArea = new[]
        {   
          GetHeight(x - 1, y),   // Left
          GetHeight(x-1, x-1),   // Left Down
          GetHeight(x-1, x+1),   // Left Up         
          GetHeight(x + 1, y),   // Right
          GetHeight(x + 1, y-1), // Right Down
          GetHeight(x + 1, y+1), // Right Up
          GetHeight(x, y - 1),   // Down
          GetHeight(x, y + 1)    // Up
        };

        var heightAtPoint = GetHeight(x, y);
        var averageHeightInArea = heightsInArea.Average();

        if (heightAtPoint > averageHeightInArea || heightAtPoint < averageHeightInArea / 2)
        {
          return heightAtPoint;
        }       

        return GetHeight(x, y) * 0.75f + heightsInArea.Average() * 0.25f;
      }

      float GetHeight(int x, int y)
      {
        return Math.Max(collisionData.GetTerrainHeight(x, y, maxHeight), 0.1f);
      }
    }
  }
}