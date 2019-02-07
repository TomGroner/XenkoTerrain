using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Packs the color's 4 byte-channel to a float.
    /// </summary>        
    public static float ToRgb(this Color self)
    {
      return
        (self.A << 24)
        | (self.R << 16)
        | (self.G << 8)
        | (self.B << 0);
    }

    public static bool SearchForComponent<T>(this Entity startWithEntity, out T component) where T : EntityComponent
    {
      return (component = startWithEntity.SearchForComponent<T>()) != null;
    }

    public static T SearchForComponent<T>(this Entity startWithEntity) where T : EntityComponent
    {
      var entitiesToSearch = new Queue<Entity>(new Entity[] { startWithEntity });

      while (entitiesToSearch.Count > 0)
      {
        var searchEntity = entitiesToSearch.Dequeue();        

        if (searchEntity.TryGet<T>(out var component))
        {
          return component;
        }

        foreach (var childEntity in searchEntity.GetChildren())
        {
          entitiesToSearch.Enqueue(childEntity);
        }
      }

      return null;
    }

    public static bool TryGetService<T>(this IServiceRegistry services, out T service) where T : class
    {
      return (service = services.GetService<T>()) != null;
    }

    public static bool TryGet<T>(this Entity entity, out T component) where T : EntityComponent
    {
      return (component = entity?.Get<T>()) != null;
    }

    public static Vector3 GetNormalized(this Vector3 self)
    {
      return Vector3.Normalize(self);
    }

    public static Vector3 GetLerp(this Vector3 self, Vector3 other, float alpha)
    {
      return Vector3.Lerp(self, other, alpha);
    }

    public static Vector3 GetRotationVector(this Vector3 self, TransformComponent transform)
    {
      return self.GetRotationVector(transform.Rotation);
    }

    public static Vector3 GetRotationVector(this Vector3 self, Quaternion rotation)
    {
      return self.GetRotationVector(Matrix.RotationQuaternion(rotation));
    }

    public static Vector3 GetRotationVector(this Vector3 self, Matrix rotation)
    {
      return Vector3.TransformCoordinate(self, rotation);
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
  }
}