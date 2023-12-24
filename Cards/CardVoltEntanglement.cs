using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltEntanglement : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            if (s.deck.Count > 0 && s.deck[s.deck.Count - 1] != this)
            {
                return s.deck[s.deck.Count - 1].GetActions(s, c);
            }
            return new List<CardAction>();
        }

        public override CardData GetData(State state)
        {
            CardData data = new CardData();

            if (state.deck.Count > 0 && state.deck[state.deck.Count - 1] != this && state.route is Combat c)
            {
                CardData copiedData = state.deck[state.deck.Count - 1].GetData(state);
                data.description = copiedData.description;

                switch (upgrade)
                {
                    case Upgrade.None:
                        data.cost = 1;
                        data.cost = copiedData.cost;
                        break;
                    case Upgrade.A:
                        data.cost = 0;
                        data.exhaust = copiedData.exhaust;
                        break;
                    case Upgrade.B:
                        data.cost = 1;
                        data.retain = true;
                        break;
                }
            }
            else
            {
                switch (upgrade)
                {
                    case Upgrade.None:
                        data.description = "Copy the actions and cost of the top card of your deck.";
                        data.cost = 0;
                        data.unplayable = true;
                        break;
                    case Upgrade.A:
                        data.description = "Copy the actions of the top card of your deck.";
                        data.cost = 0;
                        data.exhaust = true;
                        data.unplayable = true;
                        break;
                    case Upgrade.B:
                        data.description = "Copy the actions of the top card of your deck.";
                        data.cost = 1;
                        data.retain = true;
                        data.unplayable = true;
                        break;
                }
            }

            return data;
        }
    }
}
