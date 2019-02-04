using System.ComponentModel;
using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Graphics;

namespace XenkoTerrain.TerrainSystem
{
  [DataContract(nameof(TerrainTileEntityComponent))]
  [Display("Terrain Tile", Expand = ExpandRule.Once)]
  [DefaultEntityComponentRenderer(typeof(TerrainTileEntityProcessor))]
  [ComponentOrder(100)]
  public class TerrainTileEntityComponent : ActivableEntityComponent
  {
    [DataMemberIgnore]
    public bool IsGeometryProcessed { get; set; }

    [DataMemberIgnore]
    public bool IsColliderProcessed { get; set; }

    [DataMember(10)]
    [Display("Height Source")]
    public TerrainHeightSourceEntityComponent HeightSource { get; set; }    

    [DataMember(11)]
    public float Size { get; set; }

    [DataMember(12)]
    public float MaxHeight { get; set; }

    [DataMember(13)]
    public bool AllowTerrainTransparency { get; set; }

    [DataMember(29)]
    public Texture BlendMap { get; set; }

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