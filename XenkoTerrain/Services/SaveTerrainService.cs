using System.Threading.Tasks;
using System.Windows.Forms;
using XenkoTerrain.TerrainSystem;
using XenkoTerrain.TerrainSystem.Export.Obj;

namespace XenkoTerrain.Services
{
  public class SaveTerrainService
  {
    public async Task SaveAsync(GeometryData data)
    {
      var saveDialog = new SaveFileDialog()
      {
        Title = "Save Terrain to File",
        Filter = "OBJ Files|*.obj"
      };

      if (saveDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
      {        
        await new GeometryExporerObj().SaveAsync(saveDialog.FileName, data);
      }
    }
  }
}