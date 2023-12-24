using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltHECK : Card
    {
        public int charge = 0;
        public override void OnExitCombat(State s, Combat c)
        {
            charge = 0;
        }
    }
}
