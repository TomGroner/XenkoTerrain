using Xenko.Core.Mathematics;
using Xenko.Input;

namespace XenkoTerrain.TerrainSystem
{
  public class SmoothTerrainCommand : ModificationCommand
  {
    public SmoothTerrainCommand()
    {
      Name = "Smooth";
      Description = "Left Shift + Click";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return !context.DataModified &&
             context.Input.IsMouseButtonDown(MouseButton.Left) &&
             context.Input.IsKeyDown(Keys.LeftShift);
    }

    public override void Execute(ModificationCommandContext context)
    {
      var totalHeights = 0.0f;

      for (int i = 0; i < context.Data.Vertices.Length; ++i)
      {
        ref var v = ref context.Data.Vertices[i].Position;
        var dist = (v.XZ() - context.Input.MousePosition).Length();
        if (dist <= context.Modifier.Radius)
        {
          totalHeights += v.Y;
        }
      }

      float avgHeight = totalHeights / context.Data.Vertices.Length;

      for (int i = 0; i < context.Data.Vertices.Length; ++i)
      {
        ref var v = ref context.Data.Vertices[i].Position;
        var dist = (v.XZ() - context.Input.MousePosition).Length();
        if (dist <= context.Modifier.Radius)
        {
          v.Y += (avgHeight - v.Y) * (context.Modifier.Radius - dist) * context.DeltaTime;
        }
      }

      context.DataModified = true;
    }
  }
}