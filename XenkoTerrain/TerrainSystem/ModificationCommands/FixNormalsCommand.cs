using Xenko.Core.Mathematics;
using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class FixNormalsCommand : ModificationCommand
  {
    // By default we're only going to update normals around the point, and it's done per modify. However if they
    // manually run the command, we will re-calculate all of them.
    private bool isManualRun = false;

    public FixNormalsCommand()
    {
      Name = "Recalculate Normals";
      Description = "N";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      isManualRun = context.Input.IsKeyReleased(Xenko.Input.Keys.N);
      return context.DataModified || isManualRun;
    }

    public override void Execute(ModificationCommandContext context)
    {
      int x = 0;
      int y = 0;
      var data = context.Data;
      var wrapAtX = context.Data.HeightSource.Columns + 1;
      var stopAtY = context.Data.HeightSource.Rows + 1;
      var fixAt = context.GetLocalHitPosition();
      var fixFrom = fixAt.Subtract(context.Modifier.Radius*2);
      var fixTo = fixAt.Add(context.Modifier.Radius*2);      
      
      for (int i = 0; i < data.Vertices.Length; ++i)
      {
        if (isManualRun || IsInRange(data.Vertices[i].Position))
        {
          var heightL = x > 0 && i - 1 > 0 ? data.Vertices[i - 1].Position.Y : 0.0f;
          var heightR = x < wrapAtX && i + 1 < data.Vertices.Length ? data.Vertices[i + 1].Position.Y : 0.0f;
          var heightU = y > 0 && i + wrapAtX < data.Vertices.Length ? data.Vertices[i + wrapAtX].Position.Y : 0.0f;
          var heightD = y < stopAtY && i - wrapAtX > 0 ? data.Vertices[i - wrapAtX].Position.Y : 0.0f;
          var newNormal = new Vector3(heightL - heightR, 2.0f, heightD - heightU);
          newNormal.Normalize();

          ref var normal = ref data.Vertices[i].Normal;
          normal.X = newNormal.X;
          normal.Y = newNormal.Y;
          normal.Z = newNormal.Z;
        }
        
        x++;

        if (x == wrapAtX)
        {
          x = 0;
          y++;
        }
      }

      if (isManualRun)
      {
        context.DataModified = true;
      }

      bool IsInRange(Vector3 position)
      {
        return position.X >= fixFrom.X && position.X <= fixTo.X && 
               position.Y >= fixFrom.Y && position.Y <= fixTo.Y;
      }

    }
  }

  internal static class FixNormalsCommandHelperExtensions
  {
    public static Vector2 Subtract(this Vector2 vector, float amount)
    {
      return new Vector2(vector.X - amount, vector.Y - amount);
    }

    public static Vector2 Add(this Vector2 vector, float amount)
    {
      return new Vector2(vector.X + amount, vector.Y + amount);
    }
  }
}