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

    public int MaxBrushSize { get; set; } = 16;

    public override bool CanExecute(ModificationCommandContext context)
    {
      return context.Input.IsKeyReleased(Keys.PageUp);
    }

    public override void Execute(ModificationCommandContext context)
    {
      context.Modifier.Radius++;

      if (context.Modifier.Radius > MaxBrushSize)
      {
        context.Modifier.Radius = MaxBrushSize;
      }
    }
  }
}