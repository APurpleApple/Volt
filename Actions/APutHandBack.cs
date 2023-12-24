using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    public class APutHandBack : CardAction, IAOversized
    {
        public int offset => -9;
        public Icon icon => new Icon() { path = Mod.sprites["ActionPlaceHandOnTop"] };

        public override void Begin(G g, State s, Combat c)
        {
            foreach (Card card in c.hand)
            {
                s.RemoveCardFromWhereverItIs(card.uuid);
                card.targetPos = Combat.deckPos + new Vec(2.0);
                s.deck.Add(card);
            }
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Mod.glossaries["PutBackHand"].Head));
            return list;
        }
    }
}
