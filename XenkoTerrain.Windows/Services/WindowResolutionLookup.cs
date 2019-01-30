using System.Linq;
using System.Windows.Forms;

namespace XenkoTerrain.Services
{
  public class WindowResolutionLookup : IWindowResolutionLookup
  {
    public static int FallbackMaxWidth = 800;
    public static int FallbackMaxHeight = 600;
    private static WindowResolutionLookup instance;

    private WindowResolutionLookup()
    {
    }

    static WindowResolutionLookup()
    {
      instance = new WindowResolutionLookup();
    }

    public bool TryDetermineMaximumResolution(out int maxWidth, out int maxHeight)
    {
      var primaryScreen = Screen.AllScreens.FirstOrDefault(screen => screen.Primary) ?? Screen.AllScreens.First();
      maxWidth = primaryScreen != null ? primaryScreen.WorkingArea.Width : FallbackMaxWidth;
      maxHeight = primaryScreen != null ? primaryScreen.WorkingArea.Height : FallbackMaxHeight;
      return primaryScreen != null;
    }
  }
}