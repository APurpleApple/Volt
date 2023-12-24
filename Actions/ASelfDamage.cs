using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    internal class ASelfDamage : CardAction
    {
        public int amount = 0;

        public override void Begin(G g, State s, Combat c)
        {
            s.ship.NormalDamage(s, c, amount, null);
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Spr.icons_hurtBlockable, amount, Colors.damage);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Mod.glossaries["SelfDamage"].Head, amount));
            return list;
        }
    }
}
