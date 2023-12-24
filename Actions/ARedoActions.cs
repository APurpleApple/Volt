using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    public class ARedoActions : CardAction
    {
        public Card card;

        public override void Begin(G g, State s, Combat c)
        {
            
            List<CardAction> actions = card.GetActions(s, c);
            foreach (CardAction action in actions)
            {
                c.Queue(action);
            }
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon() { path = Mod.sprites["ActionRedo"] };
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Mod.glossaries["Redo"].Head));
            return list;
        }
    }
}
