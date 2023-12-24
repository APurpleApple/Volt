using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    internal class AIcon : CardAction
    {
        public Icon icon = new Icon();
        public List<Tooltip> tooltips = new List<Tooltip>();

        public override Icon? GetIcon(State s)
        {
            return icon;
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            return tooltips;
        }
    }
}
