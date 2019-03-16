using Xenko.Input;
using XenkoTerrain.Services;

namespace XenkoTerrain.TerrainSystem
{
  public class SaveTerrainCommand : ModificationCommand
  {
    public SaveTerrainCommand()
    {
      Name = "Save";
      Description = "Ctrl + S";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return context.Input.IsKeyDown(Keys.LeftCtrl) && 
             context.Input.IsKeyDown(Keys.S);
    }

    public override async void Execute(ModificationCommandContext context)
    {
      if (context.Services.TryGetService<SaveTerrainService>(out var saveService))
      {
        await saveService.SaveAsync(context.Data);
      }
    }
  }
}