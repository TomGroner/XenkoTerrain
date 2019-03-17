using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Input;
using Xenko.Physics;

namespace XenkoTerrain.TerrainSystem
{
  public class ModificationCommandContext
  {
    private Vector2? worldHitPosition;

    public InputManager Input { get; set; }

    public ITerrainTileModifier Modifier { get; set; }

    public float DeltaTime { get; set; }

    public GeometryData Data { get; set; }        

    public IServiceRegistry Services { get; set; }    

    public HitResult Hit { get; set; }    

    public bool DataModified { get; set; }

    public Matrix WorldMatrix { get; set; }

    public Vector2 GetLocalHitPosition()
    {
      if (worldHitPosition == null)
      {
        var world = WorldMatrix;
        var point = Hit.Point;
        Matrix.Invert(ref world, out Matrix worldMatrixInv);
        Vector3.Transform(ref point, ref worldMatrixInv, out Vector3 result);
        worldHitPosition = result.XZ();
      }

      return worldHitPosition.Value;
    }
  }
}