using System;
using System.Collections.Generic;
using System.Linq;
using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Engine;

namespace XenkoTerrain
{
  public static class Extensions
  {
    public static Action<T> ToAction<T, T2>(this Func<T, T2> tryExpression)
    {
      return (T arg) => tryExpression.Invoke(arg);
    }

    public static bool Has<T>(this Entity self) where T : EntityComponent
    {
      return self.Get<T>() != default;
    }

    public static bool TryGet<T>(this Entity self, out T component) where T : EntityComponent
    {
      return (component = self.Get<T>()) != default;
    }

    public static bool TryGet<T>(this Entity self, out T component, params Func<T, bool>[] callbacks) where T : EntityComponent
    {
      return self.TryGet(out component, callbacks.Select(ToAction).ToArray());
    }

    public static bool TryGet<T>(this Entity self, out T component, params Action<T>[] callbacks) where T : EntityComponent
    {
      if (self.TryGet(out component))
      {
        foreach (var callback in callbacks)
        {
          callback?.Invoke(component);
        }
      }

      return component != default;
    }

    public static bool TryRemove<T>(this Entity self) where T : EntityComponent
    {
      return self.TryRemove<T>(out _);
    }

    public static bool TryRemove<T>(this Entity self, out T clearedComponent) where T : EntityComponent
    {
      return self.TryGet(out clearedComponent, self.Remove);
    }

    public static bool ReplaceOrAdd<T>(this Entity self, T component, out T replacedComponent) where T : EntityComponent
    {
      var cleared = self.TryRemove(out replacedComponent);
      self.Add(component);
      return cleared;
    }
      
    public static float ToRgb(this Color self)
    {
      return (self.A << 24) | (self.R << 16) | (self.G << 8) | (self.B << 0);
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