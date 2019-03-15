using System;
using System.Collections.Generic;
using Xenko.Core.Mathematics;
using Xenko.Profiling;

namespace XenkoTerrain.TerrainSystem
{
  public partial class TerrainTileModifierComponent
  {
    public interface IModificationCommand
    {
      string Name { get; set; }

      string GetOnScreenDescription();

      bool CanExecute(object arg);

      void Execute(object arg);
    }

    public class ModificationCommands
    {
      private Int2[] positions;

      public ModificationCommands(DebugTextSystem debugText)
      {
        DebugText = debugText;
        positions = new Int2[5];
        positions[0].Y = 20;
        positions[1].Y = 40;
        positions[2].Y = 60;
        positions[3].Y = 80;
        positions[4].Y = 100;
      }

      public Int2 TopLeft { get; set; }

      public DebugTextSystem DebugText { get; set; }

      public IEnumerable<IModificationCommand> Commands { get; set; }

      public void Write()
      {
        DebugText.Print("Save: CTRL+S", positions[0]);
        DebugText.Print("Flatten: Left CTRL", positions[1]);
        DebugText.Print("Smoothen: Left SHIFT", positions[2]);
        DebugText.Print("Lower: Middle Mouse", positions[3]);

        /*
          if (Input.IsKeyDown(Keys.LeftCtrl) && Input.IsKeyDown(Keys.S))
          {
            mode = ModificationCommand.SaveToFile;
          }
          else if (Input.IsMouseButtonDown(MouseButton.Left) && Input.IsKeyDown(Keys.LeftCtrl))
          {
            mode = ModificationCommand.Flatten;
          }
          else if (Input.IsMouseButtonDown(MouseButton.Left) && Input.IsKeyDown(Keys.LeftShift))
          {
            mode = ModificationCommand.Smoothen;
          }
          else if (Input.IsMouseButtonDown(MouseButton.Left))
          {
            mode = ModificationCommand.Raise;
          }
          else if (Input.IsMouseButtonDown(MouseButton.Middle))
          {
            mode = ModificationCommand.Lower;
          }
        */
      }
    }
  }
}