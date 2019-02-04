using System.ComponentModel;
using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  [DataContract(nameof(TerrainHeightSourceEntityComponent))]
  [Display("Terrain Height Source", Expand = ExpandRule.Once)]
  [DefaultEntityComponentRenderer(typeof(TerrainHeightSourceEntityProcessor))]
  [ComponentOrder(101)]
  public class TerrainHeightSourceEntityComponent : ActivableEntityComponent
  {
    [DataMember(1)]
    [DefaultValue(RenderGroup.Group0)]
    public RenderGroup RenderGroup { get; set; }

    [DataMember(2)]
    public Texture HeightMap { get; set; }

    [DataMemberIgnore]
    public TerrainHeightDataRepository HeightData { get; set; }
    // TODO: Have this component have the geometry builder and collider builder and make them once, instead of
    // having other pieces use this component's height data to keep re-making them
  }
}