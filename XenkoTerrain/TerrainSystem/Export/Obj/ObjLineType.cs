using System.Collections.Generic;

namespace XenkoTerrain.TerrainSystem.Export.Obj
{
  public enum ObjLineType
  {
    None,
    Position,
    Normal,
    TextureCoordinate,
    Face,
    MaterialNew,
    MaterialUse,
    MaterialAmbient,
    MaterialAmbientMap,
    MaterialDiffuse,
    MaterialDiffuseMap,
    MaterialSpecular,
    MaterialSpecularMap,
    Group,
    Object,
    SmoothShading
  }

  public static class ObjLineTypeExtensions
  {
    private static Dictionary<ObjLineType, string> syntaxReference;

    static ObjLineTypeExtensions()
    {
      syntaxReference = new Dictionary<ObjLineType, string>()
      {
        [ObjLineType.None] = "",
        [ObjLineType.Position] = "v ",
        [ObjLineType.Normal] = "vn ",
        [ObjLineType.TextureCoordinate] = "vt ",
        [ObjLineType.Face] = "f ",
        [ObjLineType.MaterialNew] = "newmtl ",
        [ObjLineType.MaterialUse] = "usemtl ",
        [ObjLineType.MaterialAmbient] = "Ka ",
        [ObjLineType.MaterialAmbientMap] = "map_Ka ",
        [ObjLineType.MaterialDiffuse] = "Kd ",
        [ObjLineType.MaterialDiffuseMap] = "map_Kd ",
        [ObjLineType.MaterialSpecular] = "Ks ",
        [ObjLineType.MaterialSpecularMap] = "map_Ks ",
        [ObjLineType.Group] = "g ",
        [ObjLineType.Object] = "o "
      };
    }

    public static string Syntax(this ObjLineType self)
    {
      return syntaxReference[self];
    }
  }
}