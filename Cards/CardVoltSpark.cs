using CobaltCoreModding.Definitions.ModManifests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class CardVoltSpark : Card
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
                    list.Add(new ADrawCard() { count = 1 });
                    list.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["ElectricCharge"], statusAmount = 1 });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["ElectricCharge"], statusAmount = 2 });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = 1,
        };
    }
}
