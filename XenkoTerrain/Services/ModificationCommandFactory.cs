using XenkoTerrain.TerrainSystem;

namespace XenkoTerrain.Services
{
  public class ModificationCommandFactory : IModificationCommandFactory
  {
    public ModificationCommandCollection GetCommands()
    {
      return new ModificationCommandCollection()
      {
        new SaveTerrainCommand(),
        new DecreaseBrushRadiusCommand(),
        new IncreaseBrushRadiusCommand(),
        new FlattenTerrainCommand(),
        new SmoothTerrainCommand(),
        new RaiseTerrainCommand(),
        new LowerTerrainCommand(),
        new FixNormalsCommand()
      };
    }
  }
}