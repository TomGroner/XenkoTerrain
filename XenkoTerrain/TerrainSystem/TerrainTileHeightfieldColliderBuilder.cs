using Xenko.Physics;
using Xenko.Physics.Shapes;

namespace XenkoTerrain.TerrainSystem
{

  /*
  IDea:
  If the change in height is under X, where X is some ratio of a step heigh from a character, then make the height the step height
  If the change in height is over X, then make the collider exactly the height. 
  goal: try to make it so all changes are gradual, but then clearly higher parts are walls
    */

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