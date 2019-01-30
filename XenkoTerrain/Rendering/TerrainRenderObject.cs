using System;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Games;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;
using XenkoTerrain.Components;
using XenkoTerrain.Graphics;

namespace XenkoTerrain.Rendering
{
  public class TerrainRenderObject : RenderObject
  {
    public TerrainRenderObject(RenderGroup rendergroup)
    {
      RenderGroup = rendergroup;
    }

    // Component-linked fields
    public Texture BlendMap;
    public Texture HeightMap;
    public Texture SandTexture;
    public Texture DirtTexture;
    public Texture GrassTexture;
    public Texture RockTexture;

    public Vector3 LightPosition;
    public Color3 LightColor;
    public float LightIntensity;

    // Rendering properties
    public Matrix World;    
    public GeometricPrimitive TerrainGeometry;
    public bool IsInitialized => TerrainGeometry != null;

    public TerrainRenderObject Copy(TerrainEntityComponent component)
    {
      RenderGroup = component.RenderGroup;
      BlendMap = component.BlendMap;
      DirtTexture = component.DirtTexture;
      GrassTexture = component.GrassTexture;
      RockTexture = component.RockTexture;
      SandTexture = component.SandTexture;
      HeightMap = component.HeightMap;
      World = component.Entity.Transform.WorldMatrix;

      

      PopulateLighting(component);
      // TODO: Also support an ambient light. These two together should help get pretty damn close
      return this;
    }

    // Need to remember this is being used by the editor. Stuff can be empty by default all over the
    // place. Need to do safe guard checks a lot more, and need to go against the normal convention of
    // blowing up when things are missing, and just let the thing not render if it cannot. Or, even
    // better once the above is done, make it render a red box or something
    private void PopulateLighting(TerrainEntityComponent component)
    {
      if (component.Sun is LightComponent sunComponent)
      {
        LightPosition = sunComponent.Entity?.Transform?.Position ?? Vector3.Zero;
        LightColor = sunComponent.GetColor();
        LightIntensity = sunComponent.Intensity;
      }
    }

    public void Prepare(RenderDrawContext context)
    {
      if (!IsInitialized)
      {
        if (TryGetHeightMapImageData(context, out var heightMapImage))
        {
          var geometryBuilder = new TerrainGeometryBuilder(context.GraphicsDevice, TerrainRenderFeature.GRID_SIZE, heightMapImage, TerrainRenderFeature.HEIGHT_SCALE);
          TerrainGeometry = geometryBuilder.BuildTerrain();
          ApplyPipelineStateValues(TerrainGeometry);
        }
      }
    }

    private void ApplyPipelineStateValues(GeometricPrimitive primitive)
    {
      primitive.PipelineState.State.SetDefaults();
      primitive.PipelineState.State.BlendState = BlendStates.AlphaBlend;
      primitive.PipelineState.State.RasterizerState.CullMode = CullMode.Back;
      //primitive.PipelineState.State.DepthStencilState = DepthStencilStates.None;
    }

    public void Update(GameTime time)
    {
      // do we need to update the light position and such? and 
    }

    public void Draw(RenderDrawContext context, EffectInstance terrainEffect)
    {
      // Diffuse Lighting
      terrainEffect.Parameters.Set(TerrainShaderKeys.LightPosition, LightPosition);
      terrainEffect.Parameters.Set(TerrainShaderKeys.LightColor, LightColor.ToVector3());
      terrainEffect.Parameters.Set(TerrainShaderKeys.LightIntensity, LightIntensity);

      terrainEffect.Parameters.Set(TerrainShaderKeys.SpecularReflectivity, 1);
      terrainEffect.Parameters.Set(TerrainShaderKeys.SpecularDamping, 10);

      // Terrain Textures
      terrainEffect.Parameters.Set(TerrainShaderKeys.BlendMap, BlendMap);
      terrainEffect.Parameters.Set(TerrainShaderKeys.SandTexture, SandTexture);
      terrainEffect.Parameters.Set(TerrainShaderKeys.DirtTexture, DirtTexture);
      terrainEffect.Parameters.Set(TerrainShaderKeys.GrassTexture, GrassTexture);
      terrainEffect.Parameters.Set(TerrainShaderKeys.RockTexture, RockTexture);

      terrainEffect.Parameters.Set(TerrainShaderKeys.TextureScale, TerrainRenderFeature.GRID_SIZE); // put on component
      terrainEffect.Parameters.Set(TerrainShaderKeys.HeightScale, TerrainRenderFeature.HEIGHT_SCALE);

      TerrainGeometry.Draw(context.GraphicsContext, terrainEffect);
    }

    protected bool TryGetHeightMapImageData(RenderDrawContext context, out Image heightMapImage)
    {
      try
      {
        heightMapImage = HeightMap.GetDataAsImage(context.CommandList);
      }
      catch (Exception)
      {
        heightMapImage = null;
      }

      return heightMapImage != null;
    }
  }
}