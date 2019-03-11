using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Graphics;
using Xenko.Physics;

namespace XenkoTerrain.TerrainSystem
{
  public class Utils
  {
    public static HitResult ScreenPositionToWorldPositionRaycast(Vector2 screenPos, CameraComponent camera, Simulation simulation)
    {
      var invViewProj = Matrix.Invert(camera.ViewProjectionMatrix);

      // Reconstruct the projection-space position in the (-1, +1) range.
      //    Don't forget that Y is down in screen coordinates, but up in projection space
      Vector3 sPos;
      sPos.X = screenPos.X * 2f - 1f;
      sPos.Y = 1f - screenPos.Y * 2f;

      // Compute the near (start) point for the raycast
      // It's assumed to have the same projection space (x,y) coordinates and z = 0 (lying on the near plane)
      // We need to unproject it to world space
      sPos.Z = 0f;
      var vectorNear = Vector3.Transform(sPos, invViewProj);
      vectorNear /= vectorNear.W;

      // Compute the far (end) point for the raycast
      // It's assumed to have the same projection space (x,y) coordinates and z = 1 (lying on the far plane)
      // We need to unproject it to world space
      sPos.Z = 1f;
      var vectorFar = Vector3.Transform(sPos, invViewProj);
      vectorFar /= vectorFar.W;

      return simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ()); ;
    }

    public static BoundingBox FromPoints(VertexPositionNormalTexture[] verts)
    {
      var min = new Vector3(float.MaxValue);
      var max = new Vector3(float.MinValue);

      for (int i = 0; i < verts.Length; ++i)
      {
        Vector3.Min(ref min, ref verts[i].Position, out min);
        Vector3.Max(ref max, ref verts[i].Position, out max);
      }

      return new BoundingBox(min, max);
    }
  }
}