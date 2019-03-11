using Xenko.Engine;
using XenkoTerrain.Services;

namespace XenkoTerrain
{
  public class XenkoTerrainGame : Game
  {
    protected override void Initialize()
    {
      base.Initialize();

      if (Services.TryGetService<CustomGraphicsSettings>(out var service))
      {
        Services.GetService<CustomGraphicsSettings>().SetMaximizedReziableWindow();
      }
    }
  }
}