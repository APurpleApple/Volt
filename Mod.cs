using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using APurpleApple_VoltMod.Cards;
using APurpleApple_VoltMod.Actions;
using System.Drawing;
using APurpleApple_VoltMod.Artifacts;
using System.Diagnostics;
using APurpleApple_VoltMod.Stories;
using System.Globalization;
using Shockah.Shared;
using APurpleApple_VoltMod.Patchs;

namespace APurpleApple_VoltMod
{
    internal class Mod : IModManifest, ISpriteManifest, ICharacterManifest, IDeckManifest, IAnimationManifest, ICardManifest, IStatusManifest, IGlossaryManifest, IArtifactManifest, IStoryManifest, IStartershipManifest, IShipManifest, IShipPartManifest
    {
        public IEnumerable<DependencyEntry> Dependencies { get { yield break; } }
        public DirectoryInfo? GameRootFolder { get; set; }
        public ILogger? Logger { get; set; }
        public DirectoryInfo? ModRootFolder { get; set; }

        public string Name => "APurpleApple.VoltMod";

        public static Dictionary<string, ExternalSprite> extSprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, Spr> sprites = new Dictionary<string, Spr>();
        public static Dictionary<string, ExternalStatus> extStatuses = new Dictionary<string, ExternalStatus>();
        public static Dictionary<string, Status> statuses = new Dictionary<string, Status>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        public static Dictionary<string, ExternalGlossary> glossaries = new Dictionary<string, ExternalGlossary>();
        public static Dictionary<string, ExternalAnimation> animations = new Dictionary<string, ExternalAnimation>();
        public static Dictionary<string, ExternalArtifact> artifacts = new Dictionary<string, ExternalArtifact>();
        public static Dictionary<string, ExternalCard> cards = new Dictionary<string, ExternalCard>();

        public static ExternalDeck? VoltDeck { get; private set; }
        public static ExternalCharacter? VoltCharacter { get; private set; }
        public static ExternalShip? OuranosShip { get; set; }


        public void BootMod(IModLoaderContact contact)
        {
            Harmony harmony = new Harmony("APurpleApple.VoltMod");

            var pref = new HarmonyMethod(typeof(PatchElectricCharges).GetMethod(nameof(PatchElectricCharges.StoreAttackInCannon)));
            var postf = new HarmonyMethod(typeof(PatchElectricCharges).GetMethod(nameof(PatchElectricCharges.RemoveElectricCharge)));
            //harmony.PatchVirtual(typeof(AAttack).GetMethod(nameof(AAttack.Begin)), Logger, prefix: pref, postfix: postf);

            harmony.PatchAll();
        }
        

        private void RegisterSprite(string subfile, string fileName, string name, ISpriteRegistry artRegistry)
        {
            var file_name = Path.Combine(ModRootFolder?.FullName ?? "", "Sprites", subfile, fileName);
            var externalSprite = new ExternalSprite(Name + ".Sprite." + name, new FileInfo(file_name));
            artRegistry.RegisterArt(externalSprite);
            extSprites.Add(name, externalSprite);
            sprites.Add(name, (Spr)externalSprite.Id);
        }

        private void RegisterAnimation(string name, string tag, ExternalDeck deck, ExternalSprite[] sprites ,IAnimationRegistry registry)
        {
            ExternalAnimation anim = new ExternalAnimation(Name + ".Animation." + name, deck, tag, false, sprites);
            registry.RegisterAnimation(anim);
            animations.Add(name, anim);
        }

        private void RegisterCard(string name, Type cardType, ExternalSprite art, ExternalDeck? deck, string descName, ICardRegistry registry)
        {
            ExternalCard card = new ExternalCard(Name + ".Card." + name, cardType, art, deck);
            card.AddLocalisation(descName);
            registry.RegisterCard(card);
            cards.Add(name, card);
        }

        private void RegisterArtifact(string name, Type artifactType, ExternalSprite sprite, string descName, string desc, IArtifactRegistry registry)
        {
            ExternalArtifact artifact = new ExternalArtifact(Name + ".Artifact." + name, artifactType, sprite);
            artifact.AddLocalisation(descName, desc);
            registry.RegisterArtifact(artifact);
            artifacts.Add(name, artifact);
        }

        private void RegisterPart(string name, Part part, ExternalSprite on, ExternalSprite? off, IShipPartRegistry registry)
        {
            Console.WriteLine("Registering " + name);
            ExternalPart newPart = new ExternalPart(Name + ".Part." + name, part, on, off);
            registry.RegisterPart(newPart);
            parts.Add(name, newPart);
        }

        private void RegisterGlossary(string name, ExternalGlossary.GlossayType type, ExternalSprite icon, string descName, string desc, string? altDesc, IGlossaryRegisty registry)
        {
            ExternalGlossary glossary = new ExternalGlossary(Name + ".Glossary." + name, name, false, type, icon);
            glossary.AddLocalisation("en", descName, desc, altDesc);
            registry.RegisterGlossary(glossary);
            glossaries.Add(name, glossary);
        }

        private void RegisterStatus(string name, bool isGood, System.Drawing.Color mainColor, System.Drawing.Color borderColor, ExternalSprite icon, bool affectedByTimestop, string descName, string desc, IStatusRegistry registry)
        {
            ExternalStatus status = new ExternalStatus(Name + ".Status." + name, isGood, mainColor, borderColor, icon, affectedByTimestop);
            status.AddLocalisation(descName, desc);
            registry.RegisterStatus(status);
            statuses.Add(name, (Status)status.Id);
            extStatuses.Add(name, status);
        }

        public void LoadManifest(ISpriteRegistry artRegistry)
        {
            RegisterSprite("Characters", "volt.png", "VoltPortrait", artRegistry);
            RegisterSprite("Characters", "volt_mini.png","VoltMini", artRegistry);
            RegisterSprite("Characters", "char_volt.png", "VoltPanel", artRegistry);

            RegisterSprite("Cards", "volt_card_default.png", "VoltCard", artRegistry);
            RegisterSprite("Cards", "volt_card_reload.png", "VoltCardReload", artRegistry);
            RegisterSprite("Cards", "border_volt.png", "VoltBorder", artRegistry);

            RegisterSprite("Icons", "powerfocus.png", "ElectricCharge", artRegistry);
            RegisterSprite("Icons", "powerfocus_cost_full.png", "ElectricChargeCost", artRegistry);
            RegisterSprite("Icons", "powerfocus_cost_off.png", "ElectricChargeCostOff", artRegistry);
            RegisterSprite("Icons", "status_railgun_beam_icon.png", "RailgunBeam", artRegistry);
            RegisterSprite("Icons", "energyhint.png", "EnergyHint", artRegistry);
            RegisterSprite("Icons", "hurtenemyicon.png", "ActionHurtEnemy", artRegistry);
            RegisterSprite("Icons", "RedoIcon.png", "ActionRedo", artRegistry);
            RegisterSprite("Icons", "PlayTopCardIcon.png", "ActionPlayTopCard", artRegistry);
            RegisterSprite("Icons", "icon_place_on_top.png", "ActionPlaceOnTop", artRegistry);
            RegisterSprite("Icons", "icon_put_back_hand.png", "ActionPlaceHandOnTop", artRegistry);
            RegisterSprite("Icons", "drawDiscardIcon.png", "ActionDrawFromDiscard", artRegistry);
            RegisterSprite("Icons", "beamicon.png", "ActionBeam", artRegistry);
            RegisterSprite("Icons", "beamChargeicon.png", "BeamCharge", artRegistry);
            RegisterSprite("Icons", "pierceCharge.png", "PierceCharge", artRegistry);
            RegisterSprite("Icons", "attackstall.png", "AttackStall", artRegistry);

            RegisterSprite("Parts", "ouranos_cannon.png", "OuranosCannon", artRegistry);
            RegisterSprite("Parts", "ouranos_cannon_off.png", "OuranosCannonOff", artRegistry);
            RegisterSprite("Parts", "ouranos_cockpit.png", "OuranosCockpit", artRegistry);
            RegisterSprite("Parts", "ouranos_generator.png", "OuranosGenerator", artRegistry);
            RegisterSprite("Parts", "ouranos_missile.png", "OuranosMissile", artRegistry);
            RegisterSprite("Parts", "ouranos_chassis.png", "OuranosChassis", artRegistry);
            RegisterSprite("Parts", "ouranos_cannon_v2_1.png", "OuranosCannonV2_1", artRegistry);
            RegisterSprite("Parts", "ouranos_cannon_v2_2.png", "OuranosCannonV2_2", artRegistry);
            RegisterSprite("Parts", "ouranos_cannon_v2_3.png", "OuranosCannonV2_3", artRegistry);

            RegisterSprite("Artifacts", "ouranos_cannon_artifact.png", "OuranosArtifactCannon", artRegistry);
            RegisterSprite("Artifacts", "ouranos_cannon_artifact_off.png", "OuranosArtifactCannonOff", artRegistry);
            RegisterSprite("Artifacts", "ouranos_cannon_artifact_v2.png", "OuranosArtifactCannonV2", artRegistry);
            RegisterSprite("Artifacts", "volt_converter_artifact.png", "VoltArtifactConverter", artRegistry);
            RegisterSprite("Artifacts", "artifact_card_battery.png", "ArtifactCardBattery", artRegistry);
            RegisterSprite("Artifacts", "artifact_lightning_drive.png", "ArtifactLightningDrive", artRegistry);
            RegisterSprite("Artifacts", "artifact_stable_dispersion.png", "ArtifactDispersion", artRegistry);

            RegisterSprite("FX", "RailgunCharge.png", "RailgunCharge", artRegistry);
        }

        public void LoadManifest(ICharacterRegistry registry)
        {
            Console.WriteLine("Defining character");
            Console.WriteLine(VoltDeck.GlobalName);
            Console.WriteLine("ay ?");
            VoltCharacter = new ExternalCharacter("APurpleApple.VoltMod.Char.Volt", VoltDeck, extSprites["VoltPanel"], new Type[] {typeof(CardVoltSpark), typeof(CardVoltRelay)}, new Type[0], animations["VoltNeutral"], animations["VoltMini"]);
            VoltCharacter.AddNameLocalisation("Volt");
            VoltCharacter.AddDescLocalisation("<c=ffe93b>VOLT</c>\nA reckless bounty hunter. His cards are focused on dealing <c=keyword>high bursts of damage</c> using <c=ffe93b>ELECTRIC CHARGE</c>, while <c=keyword>evading</c> enemy attacks.");
            Console.WriteLine("yup");
            registry.RegisterCharacter(VoltCharacter);
            Console.WriteLine("heyo ?");
        }

        public void LoadManifest(IDeckRegistry registry)
        {
            Console.WriteLine("Defining Decks");
            VoltDeck = new ExternalDeck("APurpleApple.VoltMod.Deck.Volt", System.Drawing.Color.Yellow, System.Drawing.Color.Black, extSprites["VoltCard"], extSprites["VoltBorder"], null);
            registry.RegisterDeck(VoltDeck);
            Console.WriteLine(VoltDeck.GlobalName);
        }

        public void LoadManifest(IAnimationRegistry registry)
        {
            RegisterAnimation("VoltNeutral", "neutral", VoltDeck, new ExternalSprite[] { extSprites["VoltPortrait"] }, registry);
            RegisterAnimation("VoltMini", "mini", VoltDeck, new ExternalSprite[] { extSprites["VoltMini"] }, registry);
        }

        public void LoadManifest(ICardRegistry registry)
        {
            RegisterCard("Spark", typeof(CardVoltSpark), extSprites["VoltCard"], VoltDeck, "Spark", registry);
            RegisterCard("Relay", typeof(CardVoltRelay), extSprites["VoltCard"], VoltDeck, "Energy Relay", registry);
            RegisterCard("Ramm", typeof(CardVoltRamm), extSprites["VoltCard"], VoltDeck, "Ramming Speed", registry);
            RegisterCard("Discharge", typeof(CardVoltDischarge), extSprites["VoltCard"], VoltDeck, "Discharge", registry);
            RegisterCard("Reload", typeof(CardVoltReload), extSprites["VoltCardReload"], VoltDeck, "Reload", registry);
            RegisterCard("Entanglement", typeof(CardVoltEntanglement), extSprites["VoltCard"], VoltDeck, "Power Link", registry);
            RegisterCard("Dash", typeof(CardVoltDash), extSprites["VoltCard"], VoltDeck, "Dash", registry);
            RegisterCard("BlastThrough", typeof(CardVoltBlastThrough), extSprites["VoltCard"], VoltDeck, "Blast Through", registry);
            RegisterCard("Roadkill", typeof(CardVoltRoadkill), extSprites["VoltCard"], VoltDeck, "Roadkill", registry);
            RegisterCard("SharpAim", typeof(CardVoltSharpAim), extSprites["VoltCard"], VoltDeck, "Sharp Aim", registry);
            RegisterCard("BurstThrusters", typeof(CardVoltBurstThrusters), extSprites["VoltCard"], VoltDeck, "Burst Thrusters", registry);
            RegisterCard("Ready", typeof(CardVoltReady), extSprites["VoltCard"], VoltDeck, "Ready?", registry);
            RegisterCard("Aim", typeof(CardVoltAim), extSprites["VoltCard"], VoltDeck, "Aim..", registry);
            RegisterCard("Fire", typeof(CardVoltFire), extSprites["VoltCard"], VoltDeck, "Fire!!!", registry);

            RegisterCard("BasicCharge", typeof(CardBasicCharge), extSprites["VoltCard"], ExternalDeck.GetRaw((int)Deck.colorless), "Basic Charge", registry);
        }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {
            RegisterStatus("ElectricCharge", true, System.Drawing.Color.Yellow, System.Drawing.Color.Yellow, extSprites["ElectricCharge"], false,
                "ELECTRIC CHARGE", "Add {0} damage to your next attack.\n<c=downside>Decreases by 1 when hit.</c>", statusRegistry);

            RegisterStatus("RailgunBeam", true, System.Drawing.Color.Yellow, System.Drawing.Color.Yellow, extSprites["RailgunBeam"], false,
                "RAILGUN CHARGE", "Attacks have been loaded into the railgun, and will be release at turn end for {0} damages", statusRegistry);

            RegisterStatus("PierceCharge", true, System.Drawing.Color.Red, System.Drawing.Color.Red, extSprites["PierceCharge"], false,
                "PIERCE CHARGE", "Your next attack will ignore all shield and armor.", statusRegistry);

            RegisterStatus("BeamCharge", true, System.Drawing.Color.Red, System.Drawing.Color.Red, extSprites["BeamCharge"], false,
                "BEAM CHARGE", "Your next attack will pierce through midrow.", statusRegistry);

            RegisterStatus("CannonStall", false, System.Drawing.Color.LightYellow, System.Drawing.Color.LightYellow, extSprites["AttackStall"], false,
                "CANNON STALL", "The next {0} attacks will fail. <c=downside>Decreases by 1 every time it triggers. Goes away at end of turn.</c>", statusRegistry);
        }

        public void LoadManifest(IGlossaryRegisty registry)
        {
            RegisterGlossary("ElectricCharge", ExternalGlossary.GlossayType.cardtrait, extSprites["ElectricCharge"], "ELECTRIC CHARGE",
                "Add {0} damage to your next attack.\n<c=downside>Decreases by 1 when hit.</c>",
                "Add 1 damage to your next attack.\n<c=downside>Decreases by 1 when hit.</c>", registry);

            RegisterGlossary("Ramming", ExternalGlossary.GlossayType.action, extSprites["ActionHurtEnemy"], "RAMM",
                "Rapidly ramm into the enemy ship, destroying any midrow object in your path.",
                null, registry);

            RegisterGlossary("PutBack", ExternalGlossary.GlossayType.action, extSprites["ActionPlaceOnTop"], "RETURN",
                "Place a card on top of your deck.",
                null, registry);

            RegisterGlossary("PutBackHand", ExternalGlossary.GlossayType.action, extSprites["ActionPlaceHandOnTop"], "RETURN HAND",
                "Place your hand on top of your deck, from left to right.",
                null, registry);

            RegisterGlossary("DrawDiscard", ExternalGlossary.GlossayType.action, extSprites["ActionDrawFromDiscard"], "DRAW FROM DISCARD",
                "Draw {0} cards from the top of your discard pile.",
                "Draw cards from the top of your discard pile.", registry);

            RegisterGlossary("Redo", ExternalGlossary.GlossayType.action, extSprites["ActionRedo"], "RESTART",
                "Redo all of the actions on this card.", null, registry);

            RegisterGlossary("PlayTopCard", ExternalGlossary.GlossayType.action, extSprites["ActionPlayTopCard"], "PLAY TOP CARD",
                "Play the top card of your deck for free.", null, registry);

            RegisterGlossary("ElectricCost", ExternalGlossary.GlossayType.action, extSprites["ElectricChargeCost"], "ELECTRIC CHARGE COST",
                "Pay {0} <c=ffe93b>ELECTRIC CHARGE</c>. If you don't have enough, this action does not happen.", null, registry);

            RegisterGlossary("SelfDamage", ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Spr.icons_hurtBlockable), "IMPACT DAMAGE",
                "Take {0} damage.", null, registry);

            RegisterGlossary("BeamAttack", ExternalGlossary.GlossayType.action, extSprites["ActionBeam"], "BEAM",
                "An attack that pierces through midrow.", null, registry);
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            RegisterArtifact("OuranosStart", typeof(ArtifactOuranosCannon), extSprites["OuranosArtifactCannon"], "OURANOS RAILGUN",
                "At the end of your turn, gain 1 <c=ffe93b>ELECTRIC CHARGE</c>.\n<c=downside>Each turn, any attack card played after the first one will do 0 damage.</c>", registry);

            RegisterArtifact("OuranosStartV2", typeof(ArtifactOuranosCannonV2), extSprites["OuranosArtifactCannonV2"], "OURANOS RAILGUN V2",
                "Replaces <c=artifact>OURANOS RAILGUN</c>.\nAt the end of your turn, gain 1 <c=ffe93b>ELECTRIC CHARGE</c>. Your attacks are stored in your cannon and are fired all at once on turn end.", registry);

            RegisterArtifact("Converter", typeof(ArtifactVoltConverter), extSprites["VoltArtifactConverter"], "ENERGY COMPRESSOR",
                "At the end of your turn, gain <c=ffe93b>ELECTRIC CHARGE</c> equal to your unspent <c=status>ENERGY</c>", registry);

            RegisterArtifact("CardBattery", typeof(ArtifactCardBattery), extSprites["ArtifactCardBattery"], "CARD BATTERY",
                "Gain 1 <c=ffe93b>ELECTRIC CHARGE</c> every time you play a non-attack card.\n<c=downside>All your cards deal 2 less damage, and lose all</c> <c=ffe93b>ELECTRIC CHARGE</c> <c=downside>at turn end</c>.", registry);

            RegisterArtifact("LightningDrive", typeof(ArtifactLightningDrive), extSprites["ArtifactLightningDrive"], "LIGHTNING DRIVE",
                "Whever you gain or lose <c=ffe93b>ELECTRIC CHARGE</c>, gain or lose that much <c=status>HERMES BOOTS</c>.", registry);

            RegisterArtifact("Dispersion", typeof(ArtifactStableDispersion), extSprites["ArtifactDispersion"], "STABLE DISPERSION",
                "Every time you lose <c=ffe93b>ELECTRIC CHARGE</c>, gain 1 <c=status>TEMP SHIELD</c>", registry);
        }

        public void LoadManifest(IStoryRegistry storyRegistry)
        {

        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            OuranosShip = new ExternalShip("APurpleApple.VoltMod.Ship.Ouranos",
                    new Ship()
                    {
                        baseDraw = 5,
                        baseEnergy = 3,
                        heatTrigger = 3,
                        heatMin = 0,
                        hull = 7,
                        hullMax = 7,
                        shieldMaxBase = 4
                    },
                    new ExternalPart[]
                    {
                        parts["OuranosMissile"],
                        parts["OuranosGenerator"],
                        parts["OuranosCannon"],
                        parts["OuranosCockpit"]
                    },
                     extSprites["OuranosChassis"],
                    null
                ); ;
            Console.WriteLine("REGIUSTER SHIP");
            shipRegistry.RegisterShip(OuranosShip);
        }
        public void LoadManifest(IStartershipRegistry registry)
        {
            var starter = new ExternalStarterShip("APurpleApple.VoltMod.StarterShip.Ouranos",
                        OuranosShip.GlobalName,
                        nativeStartingCards: new Type[]
                        {
                            typeof(BasicShieldColorless),
                            typeof(DodgeColorless)
                        },
                        startingCards: new ExternalCard[] {
                            cards["BasicCharge"]
                        },
                        nativeStartingArtifacts: new Type[]
                        {
                            typeof(ShieldPrep)
                        },
                        startingArtifacts: new ExternalArtifact[]
                        {
                            artifacts["OuranosStart"]
                        },
                        exclusiveArtifacts: new ExternalArtifact[]
                        {
                            artifacts["OuranosStartV2"],
                            artifacts["Dispersion"],
                            artifacts["CardBattery"],
                            artifacts["Converter"],
                            artifacts["LightningDrive"]
                        }
                        );
            starter.AddLocalisation("Ouranos", "A nimble warship featuring a powerfull, slow shooting railgun.");
            Console.WriteLine("REGIUSTER STARTER SHIP");
            registry.RegisterStartership(starter);
        }
        public void LoadManifest(IShipPartRegistry registry)
        {
            RegisterPart("OuranosCannon", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.cannon,
                offset = new Vec(-2, 0)
            },
            extSprites["OuranosCannon"],
            extSprites["OuranosCannonOff"],
            registry);

            RegisterPart("OuranosCannonV2_1", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.cannon,
                offset = new Vec(-2, 0)
            },
            extSprites["OuranosCannonV2_1"],
            extSprites["OuranosCannonOff"],
            registry);

            RegisterPart("OuranosCannonV2_2", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.cannon,
                offset = new Vec(-2, 0)
            },
            extSprites["OuranosCannonV2_2"],
            extSprites["OuranosCannonOff"],
            registry);

            RegisterPart("OuranosCannonV2_3", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.cannon,
                offset = new Vec(-2, 0)
            },
            extSprites["OuranosCannonV2_3"],
            extSprites["OuranosCannonOff"],
            registry);

            RegisterPart("OuranosCannonOff", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.cannon,
                offset = new Vec(-2, 0)
            },
            extSprites["OuranosCannonOff"],
            extSprites["OuranosCannonOff"],
            registry);

            RegisterPart("OuranosCockpit", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.cockpit,
            },
            extSprites["OuranosCockpit"],
            null, registry);

            RegisterPart("OuranosMissile", new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = PType.missiles,
            },
            extSprites["OuranosMissile"],
            null, registry);

            RegisterPart("OuranosGenerator", new Part()
            {
                active = true,
                damageModifier = PDamMod.weak,
                type = PType.comms,
            },
            extSprites["OuranosGenerator"],
            null, registry);
        }
    }
}
