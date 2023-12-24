using APurpleApple_VoltMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltSharpAim : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus() { status = Mod.statuses["PierceCharge"], statusAmount = 1, targetPlayer = true});
                    break;

                case Upgrade.A:
                    list.Add(new AStatus() { status = Mod.statuses["PierceCharge"], statusAmount = 1, targetPlayer = true });
                    list.Add(new AStatus() { status = Mod.statuses["ElectricCharge"], statusAmount = 1, targetPlayer = true });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus() { status = Mod.statuses["PierceCharge"], statusAmount = 3, targetPlayer = true });
                    break;
            }

            return list;
        }
        public override CardData GetData(State state)
        {
            CardData data = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    data.cost = 2;
                    break;
                case Upgrade.A:
                    data.cost = 2;
                    break;
                case Upgrade.B:
                    data.cost = 1;
                    break;
            }

            return data;
        }
    }
}
