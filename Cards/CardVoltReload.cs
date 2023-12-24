using APurpleApple_VoltMod.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CardVoltReload : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ACardSelect() { browseAction = new APutBackOnTop(), browseSource = CardBrowse.Source.Hand });
                    AIcon iconAction = new AIcon() { icon = new Icon() { path = Mod.sprites["ActionPlaceOnTop"] } };
                    iconAction.tooltips.Add(new TTGlossary(Mod.glossaries["PutBack"].Head));
                    list.Add(iconAction);
                    list.Add(new ADrawFromDiscard() { count = 1});
                    break;

                case Upgrade.A:
                    list.Add(new ACardSelect() { browseAction = new APutBackOnTop(), browseSource = CardBrowse.Source.Hand });
                    AIcon iconAction1 = new AIcon() { icon = new Icon() { path = Mod.sprites["ActionPlaceOnTop"] } };
                    iconAction1.tooltips.Add(new TTGlossary(Mod.glossaries["PutBack"].Head));
                    list.Add(iconAction1);
                    list.Add(new ADrawFromDiscard() { count = 1 });
                    break;

                case Upgrade.B:
                    list.Add(new APutHandBack());
                    list.Add(new ADrawFromDiscard() { count = 5 });
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
                    data.infinite = true;
                    break;

                case Upgrade.A:
                    data.cost = 0;
                    data.exhaust = true;
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
