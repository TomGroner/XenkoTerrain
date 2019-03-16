using Xenko.Core.Mathematics;
using Xenko.Input;

namespace XenkoTerrain.TerrainSystem
{
  public class RaiseTerrainCommand : ModificationCommand
  {
    public RaiseTerrainCommand()
    {
      Name = "Raise Terrain";
      Description = "Click";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return !context.DataModified && 
             context.Hit.Succeeded &&
             context.Input.IsMouseButtonDown(MouseButton.Left);
    }

    public override void Execute(ModificationCommandContext context)
    {
      for (int i = 0; i < context.Data.Vertices.Length; ++i)
      {
        ref var position = ref context.Data.Vertices[i].Position;
        var dist = (position.XZ() - context.GetWorldHitPosition()).Length();
        var factor = context.Modifier.Radius - dist;

        if (factor >= 0.0f)
        {
          position.Y += factor * context.DeltaTime;
        }
      }

      context.DataModified = true;
    }
  }
}