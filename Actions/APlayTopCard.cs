using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    public class APlayTopCard : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            if (s.deck.Count == 0) { return; }
            Card card = s.deck[s.deck.Count - 1];
            s.RemoveCardFromWhereverItIs(card.uuid);
            c.hand.Add(card);
            c.TryPlayCard(s, card, true);
        }
        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Mod.glossaries["PlayTopCard"].Head));
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon() { path =  Mod.sprites["ActionPlayTopCard"] };
        }
    }
}
