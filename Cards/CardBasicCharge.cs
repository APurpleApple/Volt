using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, dontOffer = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class CardBasicCharge : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["ElectricCharge"], statusAmount = 1 });
                    break;

                case Upgrade.A:
                    list.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["ElectricCharge"], statusAmount = 1 });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["ElectricCharge"], statusAmount = 2 });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    data.cost = 1;
                    break;

                case Upgrade.A:
                    data.cost = 0;
                    break;

                case Upgrade.B:
                    data.cost = 1;
                    break;
            }
            return data;
        }
    }
}
