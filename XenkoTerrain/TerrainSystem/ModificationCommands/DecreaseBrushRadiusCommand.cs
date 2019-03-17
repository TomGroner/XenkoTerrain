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

    public int MinBrushSize { get; set; } = 1;

    public override bool CanExecute(ModificationCommandContext context)
    {
      return context.Input.IsKeyReleased(Keys.PageDown);
    }

    public override void Execute(ModificationCommandContext context)
    {
      context.Modifier.Radius--;

      if (context.Modifier.Radius <= MinBrushSize)
      {
        context.Modifier.Radius = MinBrushSize;
      }
    }
  }
}