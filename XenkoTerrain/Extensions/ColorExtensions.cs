using Xenko.Core.Mathematics;

namespace XenkoTerrain.Extensions
{
  public static class ColorExtensions
  {
    public static float ToRgb(this Color self)
    {
      return
        (self.A << 24)
        | (self.R << 16)
        | (self.G << 8)
        | (self.B << 0);
    }
  }
}