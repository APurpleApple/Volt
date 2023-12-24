using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    public class APutBackOnTop : CardAction
    {
        public int amount = 1;

        public override void Begin(G g, State s, Combat c)
        {
            Card selectedCard = this.selectedCard;
            s.RemoveCardFromWhereverItIs(selectedCard.uuid);
            selectedCard.targetPos = Combat.deckPos + new Vec(2.0);
            s.deck.Add(selectedCard);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Mod.glossaries["PutBack"].Head));
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Mod.sprites["ActionPlaceOnTop"], amount, Colors.textMain);
        }
    }
}
