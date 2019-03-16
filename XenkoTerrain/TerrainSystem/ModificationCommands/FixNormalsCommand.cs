using Xenko.Core.Mathematics;

namespace XenkoTerrain.TerrainSystem
{
  public class FixNormalsCommand : ModificationCommand
  {
    public FixNormalsCommand()
    {
      Name = "Correct Normals";
      Description = "Automatic";
      IsOnScreen = true;
    }

    public override bool CanExecute(ModificationCommandContext context)
    {
      return context.DataModified;
    }

    public override void Execute(ModificationCommandContext context)
    {
      int x = 0;
      int y = 0;
      var data = context.Data;
      var wrapAtX = context.Data.HeightSource.Columns + 1;
      var stopAtY = context.Data.HeightSource.Rows + 1;
      
      for (int i = 0; i < data.Vertices.Length; ++i)
      {
        var heightL = x > 0 && i - 1 > 0 ? data.Vertices[i - 1].Position.Y : 0.0f;
        var heightR = x < wrapAtX && i + 1 < data.Vertices.Length ? data.Vertices[i + 1].Position.Y : 0.0f;
        var heightU = y > 0 && i + wrapAtX < data.Vertices.Length ? data.Vertices[i + wrapAtX].Position.Y : 0.0f;
        var heightD = y < stopAtY && i - wrapAtX > 0 ? data.Vertices[i - wrapAtX].Position.Y : 0.0f;
        var newNormal = new Vector3(heightL - heightR, 2.0f, heightD - heightU);
        newNormal.Normalize();

        ref var normal = ref data.Vertices[i].Normal;
        normal.X = newNormal.X;
        normal.Y = newNormal.Y;
        normal.Z = newNormal.Z;
        x++;

        if (x == wrapAtX)
        {
          x = 0;
          y++;
        }
      }
    }
  }
}