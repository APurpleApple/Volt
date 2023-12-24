using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using APurpleApple_VoltMod.Actions;
using APurpleApple_VoltMod.Cards;
using HarmonyLib;

namespace APurpleApple_VoltMod.Patchs
{
    [HarmonyPatch]
    public static class PatchDrawAElectricCost
    {
        [HarmonyPatch(typeof(Card), nameof(Card.RenderAction)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> instrs = new List<CodeInstruction>(instructions);

            MethodInfo IsCostMethod = SymbolExtensions.GetMethodInfo((CardAction action) => IsElectricCost(action));
            MethodInfo GetCostMethod = SymbolExtensions.GetMethodInfo((CardAction action, bool dontDraw) => GetElectricCost(action, dontDraw));
            MethodInfo GetTriggeredMethod = SymbolExtensions.GetMethodInfo((CardAction action) => GetTriggeredAction(action));
            MethodInfo ShipGetStatusMethod = SymbolExtensions.GetMethodInfo(() => GetAvailableCharge());
            MethodInfo RenderCostIconMethod = SymbolExtensions.GetMethodInfo((bool costMet, int w, G g, bool dontDraw, int iconWidth) => RenderElectricChargeCostIcon(costMet, ref w, g, dontDraw, iconWidth));

            LocalBuilder Cost = generator.DeclareLocal(typeof(int));
            LocalBuilder ChargeAvailable = generator.DeclareLocal(typeof(int));

            Label jumpIfNotElectricCostLabel = generator.DefineLabel();
            Label jumpIfCostZeroLabel = generator.DefineLabel();
            Label loopStartLabel = generator.DefineLabel();
            Label loopEntryLabel = generator.DefineLabel();

            FieldInfo wfield = typeof(Card).GetNestedTypes(AccessTools.all).SelectMany(t => t.GetFields(AccessTools.all)).First(f => f.Name == "w");
            FieldInfo icoWidthField = typeof(Card).GetNestedTypes(AccessTools.all).SelectMany(t => t.GetFields(AccessTools.all)).First(f => f.Name == "iconWidth");
            
            int workingIndex = 0;

            for (; workingIndex < instrs.Count; workingIndex++)
            {
                if (instrs[workingIndex].ToString() == "ldloca.s 0 (Card+<>c__DisplayClass57_0)")
                {
                    // set cost to 0
                    instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldc_I4_0));
                    workingIndex++;
                    instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Stloc, Cost));
                    workingIndex++;

                    // check if it has a cost
                    instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldarg, 2));
                    workingIndex ++;
                    instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Call, IsCostMethod));
                    workingIndex++;

                    // if has a cost
                    instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Brfalse, jumpIfNotElectricCostLabel));
                    workingIndex++;


                        // set charge available
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Call, ShipGetStatusMethod));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Stloc, ChargeAvailable));
                        workingIndex++;

                        // get cost and set it
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldarg, 2));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldarg, 3)); // push dontDraw onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Call, GetCostMethod));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Stloc, Cost));
                        workingIndex++;

                        // replace action
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldarg, 2));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Call, GetTriggeredMethod));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Starg, 2));
                        workingIndex++;

                    // end of if
                    instrs[workingIndex].labels.Add(jumpIfNotElectricCostLabel);
                    break;
                }
            }

            for (; workingIndex < instrs.Count; workingIndex++)
            {
                if (instrs[workingIndex].opcode == OpCodes.Ldloc_0)
                {
                    if (instrs[workingIndex+2].ToString() == "ldflda System.Nullable`1<System.Int32> CardAction::shardcost")
                    {
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc, Cost));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Brfalse, jumpIfCostZeroLabel));
                        workingIndex++;

                        //setting index to 0;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldc_I4_0));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Stloc, 12));
                        workingIndex++;

                        // go to loop entry
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Br, loopEntryLabel));
                        workingIndex++;

                        //start of loop
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Nop));
                        instrs[workingIndex].labels.Add(loopStartLabel);
                        workingIndex++;

                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc, 12)); // pushing index onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc, ChargeAvailable)); // pushing available charge onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Clt)); // remove 2 previous, push < onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc_0)); // push the card onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldflda, wfield)); // pop the card, push the content of wfield
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldarg, 0)); // push G onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldarg, 3)); // push dontDraw onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc_0)); // push the card onto the stack
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldfld, icoWidthField)); // pop the card, push the content of iconWidthField
                        workingIndex++;

                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Call, RenderCostIconMethod)); // execute the method, removing all the above
                        workingIndex++;

                        //incrementing index
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc, 12));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldc_I4_1));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Add));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Stloc, 12));
                        workingIndex++;

                        //loop entry point, check for condition
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc, 12));
                        instrs[workingIndex].labels.Add(loopEntryLabel);
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc, Cost));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Blt, loopStartLabel));
                        workingIndex++;


                        // w += 3 after loop
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloca, 0));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldloc_0));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldfld, wfield)); // w
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Ldc_I4_3));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Add));
                        workingIndex++;
                        instrs.Insert(workingIndex, new CodeInstruction(OpCodes.Stfld, wfield)); // w
                        workingIndex++;

                        instrs[workingIndex].labels.Add(jumpIfCostZeroLabel);

                        break;
                    }
                }
            }

            return instrs.AsEnumerable();
        }

        public static int electricChargeCounter = 0;

        [HarmonyPatch(typeof(Card), nameof(Card.MakeAllActionIcons)), HarmonyPrefix]
        public static void ResetCounter(State s, Card __instance)
        {
            electricChargeCounter = s.ship.Get(Mod.statuses["ElectricCharge"]);
        }

        public static void RenderElectricChargeCostIcon(bool costMet, ref int w, G g, bool dontDraw, int iconWidth)
        {
            w--;
            if (!dontDraw)
            {
                Rect? rect = new Rect(w);
                Vec xy = g.Push(null, rect).rect.xy;
                Spr? id = (costMet ? Mod.sprites["ElectricChargeCost"] : Mod.sprites["ElectricChargeCostOff"]);
                Draw.Sprite(id, xy.x, xy.y, flipX: false, flipY: false, 0.0, null, null, null, null, null);
                g.Pop();
            }

            w += iconWidth - 1;
        }

        public static int GetAvailableCharge()
        {
            return electricChargeCounter;
        }

        public static CardAction GetTriggeredAction(CardAction action)
        {
            return action is AElectricCost electricCost ? electricCost.triggeredAction : new CardAction();
        } 

        public static bool IsElectricCost(CardAction action)
        {
            return action is AElectricCost;
        }

        public static int GetElectricCost(CardAction action, bool dontDraw)
        {
            int cost = action is AElectricCost electricCost ? electricCost.cost : 0;
            if (!dontDraw)
            {
                electricChargeCounter -= cost;
            }
            return cost;
        }

        //[HarmonyPatch(typeof(Combat), nameof(Combat.DrainCardActions)), HarmonyPrefix]
    }
}
