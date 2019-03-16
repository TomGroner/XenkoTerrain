using Xenko.Input;

namespace XenkoTerrain.TerrainSystem
{
  public class IncreaseBrushRadiusCommand : ModificationCommand
  {
    public IncreaseBrushRadiusCommand()
    {
      Name = "Increase Radius";
      Description = "Page Up";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return context.Input.IsKeyReleased(Keys.PageUp);
    }

    public override void Execute(ModificationCommandContext context)
    {
      context.Modifier.Radius++;

      if (context.Modifier.Radius > 10)
      {
        context.Modifier.Radius = 10;
      }
    }
  }
}