using XenkoTerrain.TerrainSystem;

namespace XenkoTerrain.Services
{
  public interface IModificationCommandFactory
  {
    ModificationCommandCollection GetCommands();
  }
}