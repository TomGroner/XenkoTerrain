using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Core.Mathematics;
using System.Collections.Generic;

namespace XenkoTerrain.TerrainSystem
{
  [ComponentOrder(128)]
  [Display("Terrain Tile", Expand = ExpandRule.Always)]
  [DataContract(nameof(TerrainTileComponent))]
  [DefaultEntityComponentRenderer(typeof(TerrainTileProcessor))]    
  public class TerrainTileComponent : StartupScript
  {
    private const int TerrainProperties = 200;
    private const int RenderingProperties = 100;

    private ModelComponent hiddenGeometry;

    public virtual bool Enabled { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(100)]
    public float Size { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(101)]
    public float MaxHeight { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(102)]
    public Texture HeightMap { get; set; }

    [Display(category: "Terrain Properties")]
    [DataMember(103)]
    public Material Material { get; set; }

    [Display(category: "Rendering Properties")]
    [DataMember(100)]
    public RenderGroup RenderGroup { get; set; }

    [Display(category: "Rendering Properties")]
    [DataMember(101)]
    public Vector2 UvScale { get; set; } = Vector2.One;

    [DataMemberIgnore]
    public bool IsSet { get; private set; } = false;

    [DataMemberIgnore]
    public bool IsHidden { get; private set; } = false;

    [DataMemberIgnore]
    public MeshDraw CurrentMeshDraw { get; private set; }

    [DataMemberIgnore]
    public GeometryData CurrentGeometryData { get; private set; }

    public void Hide()
    {
      if (!IsHidden && IsSet)
      {
        IsHidden = true;
        Entity.TryRemove(out hiddenGeometry);
      }
    }

    public void Show()
    {
      IsHidden = false;

      if (hiddenGeometry is ModelComponent geometry)
      {
        Entity.Add(geometry);
        hiddenGeometry = null;
      }
    }

    public void Clear()
    {
      IsSet = false;
      hiddenGeometry = null;
      Entity.TryRemove<ModelComponent>();
    }

    public void Build(TerrainTileRenderObject renderObject)
    {
      IsSet = true;
      hiddenGeometry = null;
      CurrentMeshDraw = renderObject.MeshDraw;
      CurrentGeometryData = renderObject.Data;

      var model = new Model
      {
        Meshes = new List<Mesh> { renderObject.Mesh },
        BoundingBox = renderObject.MeshBoundingBox,
        BoundingSphere = renderObject.MeshBoundingSphere
      };

      var modelComponent = new ModelComponent(model);
      modelComponent.Materials.Add(0, renderObject.Material);

      Entity.Add(modelComponent);
    }
  }
}