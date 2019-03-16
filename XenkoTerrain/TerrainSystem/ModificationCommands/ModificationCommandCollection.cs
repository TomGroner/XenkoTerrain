using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xenko.Core.Mathematics;
using Xenko.Profiling;

namespace XenkoTerrain.TerrainSystem
{
  public class ModificationCommandCollection: IEnumerable<IModificationCommand>
  {
    private List<IModificationCommand> commands = new List<IModificationCommand>();

    public DebugTextSystem DebugText { get; set; }

    private Int2 TopLeft { get; set; } = new Int2(10, 40);

    private Int2 DrawCommandDelta { get; set; } = new Int2(0, 24);

    public void Add(IModificationCommand command)
    {
      commands?.Add(command);
    }

    public IEnumerator<IModificationCommand> GetEnumerator()
    {
      return commands.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return commands.GetEnumerator();
    }

    public void Update(ModificationCommandContext context)
    {
      var n = 0;
  
      DebugText.Print($"Radius: {context.Modifier.Radius}", TopLeft + DrawCommandDelta * n++, Color.White);
      DebugText.Print($"Mouse: ({context.Input.MousePosition.Y}, {context.Input.MousePosition.Y})", TopLeft + DrawCommandDelta * n++, Color.White);

      foreach (var command in commands)
      {
        if (command.CanExecute(context))
        {
          DebugText.TextColor = Color.Yellow;
          command.Execute(context);
        }
        else
        {
          DebugText.TextColor = Color.White;
        }

        DebugText?.Print($"{command.Name}: {command.Description}", TopLeft + DrawCommandDelta * n++);
      }
    }
  }
}