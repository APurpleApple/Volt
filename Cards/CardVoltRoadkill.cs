using APurpleApple_VoltMod.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltRoadkill : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AMove() { dir = -2, targetPlayer = true});
                    list.Add(new ARamAnim() { timer = 0.2 });
                    list.Add(new ARamAttack() { targetPlayer = false, hurtAmount = GetDmg(s, 4), timer = 0});
                    list.Add(new ASelfDamage() { amount = 1, timer = 0.4 });
                    list.Add(new AMove() { dir = 1, isRandom = true, targetPlayer = true});
                    break;

                case Upgrade.A:
                    list.Add(new AMove() { dir = -2, targetPlayer = true });
                    list.Add(new ARamAnim() { timer = 0.2 });
                    list.Add(new ARamAttack() { targetPlayer = false, hurtAmount = GetDmg(s, 4), timer = 0 });
                    list.Add(new ASelfDamage() { amount = 1, timer = 0.4 });
                    list.Add(new AMove() { dir = 1, isRandom = true, targetPlayer = true });
                    break;

                case Upgrade.B:
                    list.Add(new AMove() { dir = -1, targetPlayer = true });
                    list.Add(new ARamAnim() { timer = 0.2 });
                    list.Add(new ARamAttack() { targetPlayer = false, hurtAmount = GetDmg(s, 3), timer = 0 });
                    list.Add(new ASelfDamage() { amount = 1, timer = 0.4 });
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
                    data.flippable = true;
                 break;
                case Upgrade.B:
                    data.cost = 1;
                    data.infinite = true;
                 break;
	        }

            return data;
        }
    }
}
