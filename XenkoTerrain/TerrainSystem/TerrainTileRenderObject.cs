using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;

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
    public TerrainHeightDataRepository HeightData;

    public void Draw(ParameterCollection parameters) // TODO: wireframe and other modes
    {
      parameters.Set(TerrainTileShaderKeys.BlendMap, BlendMap);
      parameters.Set(TerrainTileShaderKeys.SandTexture, SandTexture);
      parameters.Set(TerrainTileShaderKeys.DirtTexture, DirtTexture);
      parameters.Set(TerrainTileShaderKeys.GrassTexture, GrassTexture);
      parameters.Set(TerrainTileShaderKeys.RockTexture, RockTexture);
      parameters.Set(TerrainTileShaderKeys.TextureScale, Size);
    }

    public void Update(TerrainTileEntityComponent component)
    {
      if (Enabled != component.Enabled) { Enabled = component.Enabled; }
      if (AllowTerrainTransparency != component.AllowTerrainTransparency) { AllowTerrainTransparency = component.AllowTerrainTransparency; }
      if (BlendMap != component.BlendMap) { BlendMap = component.BlendMap; }
      if (SandTexture != component.SandTexture) { SandTexture = component.SandTexture; }
      if (DirtTexture != component.DirtTexture) { DirtTexture = component.DirtTexture; }
      if (GrassTexture != component.GrassTexture) { GrassTexture = component.GrassTexture; }
      if (RockTexture != component.RockTexture) { RockTexture = component.RockTexture; }
      if (RenderGroup != component.RenderGroup) { RenderGroup = component.RenderGroup; }

      if (Size != component.Size)
      {
        component.IsGeometryProcessed = false;
        TerrainGeometry = null;
        Size = component.Size;
      }

      if (MaxHeight != component.MaxHeight)
      {
        component.IsGeometryProcessed = false;
        TerrainGeometry = null;
        MaxHeight = component.MaxHeight;
      }

      if (component?.HeightSource?.HeightData is TerrainHeightDataRepository heightData && HeightData != heightData)
      {
        component.IsGeometryProcessed = false;
        TerrainGeometry = null;
        HeightData = heightData;
      }
    }
  }
}