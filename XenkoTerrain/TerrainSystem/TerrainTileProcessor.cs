using System.Collections.Generic;
using Xenko.Core.Annotations;
using Xenko.Engine;
using Xenko.Extensions;
using Xenko.Games;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileProcessor : EntityProcessor<TerrainTileComponent, TerrainTileRenderObject>, IEntityComponentRenderProcessor
  {
    public VisibilityGroup VisibilityGroup { get; set; }

    public override void Update(GameTime time)
    {
      foreach (var data in ComponentDatas)
      {
        UpdatePair(time, data.Key, data.Value);
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

    protected override bool IsAssociatedDataValid([NotNull] Entity entity, [NotNull] TerrainTileComponent component, [NotNull] TerrainTileRenderObject associatedData)
    {
      return component.RenderGroup == associatedData.RenderGroup;
    }

    protected override TerrainTileRenderObject GenerateComponentData([NotNull] Entity entity, [NotNull] TerrainTileComponent component)
    {
      return new TerrainTileRenderObject(component)
      {
        Size = component.Size,
        Enabled = component.Enabled,
        RenderGroup = component.RenderGroup
      };
    }

    private void UpdatePair(GameTime time, TerrainTileComponent component, TerrainTileRenderObject renderObject)
    {
      renderObject.Update(component);

      if (component.Enabled)
      {
        if (renderObject.Mesh != null && !component.IsSet)
        {
          component.Build(renderObject);
        }
        else if (component.IsHidden && component.IsSet)
        {
          component.Show();
        }
      }
      else
      {
        component.Hide();
      }
    }
  }
}