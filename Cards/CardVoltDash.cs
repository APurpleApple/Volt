using APurpleApple_VoltMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class CardVoltDash : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new APlayTopCard());
                    list.Add(new AElectricCost() { cost = 1, triggeredAction = new AAttack() { damage = 1 } });
                    break;

                case Upgrade.A:

                    break;

                case Upgrade.B:

                    break;
            }

            return list;
        }
    }
}
