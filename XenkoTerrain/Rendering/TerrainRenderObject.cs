using System;
using System.Collections.Generic;
using System.Linq;
using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Core.Serialization.Contents;
using Xenko.Engine;
using Xenko.Games;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Physics;
using Xenko.Rendering;
using XenkoTerrain.Components;
using XenkoTerrain.Graphics;

namespace XenkoTerrain.Rendering
{
  [ContentSerializer(typeof(DataContentSerializer<HeightfieldColliderShapeDesc>))]
  [DataContract("HeightfieldColliderShapeDesc")]
  [Display(50, "Heighfield")]
  public class HeightfieldColliderShapeDesc : IInlineColliderShapeDesc
  {
    public bool Match(object obj)
    {
      return true;
    }
  }


  public class TerrainRenderObject : RenderObject
  {
    public TerrainRenderObject(RenderGroup rendergroup)
    {
      RenderGroup = rendergroup;
    }

    // Component-linked fields
    public RgbPixelRepository pixels;
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
    public bool IsInitialized
    {
      get => TerrainGeometry != null;// || ShouldDraw;
    }

    private TerrainEntityComponent component;

    public TerrainRenderObject CreateOrCopy(TerrainEntityComponent component)
    {
      this.component = component;
      RenderGroup = component.RenderGroup;
      BlendMap = component.BlendMap;
      DirtTexture = component.DirtTexture;
      GrassTexture = component.GrassTexture;
      RockTexture = component.RockTexture;
      SandTexture = component.SandTexture;
      HeightMap = component.HeightMap;
      World = component.Entity.Transform.WorldMatrix;

      if (pixels != null)
      {
        //component.ProvideHeightMapData(pixels);
      }

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

    private List<TerrainEntityComponent> componentsToUpdate = new List<TerrainEntityComponent>();

    public void BuildGeometryForComponent(TerrainEntityComponent terrainEntityComponent)
    {
      if (!componentsToUpdate.Contains(terrainEntityComponent))
      {
        componentsToUpdate.Add(terrainEntityComponent);
      }
    }

    private bool stoppedForGameMode = false;

    public void Prepare(RenderDrawContext context)
    {
      if (TryGetHeightMapImageData(context))
      {
        var geometryBuilder = new TerrainGeometryBuilder(context.GraphicsDevice, TerrainRenderFeature.GRID_SIZE, pixels, TerrainRenderFeature.HEIGHT_SCALE);

        if (!IsInitialized)
        {
          TerrainGeometry = geometryBuilder.BuildTerrain();
          ApplyPipelineStateValues(TerrainGeometry);
        }

        foreach (var terrainEntityComponent in componentsToUpdate.Where(c => !c.IsGeometryProcessed))
        {
          if (terrainEntityComponent.Entity.Get<ModelComponent>() is ModelComponent existingTerrainModel)
          {
            terrainEntityComponent.Entity.Remove(existingTerrainModel);            
          }

          var geometryModel = geometryBuilder.BuildTerrainModel();
          var materialBuilder = new TerrainMaterialBuilder(context.GraphicsDevice);
          //var material = materialBuilder.BuildTerrainMaterial(false);
          //geometryModel.Materials.Add(0, material);                        
          terrainEntityComponent.Entity.Add(geometryModel);
       

          terrainEntityComponent.IsGeometryProcessed = true;
          stoppedForGameMode = true;
        }

        componentsToUpdate.Clear();
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
      if (stoppedForGameMode)
      {
        DrawComponent(context);
      }
      else
      {
        DrawSelf(context, terrainEffect);
      }      
    }

    private void DrawSelf(RenderDrawContext context, EffectInstance terrainEffect)
    {
      DoTheDraw(context, terrainEffect.Parameters);
      TerrainGeometry.Draw(context.GraphicsContext, terrainEffect);
    }

    private void DrawComponent(RenderDrawContext context)
    {
      if (component.Entity.Get<ModelComponent>().Materials.Count > 0)
      {
        DoTheDraw(context, component.Entity.Get<ModelComponent>().Materials[0].Passes[0].Parameters);
      }      
    }


    private void DoTheDraw(RenderDrawContext context, ParameterCollection parameters)
    {
      // Diffuse Lighting
      parameters.Set(TerrainShaderKeys.LightPosition, LightPosition);
      parameters.Set(TerrainShaderKeys.LightColor, LightColor.ToVector3());
      parameters.Set(TerrainShaderKeys.LightIntensity, LightIntensity);

      //parametersSet(TerrainShaderKeys.SpecularReflectivity, 1);
      //parametersSet(TerrainShaderKeys.SpecularDamping, 10);

      // Terrain Textures
      parameters.Set(TerrainShaderKeys.BlendMap, BlendMap);
      parameters.Set(TerrainShaderKeys.SandTexture, SandTexture);
      parameters.Set(TerrainShaderKeys.DirtTexture, DirtTexture);
      parameters.Set(TerrainShaderKeys.GrassTexture, GrassTexture);
      parameters.Set(TerrainShaderKeys.RockTexture, RockTexture);
      parameters.Set(TerrainShaderKeys.TextureScale, TerrainRenderFeature.GRID_SIZE); // put on component
      parameters.Set(TerrainShaderKeys.HeightScale, TerrainRenderFeature.HEIGHT_SCALE);
    }

    protected bool TryGetHeightMapImageData(RenderDrawContext context)
    {
      if (pixels != null)
      {
        return true;
      }
      else if (HeightMap.Width == 0)
      {
        return false;
      }

      try
      {
        var heightMapImage = HeightMap.GetDataAsImage(context.CommandList);
        pixels = new RgbPixelRepository(heightMapImage.PixelBuffer[0]);
        return true;
      }
      catch (Exception)
      {        
      }


      return false;

      
    }
  }
}