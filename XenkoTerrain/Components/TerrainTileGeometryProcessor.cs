using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Design;
using Xenko.Games;
using Xenko.Rendering;
using XenkoTerrain.Graphics;
using XenkoTerrain.Rendering;

namespace XenkoTerrain.Components
{
  public class TerrainTileGeometryProcessor : EntityProcessor<TerrainEntityComponent>
  { 

    public override void Draw(RenderContext context)
    {

    }

 
  }

  [DataContract(nameof(TerrainSystemEntityComponent))]
  [Display("Terrain System", Expand = ExpandRule.Once)]
  [DefaultEntityComponentProcessor(typeof(TerrainTileGeometryProcessor))]
  [ComponentOrder(101)]
  public class TerrainSystemEntityComponent : StartupScript
  {
  }
}
