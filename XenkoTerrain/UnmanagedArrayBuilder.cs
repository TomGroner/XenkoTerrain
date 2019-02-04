using System;
using Xenko.Core;

namespace XenkoTerrain
{
  public static class UnmanagedArrayBuilder
  {
    public unsafe static UnmanagedArray<float> New(float[] data)
    {
      fixed (float* p = data)
      {
        return new UnmanagedArray<float>(data.Length, (IntPtr)p);
      }
    }
  }
}