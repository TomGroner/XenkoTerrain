namespace XenkoTerrain.TerrainSystem
{
  public interface IModificationCommand
  {
    string Name { get; }

    string Description { get; }

    bool IsOnScreen { get; }

    bool CanExecute(ModificationCommandContext context);

    void Execute(ModificationCommandContext context);
  }
}