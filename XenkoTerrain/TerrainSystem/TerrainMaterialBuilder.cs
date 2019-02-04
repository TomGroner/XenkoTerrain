using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Rendering.Materials;
using Xenko.Rendering.Materials.ComputeColors;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainMaterialBuilder
  {
    public Material BuildTerrainMaterial(GraphicsDevice graphicsDevice, TerrainTileRenderObject renderObject)
    {
      // TODO: magic string on shader name
      return BuildMaterial("TerrainTileShader", graphicsDevice, renderObject.AllowTerrainTransparency);
    }

    protected Material BuildMaterial(string shaderName, GraphicsDevice graphicsDevice, bool includeTransparency)
    {
      return Material.New(graphicsDevice, new MaterialDescriptor
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