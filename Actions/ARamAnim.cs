﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APurpleApple_VoltMod.VFXs;
namespace APurpleApple_VoltMod.Actions
{
    internal class ARamAnim : CardAction, IAInvisible
    {
        public override void Begin(G g, State s, Combat c)
        {
            c.fx.Add(new ShipRamm());
        }
    }
}
