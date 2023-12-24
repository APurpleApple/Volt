using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    internal class AAddToHand : CardAction
    {
        public Card card;

        public override void Begin(G g, State s, Combat c)
        {
            s.RemoveCardFromWhereverItIs(card.uuid);
            c.SendCardToHand(s, card);
        }
    }
}
