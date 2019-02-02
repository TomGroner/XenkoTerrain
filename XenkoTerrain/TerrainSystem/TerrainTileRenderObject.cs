using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;
using XenkoTerrain.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileRenderObject : RenderObject
  {
    public float Size;
    public float MaxHeight;
    public Texture BlendMap;
    public Texture SandTexture;
    public Texture DirtTexture;
    public Texture GrassTexture;
    public Texture RockTexture;
    public bool AllowTerrainTransparency;

    public Material TerrainMaterial;
    public GeometricPrimitive TerrainGeometry;    
    public RgbPixelRepository HeightData;
  }
}