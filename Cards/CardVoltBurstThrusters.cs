using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltBurstThrusters : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AMove() { dir = 2, targetPlayer = true });
                    list.Add(new AStatus() { status = Status.evade, statusAmount = 1, targetPlayer = true });
                    list.Add(new AStatus() { status = Status.engineStall, statusAmount = 1, targetPlayer = true });
                    break;

                case Upgrade.A:
                    list.Add(new AMove() { dir = 3, targetPlayer = true });
                    list.Add(new AStatus() { status = Status.evade, statusAmount = 3, targetPlayer = true });
                    list.Add(new AStatus() { status = Status.engineStall, statusAmount = 2, targetPlayer = true });
                    break;

                case Upgrade.B:
                    list.Add(new AMove() { dir = 2, targetPlayer = true });
                    list.Add(new AStatus() { status = Status.evade, statusAmount = 2, targetPlayer = true });
                    list.Add(new AStatus() { status = Status.engineStall, statusAmount = 1, targetPlayer = true });
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
                    data.cost = 1;
                    break;
                case Upgrade.A:
                    data.cost = 1;
                    break;
                case Upgrade.B:
                    data.cost = 1;
                    data.flippable = true;
                    break;
            }

            return data;
        }
    }
}
