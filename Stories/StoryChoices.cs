using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Stories
{
    public static class StoryChoices
    {
        public static List<Choice> AddScaffoldModified(State s)
        {
            Part part = new Part()
            {
                type = PType.empty,
                skin = "scaffolding"
            };

            List<Choice> choiceList = new List<Choice>();
            Choice choice = new Choice();
            choice.label = "Sure!\n<c=choiceBold> - Add an unshootable scaffold in the middle of the ship</c>";
            choice.key = "AddScaffold_Yes";
            List<CardAction> actions = choice.actions;
            AShipUpgrades ashipUpgrades = new AShipUpgrades();
            ashipUpgrades.actions.Add((CardAction)new AInsertPart()
            {
                targetPlayer = true,
                x = (int)Math.Ceiling((double)s.ship.parts.Count / 2.0),
                part = part
            });
            actions.Add((CardAction)ashipUpgrades);
            choiceList.Add(choice);
            choiceList.Add(new Choice()
            {
                label = Loc.T("AddScaffold_No", "No thanks."),
                key = "AddScaffold_No"
            });
            choiceList.Add(new Choice()
            {
                label = "*Add an unshootable scaffold in the middle of HER ship*",
                key = "AddScaffold_Reverse"
            });
            return choiceList;
        }
    }
}
