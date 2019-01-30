namespace XenkoTerrain.Services
{
  public interface IWindowResolutionLookup
  {
    bool TryDetermineMaximumResolution(out int maxWidth, out int maxHeight);
  }
}