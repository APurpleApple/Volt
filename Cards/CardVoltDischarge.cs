using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class CardVoltDischarge : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            int cannonX = 0;
            for (int i = 0; i < s.ship.parts.Count; i++)
            {
                if (s.ship.parts[i].type == PType.cannon)
                {
                    cannonX = i + s.ship.x;
                    break;
                }
            }
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AVariableHint() { status = Mod.statuses["ElectricCharge"] });
                    list.Add(new AAttack() { damage = GetDmg(s, GetChargeTotal(s)), xHint = 1 });
                    break;

                case Upgrade.A:
                    list.Add(new AStatus() { targetPlayer = true, status = Mod.statuses["ElectricCharge"], statusAmount = 1 });
                    list.Add(new AVariableHint() { status = Mod.statuses["ElectricCharge"] });
                    list.Add(new AAttack() { damage = GetDmg(s, GetChargeTotal(s)), xHint = 1 });
                    break;

                case Upgrade.B:
                    list.Add(new AVariableHint() { status = Mod.statuses["ElectricCharge"] });
                    list.Add(new AAttack() { damage = GetDmg(s, GetChargeTotal(s)*2), xHint = 2 });
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
                    data.cost = 2;
                    data.exhaust = true;
                    break;

                case Upgrade.A:
                    data.cost = 2;
                    data.retain = true;
                    data.exhaust = true;
                    break;

                case Upgrade.B:
                    data.cost = 2;
                    data.exhaust = true;
                    break;
            }
            return data;
        }

        private int GetChargeTotal(State s)
        {
            int total = s.ship.Get(Mod.statuses["ElectricCharge"]);
            if (upgrade == Upgrade.A)
            {
                total+=2;
            }
            return total;
        }
    }
}
