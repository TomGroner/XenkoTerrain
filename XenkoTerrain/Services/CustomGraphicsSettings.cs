using Xenko.Core.Mathematics;

namespace XenkoTerrain.Services
{
  /// <summary>
  /// Very basic "custom graphics settings" that just allows a full window resolution.
  /// </summary>
  public class CustomGraphicsSettings
  {
    private int? PrimaryScreenWidth;
    private int? PrimaryScreenHeight;

    public CustomGraphicsSettings(XenkoTerrainGame game, int? maxScreenWidth, int? maxScreenHeight)
    {
      Game = game;
      PrimaryScreenWidth = maxScreenWidth;
      PrimaryScreenHeight = maxScreenHeight;
    }

    public XenkoTerrainGame Game { get; }

    public void SetMaximizedReziableWindow()
    {
      Game.Window.AllowUserResizing = true;
      Game.Window.BeginScreenDeviceChange(true);
      Game.Window.EndScreenDeviceChange(PrimaryScreenWidth.Value, PrimaryScreenHeight.Value);
      Game.Window.Position = new Int2(0, 0);
    }
  }
}
