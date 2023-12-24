using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class CardVoltAim : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new AStatus() { status = Mod.statuses["PierceCharge"], statusAmount = 1, targetPlayer = true });
                    list.Add(new AAddCard() { amount = 1, card = new CardVoltFire(), callItTheDeckNotTheDrawPile = true, destination = CardDestination.Deck });
                    break;

                case Upgrade.A:
                    list.Add(new AStatus() { status = Mod.statuses["PierceCharge"], statusAmount = 1, targetPlayer = true });
                    list.Add(new AAddCard() { amount = 1, card = new CardVoltFire(), callItTheDeckNotTheDrawPile = true, destination = CardDestination.Deck });
                    break;

                case Upgrade.B:
                    list.Add(new AStatus() { status = Mod.statuses["PierceCharge"], statusAmount = 1, targetPlayer = true });
                    list.Add(new AAddCard() { amount = 1, card = new CardVoltFire(), callItTheDeckNotTheDrawPile = true, destination = CardDestination.Deck });
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
                    data.singleUse = true;
                    break;

                case Upgrade.A:
                    data.cost = 1;
                    data.singleUse = true;
                    break;

                case Upgrade.B:
                    data.cost = 2;
                    data.singleUse = true;
                    break;
            }
            return data;
        }
    }
}
