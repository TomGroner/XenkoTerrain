using Xenko.Input;

namespace XenkoTerrain.TerrainSystem
{
  public class DecreaseBrushRadiusCommand : ModificationCommand
  {
    public DecreaseBrushRadiusCommand()
    {
      Name = "Decrease Radius";
      Description = "PageDown";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return context.Input.IsKeyReleased(Keys.PageDown);
    }

    public override void Execute(ModificationCommandContext context)
    {
      context.Modifier.Radius--;

      if (context.Modifier.Radius <= 0)
      {
        context.Modifier.Radius = 1;
      }
    }
  }
}