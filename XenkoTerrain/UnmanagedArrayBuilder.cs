using Xenko.Core;

namespace XenkoTerrain
{
  public static class UnmanagedArrayBuilder
  {
    public unsafe static UnmanagedArray<float> New(float[] arrayData)
    {
      fixed (float* p = arrayData)
      {
        var unmanagedData = new UnmanagedArray<float>(arrayData.Length);
        unmanagedData.Write(arrayData);
        return unmanagedData;
      }
    }
  }
}