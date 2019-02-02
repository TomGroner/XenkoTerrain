using Xenko.Core;
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

  public static class IServiceRegistryExtensions
  {
    public static bool TryGetService<T>(this IServiceRegistry services, out T service) where T : class
    {
      return (service = services.GetService<T>()) != null;
    }
  }
}