using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Graphics;
using Xenko.Rendering;
using Xenko.Core.Mathematics;
using System.Collections.Generic;
using Xenko.Physics.Shapes;
using Xenko.Physics;

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

            //Here we will generate the collider for the first time
            RegenCollider();
        }

        //Thanks to EternalTamago
        private HeightfieldColliderShape CreateHeightfield(int width, int length, float min, float max, float[] values, out UnmanagedArray<float> points)
        {
            points = new UnmanagedArray<float>(width * length);
            points.Write(values);
            return new HeightfieldColliderShape(width, length, points, 1f, min, max, false);
        }

        public void RegenCollider(GeometryData data = null)
        {
            //If no new data is provided, use the existing data
            if (data == null)
                data = CurrentGeometryData; //Use the existing data
            else
                CurrentGeometryData = data; //Save the new data, there's been an update

            //Whether or not to add the collider to the entity
            var add = false;

            //Get or create a collider
            StaticColliderComponent colliderComponent = Entity.Get<StaticColliderComponent>();
            if (colliderComponent == null)
            {
                colliderComponent = new StaticColliderComponent();
                add = true;
            }

            //Create collider
            var heightfield = CreateHeightfield((int)data.TessellationX, (int)data.TessellationY, -MaxHeight, MaxHeight, data.Vertices.Select(v => v.Position.Y).ToArray(), out var points);

            //Clear any existing colliders
            colliderComponent.ColliderShapes.Clear();

            //Add the collider
            colliderComponent.ColliderShapes.Add(new BoxColliderShapeDesc());
            colliderComponent.ColliderShape = heightfield;

            //Add to entity if it does not already exist
            if (add)
                Entity.Add(colliderComponent);
        }
    }
}