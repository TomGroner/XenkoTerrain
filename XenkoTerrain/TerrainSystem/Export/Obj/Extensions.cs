using Xenko.Core.Mathematics;

namespace XenkoTerrain.TerrainSystem.Export.Obj
{
  public static class Extensions
  {
    public static string PrintObj(this Vector2 self, ObjLineType lineType)
    {
      return $"{self.X:0.00000} {self.Y:0.00000}";
    }

    public static string PrintObj(this Vector3 self, ObjLineType lineType)
    {
      return $"{self.X:0.00000} {self.Y:0.00000} {self.Z:0.00000}";
    }
  }
}