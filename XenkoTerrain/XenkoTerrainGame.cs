using System;
using System.Threading.Tasks;
using Xenko.Core;
using Xenko.Core.Mathematics;
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

  public static class XenkoTerrainGameGlobalExtensions
  {
    public static Vector3 Normalized(this Vector3 self)
    {
      return Vector3.Normalize(self);
    }

    public static Vector3 MoveTowards(this Vector3 current, Vector3 target, float maxDistanceDelta)
    {
      var a = target - current;
      var magnitude = a.Length();

      if (magnitude <= maxDistanceDelta || magnitude == 0f)
      {
        return target;
      }

      return current + a / magnitude * maxDistanceDelta;
    }

    public static bool TryGetService<T>(this IServiceRegistry services, out T service) where T : class
    {
      return (service = services.GetService<T>()) != null;
    }

    public static bool TryGet<T>(this Entity entity, out T component) where T : EntityComponent
    {
      return (component = entity?.Get<T>()) != null;
    }
  }
}