using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Core.Mathematics;

namespace XenkoTerrain.TerrainSystem
{
  [ComponentOrder(128)]
  [Display("Terrain Tile", Expand = ExpandRule.Always)]
  [DataContract(nameof(TerrainTileComponent))]
  [DefaultEntityComponentRenderer(typeof(TerrainTileProcessor))]
  public class TerrainTileComponent : ActivableEntityComponent
  {
    internal bool IsGeometryProcessed;

    public RenderGroup RenderGroup { get; set; }

    public Texture HeightMap { get; set; }

    public float Size { get; set; }

    public float MaxHeight { get; set; }

    public Vector2 ScaleUv { get; set; }

    public Material Material { get; set; }
  }
}