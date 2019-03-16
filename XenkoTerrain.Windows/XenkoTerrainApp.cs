using System;
using XenkoTerrain.Services;

namespace XenkoTerrain.Windows
{
  internal class XenkoTerrainApp
  {
    [STAThread]
    private static void Main(string[] args)
    {
      using (var game = new XenkoTerrainGame())
      {
        if (new WindowResolutionLookup().TryDetermineMaximumResolution(out var width, out var height))
        {
          game.Services.AddService(new CustomGraphicsSettings(game, width, height));
        }

        game.Services.AddService(new SaveTerrainService());
        game.Run();
      }
    }
  }
}