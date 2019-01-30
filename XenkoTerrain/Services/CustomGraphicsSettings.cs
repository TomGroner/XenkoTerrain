using Xenko.Core.Mathematics;

namespace XenkoTerrain.Services
{
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
