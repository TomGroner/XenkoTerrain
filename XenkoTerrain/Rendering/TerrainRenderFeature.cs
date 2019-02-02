using System;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Rendering;
using Xenko.Streaming;

namespace XenkoTerrain.Rendering
{
  public class TerrainRenderFeature : RootRenderFeature
  {
    public const float GRID_SIZE = 128f; // TODO: Remove the need for these. Move this to the terrain component
    public const float HEIGHT_SCALE = 16.0f;

    private DynamicEffectInstance effect;

    public override Type SupportedRenderObjectType => typeof(TerrainRenderObject);    

    public TerrainRenderFeature()
    {
      SortKey = 200;
    }

    protected override void InitializeCore()
    {           
      effect = new DynamicEffectInstance("TerrainRenderFeatureEffect");
      effect.Initialize(Context.Services);
    }

    public override void Prepare(RenderDrawContext context)
    {
      base.Prepare(context);

      foreach (var renderObject in RenderObjects)
      {
        if (renderObject is TerrainRenderObject terrainRenderObject)
        {
          terrainRenderObject.Prepare(context);
        }
      }
    }

    // make just add a modelcomponent to the entiy and all of this rendering is just a preview in the editor.....
    public override void Draw(RenderDrawContext context, RenderView renderView, RenderViewStage renderViewStage, int startIndex, int endIndex)
    {
      var graphicsDevice = context.GraphicsDevice;
      var graphicsContext = context.GraphicsContext;
      var commandList = context.GraphicsContext.CommandList;
      var camera = context.RenderContext.GetCurrentCamera();

      if (camera == null)
      {
        return;
      }
      
      effect.UpdateEffect(graphicsDevice);

      for (var index = startIndex; index < endIndex; index++)
      {
        var renderNodeReference = renderViewStage.SortedRenderNodes[index].RenderNode;
        var renderNode = GetRenderNode(renderNodeReference);
        var renderObject = (TerrainRenderObject)renderNode.RenderObject;
        var cameraPosition = camera.Entity.Transform.Position;

        if (!renderObject.IsInitialized)
        {
          continue;
        }


        var worldView = renderObject.World * renderView.View;
        var worldViewProjection = renderObject.World * renderView.ViewProjection;
        var projScreenRay = new Vector2(-1.0f / renderView.Projection.M11, 1.0f / renderView.Projection.M22);

        Matrix.Invert(ref renderView.View, out var inverseView);
        Matrix.Invert(ref renderView.Projection, out var projectionInverse);
        Matrix.Invert(ref renderObject.World, out var worldInverse);
        Matrix.Invert(ref worldView, out var worldViewInverse);
        Matrix.Transpose(ref worldInverse, out var worldInverseTranspose);

        effect.Parameters.Set(TransformationKeys.View, renderView.View);
        effect.Parameters.Set(TransformationKeys.Projection, renderView.Projection);
        effect.Parameters.Set(TransformationKeys.World, renderObject.World);
        effect.Parameters.Set(TransformationKeys.ViewProjection, renderView.ViewProjection);
        effect.Parameters.Set(TransformationKeys.WorldView, worldView);
        effect.Parameters.Set(TransformationKeys.WorldViewProjection, worldViewProjection);

        effect.Parameters.Set(TransformationKeys.ViewInverse, inverseView);
        effect.Parameters.Set(TransformationKeys.ProjScreenRay, projScreenRay);
        effect.Parameters.Set(TransformationKeys.WorldInverse, worldInverse);
        effect.Parameters.Set(TransformationKeys.WorldInverseTranspose, worldInverseTranspose);
        effect.Parameters.Set(TransformationKeys.WorldInverse, worldInverseTranspose);
        effect.Parameters.Set(TransformationKeys.WorldScale, Vector3.One);

        effect.Parameters.Set(GlobalKeys.Time, (float)context.RenderContext.Time.Total.TotalSeconds);
        effect.Parameters.Set(GlobalKeys.TimeStep, (float)context.RenderContext.Time.Elapsed.TotalSeconds);
        renderObject.Draw(context, effect);
      }
    }
  }
}