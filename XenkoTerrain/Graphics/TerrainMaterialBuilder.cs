using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Rendering.Materials;
using Xenko.Rendering.Materials.ComputeColors;
using XenkoTerrain.TerrainSystem;

namespace XenkoTerrain.Graphics
{
  public class TerrainMaterialBuilder
  {
    public TerrainMaterialBuilder(GraphicsDevice graphicsDevice)
    {
      GraphicsDevice = graphicsDevice;
    }

    public GraphicsDevice GraphicsDevice { get; }

    public Material BuildTerrainMaterial(TerrainTileRenderObject renderObject)
    {
      return BuildMaterial("TerrainTileShader", renderObject.AllowTerrainTransparency);
    }

    protected Material BuildMaterial(string shaderName, bool includeTransparency)
    {
      return Material.New(GraphicsDevice, new MaterialDescriptor
      {
        Attributes = new MaterialAttributes()
        {
          DiffuseModel = new MaterialDiffuseLambertModelFeature(),
          Diffuse = BuildDiffuseMap(shaderName),
          Transparency = includeTransparency ? GetTransparencyFeature() : null
        }
      });
    }

    protected MaterialDiffuseMapFeature BuildDiffuseMap(string shaderName)
    {
      return new MaterialDiffuseMapFeature
      {
        DiffuseMap = new ComputeShaderClassColor
        {
          MixinReference = shaderName
        }
      };
    }

    protected MaterialTransparencyBlendFeature GetTransparencyFeature()
    {
      return new MaterialTransparencyBlendFeature()
      {
        Alpha = new ComputeFloat() { Value = 1.0f },
        Tint = new ComputeColor() { Value = new Color4(1, 1, 1, 1) }
      };
    }
  }
}