using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APurpleApple_VoltMod.Actions;
using APurpleApple_VoltMod.Artifacts;
using HarmonyLib;

namespace APurpleApple_VoltMod.Patchs
{
    [HarmonyPatch]
    public static class PatchElectricCharges
    {

        [HarmonyPatch(typeof(Card), nameof(Card.GetActualDamage)), HarmonyPostfix]
        public static void GetElectricChargeDamage(ref int __result, State s, bool targetPlayer)
        {
            Ship otherShip = s.route is Combat route ? route.otherShip : (Ship)null;
            Ship ship = targetPlayer ? otherShip : s.ship;
            __result += ship.Get(Mod.statuses["ElectricCharge"]);

            if (!targetPlayer)
            {
                foreach (Artifact art in s.artifacts)
                {
                    if (art is ArtifactOuranosCannon artifactOuranosCannon)
                    {
                        if (!artifactOuranosCannon.isCannonActive)
                        {
                            __result = 0;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Ship), nameof(Ship.NormalDamage)), HarmonyPostfix]
        public static void ReduceElectricCharge(Ship __instance, Combat c)
        {
            if (__instance.Get(Mod.statuses["ElectricCharge"]) > 0)
            {
                c.QueueImmediate(new AStatus() { status = Mod.statuses["ElectricCharge"], statusAmount = -1, targetPlayer = __instance.isPlayerShip });
            }
        }

        //[HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin)), HarmonyPostfix]
        public static void RemoveElectricCharge(AAttack __instance, G __0, State __1, Combat __2, bool __runOriginal)
        {
            G g = __0;
            State s = __1;
            Combat c = __2;
            if (__runOriginal && !__instance.fromDroneX.HasValue)
            {
                Ship source = __instance.targetPlayer ? c.otherShip : s.ship;
                Ship other = __instance.targetPlayer ? s.ship : c.otherShip;
                int electric_charge_count = source.Get(Mod.statuses["ElectricCharge"]);

                if (electric_charge_count > 0)
                {
                    var key = DB.story.QuickLookup(s, ".APurpleApple.electric_charge_release");
                    if (key != null)
                    {
                        Narrative.ActivateShoutSequence(g, c, key);
                    }

                    //Vec impactLoc = other.GetShipRect().Center();
                    //PFX.screenSpaceAdd.Add(new Particle { lifetime = 3, sprite = (Spr)VoltPortrait.Id, pos = impactLoc });
                    c.QueueImmediate(new AStatus() { status = Mod.statuses["ElectricCharge"], statusAmount = -electric_charge_count, targetPlayer = source.isPlayerShip });
                }

                if (__instance.targetPlayer) { return; }

                foreach (Artifact art in s.artifacts)
                {
                    if (art is ArtifactOuranosCannon ouranosCannon)
                    {
                        if (__instance.damage > 0)
                        {
                            EffectSpawnerExtension.RailgunBeam(c, s.ship.parts.FindIndex((Part p) => p.type == PType.cannon && p.active) + s.ship.x, __instance.damage, new Color("ff8866"));
                        }
                        break;
                    }
                    if (art is ArtifactOuranosCannonV2 ouranosCannonV2)
                    {
                        EffectSpawnerExtension.RailgunBeam(c, s.ship.parts.FindIndex((Part p) => p.type == PType.cannon && p.active) + s.ship.x, __instance.damage, new Color("ff6644"));
                        break;
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(AAttack), nameof(AAttack.Begin)), HarmonyPrefix]
        public static bool StoreAttackInCannon(State __1, Combat __2, AAttack __instance)
        {
            State s = __1;
            Combat c = __2;
            if (!__instance.targetPlayer)
            {
                foreach (Artifact art in s.artifacts)
                {
                    if (art is ArtifactOuranosCannonV2)
                    {
                        var cv2 = art as ArtifactOuranosCannonV2;
                        if (!cv2.allowAttacks)
                        {
                            cv2.StoreAttack(s, c, __instance);
                            return false;
                        }
                        else
                        {

                        }
                        break;
                    }
                }
            }
            return true;
        }

    }
}
