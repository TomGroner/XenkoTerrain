using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileRenderObject : RenderObject
  {
    public TerrainTileRenderObject(TerrainTileComponent component)
    {
      Update(component);
    }

    public float Size;
    public float MaxHeight;
    public Mesh Mesh;
    public GeometryData Data;
    public Texture HeightMap;
    public Vector2 UvScale;
    public BoundingBox MeshBoundingBox;
    public BoundingSphere MeshBoundingSphere;
    public MeshDraw MeshDraw;
    public Material Material;

    public void Update(TerrainTileComponent component)
    {
      var shouldClear = Size != component.Size ||
                        MaxHeight != component.MaxHeight ||
                        HeightMap != component.HeightMap ||
                        UvScale != component.UvScale;

      Enabled = component.Enabled;
      Size = component.Size;
      HeightMap = component.HeightMap;
      MaxHeight = component.MaxHeight;
      UvScale = component.UvScale;
      Material = component.Material;

      if (shouldClear)
      {
        Mesh = null;
        Data = null;
        component.Clear();
      }
    }

    public void Build(RenderDrawContext context)
    {
      if (Mesh == null && TryGetHeightMapImageData(context.CommandList, out var data))
      {
        Data = new GeometryBuilder(data).BuildTerrainData(Size, MaxHeight, UvScale);
        MeshBoundingBox = Utils.FromPoints(Data.Vertices);
        MeshBoundingSphere = BoundingSphere.FromBox(MeshBoundingBox);
        BoundingBox = new BoundingBoxExt(MeshBoundingBox);

        var vertexBuffer = Buffer.Vertex.New(context.GraphicsDevice, Data.Vertices, GraphicsResourceUsage.Dynamic);
        var indexBuffer = Buffer.Index.New(context.GraphicsDevice, Data.Indices);
        var vertexBufferBinding = new VertexBufferBinding(vertexBuffer, VertexPositionNormalTexture.Layout, vertexBuffer.ElementCount);
        var indexBufferBinding = new IndexBufferBinding(indexBuffer, true, indexBuffer.ElementCount);

        MeshDraw = new MeshDraw
        {
          StartLocation = 0,
          PrimitiveType = PrimitiveType.TriangleList,
          VertexBuffers = new[] { vertexBufferBinding },
          IndexBuffer = indexBufferBinding,
          DrawCount = indexBuffer.ElementCount
        };

        Mesh = new Mesh(MeshDraw, new ParameterCollection());
      }
    }

    protected bool TryGetHeightMapImageData(CommandList commandList, out HeightDataSource data)
    {
      if (HeightMap?.Width > 0)
      {
        data = new HeightDataSource(HeightMap.GetDataAsImage(commandList).PixelBuffer[0]);
        return true;
      }
      else
      {
        data = default;
        return false;
      }
    }
  }
}