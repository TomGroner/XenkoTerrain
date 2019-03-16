namespace XenkoTerrain.TerrainSystem
{
  public abstract class ModificationCommand : IModificationCommand
  {
    public string Name { get; protected set; }

    public string Description { get; protected set; }

    public bool IsOnScreen { get; protected set; }

    public abstract bool CanExecute(ModificationCommandContext context);

    public abstract void Execute(ModificationCommandContext context);
  }
}