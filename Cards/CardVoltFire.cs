using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class CardVoltFire : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AAttack() { damage = GetDmg(s, 5)});
                    list.Add(new AAddCard() { amount = 1, card = new CardVoltReady(), callItTheDeckNotTheDrawPile = true, destination = CardDestination.Deck });
                    break;

                case Upgrade.A:
                    list.Add(new AAttack() { damage = GetDmg(s, 5) });
                    list.Add(new AAddCard() { amount = 1, card = new CardVoltReady(), callItTheDeckNotTheDrawPile = true, destination = CardDestination.Deck });
                    break;

                case Upgrade.B:
                    list.Add(new AAttack() { damage = GetDmg(s, 10) });
                    list.Add(new AAddCard() { amount = 1, card = new CardVoltReady(), callItTheDeckNotTheDrawPile = true, destination = CardDestination.Deck });
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
                    data.cost = 3;
                    data.singleUse = true;
                    break;

                case Upgrade.A:
                    data.cost = 2;
                    data.singleUse = true;
                    break;

                case Upgrade.B:
                    data.cost = 3;
                    data.singleUse = true;
                    break;
            }
            return data;
        }
    }
}
