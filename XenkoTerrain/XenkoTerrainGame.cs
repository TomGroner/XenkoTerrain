using System.IO;
using System.Threading.Tasks;
using Xenko.Engine;
using Xenko.Graphics;
using XenkoTerrain.Graphics;

namespace XenkoTerrain
{
  public class XenkoTerrainGame : Game
  {
    //public static RgbPixelRepository HeightMapData;
    //public static bool HeightMapDataReady = false;

    //protected async override Task LoadContent()
    //{
    //  await base.LoadContent();

    //  var heightMapTexturePath = @"T:\Storage\Projects\TomGroner\XenkoTerrain\XenkoTerrain\Resources\heightmap.png";

    //  using (var stream = new FileStream(heightMapTexturePath, FileMode.Open, FileAccess.Read, FileShare.Read))
    //  {
    //    var texture = Texture.Load(GraphicsDevice, stream);
    //    var image = texture.GetDataAsImage(GraphicsContext.CommandList);
    //    HeightMapData = new RgbPixelRepository(image.PixelBuffer[0]);
    //    HeightMapDataReady = true;
    //  }
    //}
  }
}