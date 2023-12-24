using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    public class AElectricCost : CardAction
    {
        public CardAction triggeredAction;
        public int cost;

        public override void Begin(G g, State s, Combat c)
        {
            timer = 0;
            if (s.ship.Get(Mod.statuses["ElectricCharge"]) >= cost)
            {
                c.QueueImmediate(triggeredAction);
                g.state.ship.Set(Mod.statuses["ElectricCharge"], g.state.ship.Get(Mod.statuses["ElectricCharge"]) - cost);
            }
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = triggeredAction.GetTooltips(s);
            tooltips.Add(new TTGlossary(Mod.glossaries["ElectricCharge"].Head, new object[] { cost }));
            return tooltips;
        }
    }
}
