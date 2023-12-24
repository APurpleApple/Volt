using APurpleApple_VoltMod.Actions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class CardVoltRamm : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AVariableHint() { status = Status.evade });
                    list.Add(new ARamAnim() { timer = 0.2 });
                    list.Add(new ARamAttack() { targetPlayer = false, hurtAmount = GetDmg(s, s.ship.Get(Status.evade)), timer = 0, xHint = 1 });
                    list.Add(new ASelfDamage() {amount = 1, timer = 0.4});
                    break;

                case Upgrade.A:
                    list.Add(new AVariableHint() { status = Status.evade });
                    list.Add(new ARamAnim() { timer = 0.2 });
                    list.Add(new ARamAttack() { targetPlayer = false, hurtAmount = GetDmg(s, s.ship.Get(Status.evade)), timer = 0, xHint = 1 });
                    list.Add(new ASelfDamage() { amount = 1, timer = 0.4 });
                    break;

                case Upgrade.B:
                    list.Add(new AVariableHint() { status = Status.evade });
                    list.Add(new ARamAnim() { timer = 0.2 });
                    list.Add(new ARamAttack() { targetPlayer = false, hurtAmount = GetDmg(s, s.ship.Get(Status.evade)), timer = 0, xHint = 1 });
                    list.Add(new ASelfDamage() { amount = 1, timer = 0.4 });
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
