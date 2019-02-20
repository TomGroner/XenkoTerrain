using System;
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileRenderFeature : RootRenderFeature
  {
    private MutablePipelineState pipelineState;
    private DynamicEffectInstance effect;
    public override Type SupportedRenderObjectType => typeof(TerrainTileRenderObject);

    public TerrainTileRenderFeature()
    {
    }

    protected override void InitializeCore()
    {
      effect = new DynamicEffectInstance("TerrainTileRenderFeatureEffect");
      effect.Initialize(Context.Services);
    }

    public override void Prepare(RenderDrawContext context)
    {
      base.Prepare(context);

      foreach (var renderObject in RenderObjects)
      {
        if (renderObject is TerrainTileRenderObject terrainTile)
        {
          if (terrainTile.Enabled && terrainTile.TerrainGeometry == null && terrainTile.HeightData != null)
          {
            var geometryBuilder = new TerrainGeometryBuilder(terrainTile.HeightData); // TODO: save to component
            var materialBuilder = new TerrainMaterialBuilder(); // TODO: save to component
            terrainTile.TerrainGeometry = geometryBuilder.BuildTerrainGeometricPrimitive(context.GraphicsDevice, terrainTile.Size, terrainTile.MaxHeight);
            terrainTile.TerrainMaterial = materialBuilder.BuildTerrainMaterial(context.GraphicsDevice, terrainTile);
          }
        }
      }
    }

    public override void Draw(RenderDrawContext context, RenderView renderView, RenderViewStage renderViewStage, int startIndex, int endIndex)
    {
      //var graphicsDevice = context.GraphicsDevice;
      //var graphicsContext = context.GraphicsContext;
      //var commandList = context.GraphicsContext.CommandList;
      //var camera = context.RenderContext.GetCurrentCamera();

      //if (camera == null)
      //{
      //  return;
      //}

      //effect.UpdateEffect(graphicsDevice);

      //for (var index = startIndex; index < endIndex; index++)
      //{
      //  var renderNodeReference = renderViewStage.SortedRenderNodes[index].RenderNode;
      //  var renderNode = GetRenderNode(renderNodeReference);
      //  var renderObject = (TerrainTileRenderObject)renderNode.RenderObject;
      //  var cameraPosition = camera.Entity.Transform.Position;

      //  if (renderObject.TerrainGeometry == null)
      //  {
      //    continue;
      //  }


      //  var worldView = renderObject.World * renderView.View;
      //  var worldViewProjection = renderObject.World * renderView.ViewProjection;
      //  var projScreenRay = new Vector2(-1.0f / renderView.Projection.M11, 1.0f / renderView.Projection.M22);

      //  Matrix.Invert(ref renderView.View, out var inverseView);
      //  Matrix.Invert(ref renderView.Projection, out var projectionInverse);
      //  Matrix.Invert(ref renderObject.World, out var worldInverse);
      //  Matrix.Invert(ref worldView, out var worldViewInverse);
      //  Matrix.Transpose(ref worldInverse, out var worldInverseTranspose);

      //  effect.Parameters.Set(TransformationKeys.View, renderView.View);
      //  effect.Parameters.Set(TransformationKeys.Projection, renderView.Projection);
      //  effect.Parameters.Set(TransformationKeys.World, renderObject.World);
      //  effect.Parameters.Set(TransformationKeys.ViewProjection, renderView.ViewProjection);
      //  effect.Parameters.Set(TransformationKeys.WorldView, worldView);
      //  effect.Parameters.Set(TransformationKeys.WorldViewProjection, worldViewProjection);

      //  effect.Parameters.Set(TransformationKeys.ViewInverse, inverseView);
      //  effect.Parameters.Set(TransformationKeys.ProjScreenRay, projScreenRay);
      //  effect.Parameters.Set(TransformationKeys.WorldInverse, worldInverse);
      //  effect.Parameters.Set(TransformationKeys.WorldInverseTranspose, worldInverseTranspose);
      //  effect.Parameters.Set(TransformationKeys.WorldInverse, worldInverseTranspose);
      //  effect.Parameters.Set(TransformationKeys.WorldScale, Vector3.One);

      //  effect.Parameters.Set(GlobalKeys.Time, (float)context.RenderContext.Time.Total.TotalSeconds);
      //  effect.Parameters.Set(GlobalKeys.TimeStep, (float)context.RenderContext.Time.Elapsed.TotalSeconds);

      //  // Diffuse Lighting
      //  //effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.LightPosition, renderObject.LightPosition);
      //  //effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.LightColor, renderObject.LightColor.ToVector3());
      //  //effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.LightIntensity, renderObject.LightIntensity);

      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.SpecularReflectivity, 1);
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.SpecularDamping, 10);

      //  // Terrain Textures
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.BlendMap, renderObject.BlendMap);
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.SandTexture, renderObject.SandTexture);
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.DirtTexture, renderObject.DirtTexture);
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.GrassTexture, renderObject.GrassTexture);
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.RockTexture, renderObject.RockTexture);

      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.TextureScale, renderObject.Size); // put on component
      //  effect.Parameters.Set(TerrainTileRenderFeatureEffectKeys.HeightScale, renderObject.MaxHeight);

      //  renderObject.TerrainGeometry.PipelineState.State.RasterizerState.FillMode = FillMode.Wireframe;
      //  renderObject.TerrainGeometry.Draw(context.GraphicsContext, effect);        
      //}
    }
  }
}