using Xenko.Physics;
using Xenko.Physics.Shapes;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileHeightfieldColliderBuilder
  {
    // This version of collision does work, but it's got some oddities that I am trying to sort out. There is a PR to the engine
    // that will add debug rendering to height field colliders that will help sort it out. For now, using the generic box collider method 
    public StaticColliderComponent BuildCollider(TerrainHeightDataRepository heightData, float size, float maxHeight)
    {
      var collider = new StaticColliderComponent();
      collider.ColliderShapes.Add(new BoxColliderShapeDesc());
      collider.ColliderShape = CreateHeightfield(heightData, maxHeight);
      return collider;
    }

    public HeightfieldColliderShape CreateHeightfield(TerrainHeightDataRepository heightData, float maxHeight, float heightScaling = 1.0f)
    {
      return new HeightfieldColliderShape(
        heightData.Columns,
        heightData.Rows,
        UnmanagedArrayBuilder.New(heightData.GetAllTerrainHeights(maxHeight)),
        heightScaling,
        -maxHeight,
        maxHeight,
        false);
    }
  }
}