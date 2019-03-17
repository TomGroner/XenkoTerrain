using Xenko.Core.Mathematics;
using Xenko.Input;

namespace XenkoTerrain.TerrainSystem
{
  public class FlattenTerrainCommand : ModificationCommand
  {
    public FlattenTerrainCommand()
    {
      Name = "Flatten Terrain";
      Description = "Ctrl + LeftClick";
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return !context.DataModified &&        
             context.Input.IsMouseButtonDown(MouseButton.Left) && 
             context.Input.IsKeyDown(Keys.LeftCtrl);
    }

    public override void Execute(ModificationCommandContext context)
    {
      var closestVertVal = float.MaxValue;
      var closestVertHeight = 0.0f;

      for (int i = 0; i < context.Data.Vertices.Length; ++i)
      {
        ref var v = ref context.Data.Vertices[i].Position;
        float dist = (v.XZ() - context.Input.MousePosition).Length();

        if (dist <= context.Modifier.Radius && dist <= closestVertVal)
        {
          closestVertVal = dist;
          closestVertHeight = v.Y;
        }
      }

      for (int i = 0; i < context.Data.Vertices.Length; ++i)
      {
        ref var v = ref context.Data.Vertices[i].Position;
        var dist = (v.XZ() - context.GetLocalHitPosition()).Length();

        if (dist <= context.Modifier.Radius)
        {
          v.Y = closestVertHeight;
        }
      }

      context.DataModified = true;
    }
  }
}