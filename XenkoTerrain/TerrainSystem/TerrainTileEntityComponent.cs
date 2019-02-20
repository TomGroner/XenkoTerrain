using System.ComponentModel;
using Xenko.Core;
using Xenko.Core.Annotations;
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
    private const int RenderingCategory = 100;
    private const int TerrainCategory   = 200;
    private const int TexturesCategory  = 300;

    internal bool IsGeometryProcessed;
    internal bool IsColliderProcessed;

    // *** RENDER PROPERTIES
    [Display(category: "Rendering Properties")]
    [DataMember(RenderingCategory + 1)]
    [DataMemberRange(0.0, 20, 1.0, 10.0, 0)]
    [DefaultValue(1.0f)]
    public float AdditionalTessellation { get; set; } 

    [Display(category: "Rendering Properties")]
    [DataMember(RenderingCategory + 3)]
    [DefaultValue(RenderGroup.Group0)]
    public RenderGroup RenderGroup { get; set; }

    // *** TERRAIN PROPERTIES
    [Display(category: "Terrain Properties")]
    [DataMember(TerrainCategory + 1)]
    public TerrainHeightSourceEntityComponent HeightSource { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(TerrainCategory + 2)]
    public float Size { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(TerrainCategory + 3)]
    [DataMemberRange(1.0, 100, 1.0, 10.0, 0)]
    [DefaultValue(3.0f)]
    public float MaxHeight { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(TerrainCategory + 4)]
    [DefaultValue(false)]
    public bool AllowTerrainTransparency { get; set; }

    // ** TEXTURE PROPERTIES
    [Display(category: "Terrain Textures")]
    [DataMember(TexturesCategory + 1)]
    public Texture BlendMap { get; set; }

    [Display(category: "Terrain Textures")]
    [DataMember(TexturesCategory + 2)]
    public Texture SandTexture { get; set; }

    [Display(category: "Terrain Textures")]
    [DataMember(TexturesCategory + 3)]
    public Texture DirtTexture { get; set; }

    [Display(category: "Terrain Textures")]
    [DataMember(TexturesCategory + 4)]
    public Texture GrassTexture { get; set; }

    [Display(category: "Terrain Textures")]
    [DataMember(TexturesCategory + 5)]
    public Texture RockTexture { get; set; }

  }// Make one of these for textures? make a nice UI?
   // public IndexingDictionary<Material> Materials { get; } = new IndexingDictionary<Material>();
}