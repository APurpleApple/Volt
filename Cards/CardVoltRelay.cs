using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APurpleApple_VoltMod;
using APurpleApple_VoltMod.Actions;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class CardVoltRelay : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AEnergyHint());
                    list.Add(new AStatus() { targetPlayer = true, status = Status.energyNextTurn, statusAmount = GetEnergyLeft(s, c), xHint = 1});
                    list.Add(new AEndTurn());
                    break;

                case Upgrade.A:
                    list.Add(new AEnergyHint());
                    list.Add(new AStatus() { targetPlayer = true, status = Status.energyNextTurn, statusAmount = GetEnergyLeft(s, c), xHint = 1 });
                    list.Add(new AEndTurn());
                    break;

                case Upgrade.B:
                    list.Add(new AEnergyHint());
                    list.Add(new AStatus() { targetPlayer = true, status = Status.energyNextTurn, statusAmount = GetEnergyLeft(s, c), xHint = 1 });
                    list.Add(new AEndTurn());
                    break;
            }

            return list;
        }

        private int GetEnergyLeft(State s, Combat c)
        {
            int value = s.route is Combat ? c.energy : 1;
            //value = GetDataWithOverrides(s).cost;
            return value;
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData();
            Upgrade upgrade = this.upgrade;
            switch (upgrade)
            {
                case Upgrade.None:
                    data.cost = 1;
                    break;
                case Upgrade.A:
                    data.cost = 0;
                    break;
                case Upgrade.B:
                    data.cost = 1;
                    data.retain = true;
                    break;
            }
            return data;
        }
    }
}
