using System.ComponentModel;
using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Graphics;

namespace XenkoTerrain.Components
{
  [DataContract(nameof(TerrainEntityComponent))]
  [Display(nameof(TerrainEntityComponent), Expand = ExpandRule.Once)]
  [DefaultEntityComponentRenderer(typeof(TerrainEntityProcessor))]
  [ComponentOrder(100)]
  public class TerrainEntityComponent : ActivableEntityComponent
  {
    internal bool IsGeometryProcessed { get; set; }
    
    [DataMember(28)]
    public LightComponent Sun { get; set; }

    [DataMember(29)]
    public Texture BlendMap { get; set; }

    [DataMember(30)]
    public Texture HeightMap { get; set; }

    [DataMember(32)]
    public Texture SandTexture { get; set; }

    [DataMember(33)]
    public Texture DirtTexture { get; set; }

    [DataMember(34)]
    public Texture GrassTexture { get; set; }

    [DataMember(35)]
    public Texture RockTexture { get; set; }

    [DataMember(100)]
    [DefaultValue(RenderGroup.Group0)]
    public RenderGroup RenderGroup { get; set; }
  }
}