using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    internal class AEnergyHint : CardAction, IAOversized
    {
        public int offset => -12;

        public Icon icon => new Icon() { path = Mod.sprites["EnergyHint"] };

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            int value = 1;
            if ((s.route is Combat))
            {
                Combat c = (Combat)s.route;
                value = c.energy;
            }
            list.Add(new TTGlossary("action.xHint.desc", "<c=status>" + "ENERGY" + "</c>", (s.route is Combat) ? $" </c>(<c=keyword>{value}</c>)" : "", "", ""));
            return list;
        }
    }
}
