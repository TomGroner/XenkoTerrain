using Xenko.Core.Annotations;
using Xenko.Engine;
using Xenko.Games;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainHeightSourceEntityProcessor : EntityProcessor<TerrainHeightSourceEntityComponent, TerrainHeightSourceRenderObject>, IEntityComponentRenderProcessor
  {
    public VisibilityGroup VisibilityGroup { get; set; }

    protected override TerrainHeightSourceRenderObject GenerateComponentData([NotNull] Entity entity, [NotNull] TerrainHeightSourceEntityComponent component)
    {
      return new TerrainHeightSourceRenderObject()
      {
        HeightMap = component.HeightMap
      };
    }

    protected override bool IsAssociatedDataValid([NotNull] Entity entity, [NotNull] TerrainHeightSourceEntityComponent component, [NotNull] TerrainHeightSourceRenderObject associatedData)
    {
      return component.RenderGroup == associatedData.RenderGroup;
    }

    public override void Update(GameTime time)
    {
      foreach (var data in ComponentDatas)
      {
        var component = data.Key;
        var renderObject = data.Value;

        if (component.Enabled)
        {
          if (renderObject.HeightMap != component.HeightMap)
          {
            renderObject.HeightMap = component.HeightMap;
            renderObject.HeightData?.Dispose();
            renderObject.HeightData = null;
          }
          else if (renderObject.HeightData != null && component.HeightData != renderObject.HeightData)
          {
            component.HeightData = renderObject.HeightData;
          }
        }
      }

      base.Update(time);
    }

    public override void Draw(RenderContext context)
    {
      foreach (var data in ComponentDatas)
      {
        var component = data.Key;
        var renderObject = data.Value;

        if (component.Enabled)
        {
          VisibilityGroup.RenderObjects.Add(renderObject);
        }
        else
        {
          VisibilityGroup.RenderObjects.Remove(renderObject);
        }
      }
    }
  }
}