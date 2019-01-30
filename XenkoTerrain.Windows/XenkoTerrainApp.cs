namespace XenkoTerrain.Windows
{
  internal class XenkoTerrainApp
  {
    private static void Main(string[] args)
    {
      using (var game = new XenkoTerrainGame())
      {
        game.Run();
      }
    }
  }
}