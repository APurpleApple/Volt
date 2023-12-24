using APurpleApple_VoltMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltBlastThrough : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> cardActions = new List<CardAction>();

            switch (upgrade)
            {
                case Upgrade.None:
                    cardActions.Add(new ABeamAttack() { damage = 3});
                    break;
                case Upgrade.A:
                    cardActions.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["PierceCharge"], statusAmount = 1 });
                    cardActions.Add(new ABeamAttack() { damage = 2 });
                    break;
                case Upgrade.B:
                    cardActions.Add(new ABeamAttack() { damage = 5 });
                    break;
            }

            return cardActions;
        }

        public override CardData GetData(State state)
        {
            CardData cardData = new CardData();

            cardData.cost = 2;

            return cardData;
        }
    }
}
