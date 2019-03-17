using Xenko.Core.Mathematics;
using Xenko.Input;

namespace XenkoTerrain.TerrainSystem
{
  public class LowerTerrainCommand : ModificationCommand
  {
    public LowerTerrainCommand()
    {
      Name = "Lower Terrain";
      Description = "Middle Mouse";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return !context.DataModified && context.Input.IsMouseButtonDown(MouseButton.Middle);
    }

    public override void Execute(ModificationCommandContext context)
    {
      for (int i = 0; i < context.Data.Vertices.Length; ++i)
      {
        ref var position = ref context.Data.Vertices[i].Position;
        var dist = (position.XZ() - context.GetLocalHitPosition()).Length();
        var factor = context.Modifier.Radius - dist;

        if (factor >= 0.0f)
        {
          position.Y -= factor * context.DeltaTime;
        }
      }

      context.DataModified = true;
    }
  }
}