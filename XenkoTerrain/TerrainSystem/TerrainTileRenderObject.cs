using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Graphics;
using Xenko.Graphics.GeometricPrimitives;
using Xenko.Rendering;

namespace XenkoTerrain.TerrainSystem
{
  public class TerrainTileRenderObject : RenderObject
  {
    public float Size;
    public float MaxHeight;
    public Texture HeightMap;
    public Matrix World;
    public GeometricPrimitive Geometry;
    public HeightDataSource HeightData;
    public Material Material;
    public Vector2 ScaleUv;

    public void Update(TerrainTileComponent component)
    {
      RenderGroup = component.RenderGroup;
      World = component.Entity.Transform.WorldMatrix;
      Enabled = component.Enabled;

      if (Material != component.Material)
      {
        Material = component.Material;
      }

      if (ScaleUv != component.ScaleUv)
      {
        ScaleUv = component.ScaleUv;
        Clear();
      }

      if (HeightMap != component.HeightMap)
      {
        HeightMap = component.HeightMap;
        Clear();
      }

      if (Size != component.Size)
      {
        Size = component.Size;
        Clear();
      }

      if (MaxHeight != component.MaxHeight)
      {
        MaxHeight = component.MaxHeight;
        Clear();
      }

      void Clear()
      {
        Geometry = null;
        component.IsGeometryProcessed = false;   
      }
    }

    public void Prepare(RenderDrawContext context)
    {
      if (Enabled && HeightDataNeedsRebuilt() && TryGetHeightMapImageData(context.CommandList, out var heightData))
      {
        HeightData = heightData;
        Geometry = new GeometryBuilder(heightData).BuildTerrainGeometricPrimitive(context.GraphicsDevice, Size, MaxHeight, ScaleUv);
      }
    }

    private bool HeightDataNeedsRebuilt()
    {
      return HeightMap != null && HeightData == null;
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