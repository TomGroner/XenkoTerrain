using Xenko.Core.Annotations;
using Xenko.Engine;
using Xenko.Games;
using Xenko.Rendering;
using XenkoTerrain.Rendering;

namespace XenkoTerrain.Components
{
  public class TerrainEntityProcessor : EntityProcessor<TerrainEntityComponent, TerrainRenderObject>, IEntityComponentRenderProcessor
  {
    public VisibilityGroup VisibilityGroup { get; set; }

    protected override TerrainRenderObject GenerateComponentData([NotNull] Entity entity, [NotNull] TerrainEntityComponent component)
    {
      return new TerrainRenderObject(component.RenderGroup).Copy(component);
    }

    protected override bool IsAssociatedDataValid(Entity entity, TerrainEntityComponent component, TerrainRenderObject associatedData)
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
          renderObject.Update(time);
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
          renderObject.Copy(component);
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