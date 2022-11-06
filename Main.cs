using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Main.Scenes;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Powers;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Assets.Scripts.Unity.UI_New.Upgrade;
using Assets.Scripts.Utils;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using MelonLoader;

using UnhollowerBaseLib;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using System.Net;
using Assets.Scripts.Unity.UI_New.Popups;
using TMPro;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.TowerFilters;
using Assets.Scripts.Unity.UI_New.Main.MonkeySelect;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;
using MinerMonkey;
using MinerMonkey.Displays.Projectiles;
using BTD_Mod_Helper.Api.Display;

namespace MinerMonkey
{

    class Main : BloonsMod
    {
        //https://github.com/gurrenm3/BloonsTD6-Mod-Helper/releases

        public class MinerMonkey : ModTower
        {
            public override string Name => "MinerMonkey";
            public override string DisplayName => "Miner Monkey";
            public override string Description => "The crazy scientist Pyromanic developed the Miner Monkey to perfection.";
            public override string BaseTower => "BombShooter";
            public override int Cost => 900;
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 5;
            public override ParagonMode ParagonMode => ParagonMode.Base555;
            public override string TowerSet => "Primary";
            public override int GetTowerIndex(System.Collections.Generic.List<TowerDetailsModel> towerSet)
            {
                return towerSet.First(model => model.towerId == TowerType.DartMonkey).towerIndex + 1;
            }
            public override bool IsValidCrosspath(int[] tiers) =>
   HasMod("UltimateCrosspathing") ? true : base.IsValidCrosspath(tiers);
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                //balance stuff
                //towerModel.display = "06bf915dea753ad43b772045caf1d906";
                towerModel.display = new PrefabReference() { guidRef = "06bf915dea753ad43b772045caf1d906" };
                //towerModel.GetBehavior<DisplayModel>().display = "06bf915dea753ad43b772045caf1d906";
                towerModel.GetBehavior<DisplayModel>().display = new PrefabReference() { guidRef = "06bf915dea753ad43b772045caf1d906" };
                var attackModel = towerModel.GetBehavior<AttackModel>();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().CapDamage(9999);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().maxDamage = 9999;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.maxPierce = 99999;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.CapPierce(99999);
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 99;
                //attackModel.weapons[0].projectile.display = "62e990209b10d374d89f70c6f578def0";
                attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = "62e990209b10d374d89f70c6f578def0" };

                //pierce and damage
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 25;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().damage = 2;

                //change radius to 75% of 100 mortar
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius = 28 * 0.75f;



                //how many seconds until it shoots
                attackModel.weapons[0].Rate = 2.5f;
            }
            public override string Icon => "steve";
            public override string Portrait => "steve";
        }
        public class Money : ModUpgrade<MinerMonkey>
        {
            public override string Name => "Money";
            public override string DisplayName => "Poor";
            public override string Description => "Uneducated monkey";
            public override int Cost => 3000;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 10;
                MinerCash.maximum += 10;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);

                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<Penny100>(); // Make the projectiles look like Pickaxe
            }
            public override string Icon => "Penny100";
            public override string Portrait => "Penny100";
        }
        public class Mon : ModUpgrade<MinerMonkey>
        {
            public override string Name => "Mon";
            public override string DisplayName => "Going To College";
            public override string Description => "Has luck on his side";
            public override int Cost => 75000;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 100;
                MinerCash.maximum += 100;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);

                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<MinerPickDisplay>(); // Make the projectiles look like Pickaxe
            }
            public override string Icon => "college";
            public override string Portrait => "college";
        }
        public class KRKMONEY : ModUpgrade<MinerMonkey>
        {
            public override string Name => "KRKMONEY";
            public override string DisplayName => "Got Sponsored";
            public override string Description => "Got sponsored by BTD6";
            public override int Cost => 1000000;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 500;
                MinerCash.maximum += 500;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);
            }
            public override string Icon => "ninja";
            public override string Portrait => "ninja";
        }
        public class bigearner : ModUpgrade<MinerMonkey>
        {
            public override string Name => "bigearner";
            public override string DisplayName => "Got a Job";
            public override string Description => "Became an accountant";
            public override int Cost => 2000000;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 1000;
                MinerCash.maximum += 1000;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);
            }
            public override string Icon => "job";
            public override string Portrait => "job";
        }
        public class Hello : ModUpgrade<MinerMonkey>
        {
            public override string Name => "Hello";
            public override string DisplayName => "Mr.Beast mode";
            public override string Description => "Better than Elon Musk";
            public override int Cost => 20000000;
            public override int Path => TOP;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 10000;
                MinerCash.maximum += 10000;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);
            }
            public override string Icon => "what";
            public override string Portrait => "what";
        }
        public class woodenpickaxe : ModUpgrade<MinerMonkey>
        {
            public override string Name => "woodenpickaxe";
            public override string DisplayName => "Wooden Pickaxe";
            public override string Description => "Better than a stick";
            public override int Cost => 1500;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<wooden>(); // Make the projectiles look like Pickaxe
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            }
            public override string Icon => "woodenpickaxe";
            public override string Portrait => "woodenpickaxe";

        }
        public class Stonepickaxe : ModUpgrade<MinerMonkey>
        {
            public override string Name => "Stonepickaxe";
            public override string DisplayName => "Iron Pickaxe";
            public override string Description => "Hard work pays off";
            public override int Cost => 15000;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var fire = Game.instance.model.GetTowerFromId("MortarMonkey-002").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<AddBehaviorToBloonModel>();
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<iron>(); // Make the projectiles look like Pickaxe
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(fire);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, -1 };
                attackModel.weapons[0].Rate = 1;
            }
            public override string Icon => "ironpickaxe";
            public override string Portrait => "ironpickaxe";
        }
        public class IronPickaxe : ModUpgrade<MinerMonkey>
        {
            public override string Name => "IronPickaxe";
            public override string DisplayName => "Netherite Pickaxe";
            public override string Description => "Harder than diamonds";
            public override int Cost => 30000;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();


                var normal = Game.instance.model.GetTowerFromId("BombShooter-050").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>();
                normal.setDamage(3);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.RemoveBehavior<AttackModel>();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(normal);

                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 10, false, false) { tags = new string[] { "Fortified" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 10, false, false) { tags = new string[] { "Ceramic" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moab", 1, 10, false, false) { tags = new string[] { "Moab" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Bfb", 1, 10, false, false) { tags = new string[] { "Bfb" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Zomg", 1, 10, false, false) { tags = new string[] { "Zomg" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ddt", 1, 10, false, false) { tags = new string[] { "Ddt" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Bad", 1, 10, false, false) { tags = new string[] { "Bad" }, collisionPass = 0 });
                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<netherite>(); // Make the projectiles look like Pickaxe
                attackModel.weapons[0].Rate = 0.7f;
            }
            public override string Icon => "netheritepickaxe";
            public override string Portrait => "netheritepickaxe";
        }
        public class chaching : ModUpgrade<MinerMonkey>
        {
            public override string Name => "chaching";
            public override string DisplayName => "Efficiency 5";
            public override string Description => "Best pickaxe.....or is it?";
            public override int Cost => 70000;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                attackModel.weapons[0].projectile.pierce += 3;
                var bouncy = Game.instance.model.GetTowerFromId("SniperMonkey-030").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<RetargetOnContactModel>();
                bouncy.distance = 100;
                attackModel.weapons[0].projectile.AddBehavior(bouncy);
                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<efficiency5>(); // Make the projectiles look like Pickaxe
            }
            public override string Icon => "efficiency5";
            public override string Portrait => "efficiency5";
        }
        public class npp : ModUpgrade<MinerMonkey>
        {
            public override string Name => "npp";
            public override string DisplayName => "Admin Hammer";
            public override string Description => "YOU SHOULDN'T HAVE THAT!!!";
            public override int Cost => 500000;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                //balance stuff
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Fortified", 1, 999999, false, false) { tags = new string[] { "Fortified" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ceramic", 1, 999999, false, false) { tags = new string[] { "Ceramic" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Moab", 1, 999999, false, false) { tags = new string[] { "Moab" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Bfb", 1, 999999, false, false) { tags = new string[] { "Bfb" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Zomg", 1, 999999, false, false) { tags = new string[] { "Zomg" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Ddt", 1, 999999, false, false) { tags = new string[] { "Ddt" }, collisionPass = 0 });
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(new DamageModifierForTagModel("aaa", "Bad", 1, 999999, false, false) { tags = new string[] { "Bad" }, collisionPass = 0 });

                var ability = Game.instance.model.GetTowerFromId("AdmiralBrickell 3").Duplicate<TowerModel>().GetBehavior<AbilityModel>();
                ability.GetBehavior<ActivateRateSupportZoneModel>().filters[0] = new FilterInBaseTowerIdModel("lol", new string[] { "DartMonkey", "MortarMonkey" });
                towerModel.AddBehavior(ability);
                var projectile = attackModel.weapons[0].projectile;
                projectile.ApplyDisplay<Banhammer>(); // Make the projectiles look like Pickaxe
                attackModel.weapons[0].Rate = 0.5f;
            }
            public override string Icon => "Banhammer";
            public override string Portrait => "Banhammer";
        }
        public class BitSoil : ModUpgrade<MinerMonkey>
        {
            public override string Name => "Bitsoil";
            public override string DisplayName => "Bitsoil";
            public override string Description => "EW";
            public override int Cost => 1;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 10;
                MinerCash.maximum += 10;
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                attackModel.weapons[0].Rate *= 0.8f;
            }
            public override string Icon => "bitsoil";
            public override string Portrait => "bitsoil";
        }
        public class CrashtheMarket : ModUpgrade<MinerMonkey>
        {
            public override string Name => "CrashtheMarket";
            public override string DisplayName => "Crash the Market";
            public override string Description => "Hard work pays off";
            public override int Cost => 100000;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 3000;
                MinerCash.maximum += 3000;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);
                towerModel.ignoreBlockers = true;
                attackModel.weapons[0].projectile.ignoreBlockers = true;
                attackModel.weapons[0].projectile.canCollisionBeBlockedByMapLos = false;
                attackModel.attackThroughWalls = true;
            }
            public override string Icon => "Crash";
            public override string Portrait => "Crash";
        }
        public class Diamondcoin : ModUpgrade<MinerMonkey>
        {
            public override string Name => "Diamondcoin";
            public override string DisplayName => "Diamond coin";
            public override string Description => "Worth more than your whole house";
            public override int Cost => 100000;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 9182;
                MinerCash.maximum += 9182;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);
                var lasershock = Game.instance.model.GetTowerFromId("DartlingGunner-200").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>();
                lasershock.lifespan = 4;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(lasershock);
                attackModel.weapons[0].projectile.AddBehavior(lasershock);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.collisionPasses = new int[] { 0, 1 };
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().damage += 3;
            }
            public override string Icon => "diamond";
            public override string Portrait => "diamond";
        }
        public class DogeCoin : ModUpgrade<MinerMonkey>
        {
            public override string Name => "DogeCoin";
            public override string DisplayName => "Doge Coin";
            public override string Description => "Best Coin.....or is it?";
            public override int Cost => 1000000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 100000;
                MinerCash.maximum += 100000;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);
            }
            public override string Icon => "dogecoin";
            public override string Portrait => "dogecoin";
        }
        public class alien : ModUpgrade<MinerMonkey>
        {
            public override string Name => "AlienCoin";
            public override string DisplayName => "AlienCoin";
            public override string Description => "How did you buy this?";
            public override int Cost => 100000000;
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce += 100;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<DamageModel>().damage += 600;
                var genericDamage = Game.instance.model.GetTowerFromId("Gwendolin").Duplicate<TowerModel>().GetBehavior<AttackModel>().weapons[0].projectile.GetBehavior<DamageModel>();
                attackModel.weapons[0].projectile.AddBehavior(genericDamage);
                attackModel.weapons[0].projectile.GetBehavior<DamageModel>().damage += 500;
                attackModel.weapons[0].projectile.pierce += 50;
                attackModel.weapons[0].projectile.display = new PrefabReference() { guidRef = "c8c2a5ade2633394eafdf29a4013e919" };
                var MinerCash = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var MinerText = Game.instance.model.GetTowerFromId("BananaFarm-004").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                MinerCash.minimum += 100000;
                MinerCash.maximum += 100000;
                attackModel.weapons[0].projectile.AddBehavior(MinerCash);
                attackModel.weapons[0].projectile.AddBehavior(MinerText);

            }
            public override string Icon => "alien";
            public override string Portrait => "alien";
        }
        public class ParagonDisplay : ModTowerDisplay<MinerMonkey>
        {
            public override string BaseDisplay => GetDisplay(TowerType.DartMonkey);

            public override bool UseForTower(int[] tiers) => IsParagon(tiers);


            public class Cosmicinvestor : ModParagonUpgrade<MinerMonkey>
            {
                public override int Cost => 1000000000;
                public override string Description => " you just beat capitalism (see what i did with the name?)";
                public override string DisplayName => "Kosmic Investor";


                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
                    var projectile = attackModel.weapons[0].projectile;
                    var banhamer = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().Duplicate();
                    var banhamerWeapon = banhamer.weapons[0];
                    var banhamerProj = banhamer.weapons[0].projectile;
                    banhamer.RemoveBehavior<TargetTrackModel>();
                    banhamer.AddBehavior(new TargetFirstModel("TargetFirstModel_", true, false));
                    banhamer.AddBehavior(new TargetStrongModel("TargetStrongModel_", true, false));
                    banhamer.AddBehavior(new TargetCloseModel("TargetCloseModel_", true, false));
                    banhamer.AddBehavior(new TargetLastModel("TargetLastModel_", true, false));
                    banhamer.AddBehavior(new RotateToTargetModel("RotateToTargetModel_", true, true, true, 1, true, true));
                    banhamer.attackThroughWalls = true;


                    banhamerWeapon.fireWithoutTarget = false;
                    banhamerWeapon.rate = 0f;
                    banhamerProj.RemoveBehavior<DamageModel>();
                    banhamerProj.RemoveBehavior<SetSpriteFromPierceModel>();
                    banhamerProj.AddBehavior(new DamageModel("DamageModel_", 999999, 999999, true, true, true, BloonProperties.None, BloonProperties.None));
                    banhamerProj.AddBehavior(new WindModel("WindModel_", 0, 200, 999999, true, null, 0));
                    banhamerProj.GetBehavior<ArriveAtTargetModel>().timeToTake = 0f;
                    banhamerProj.pierce = 9999999;
                    banhamerProj.ApplyDisplay<Banhammer>();

                    var BananaFarmAttackModel = Game.instance.model.GetTowerFromId("BananaFarm").GetAttackModel().Duplicate();
                    BananaFarmAttackModel.name = "BananaFarm_";
                    BananaFarmAttackModel.weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count = 9999999;
                    BananaFarmAttackModel.weapons[0].projectile.GetBehavior<CashModel>().maximum = 9999999f;
                    BananaFarmAttackModel.weapons[0].projectile.GetBehavior<CashModel>().minimum = 9999999f;
                    towerModel.range = 100000;
                    banhamer.range = towerModel.range;
                    towerModel.isGlobalRange = true;
                    towerModel.AddBehavior(BananaFarmAttackModel);

                    towerModel.AddBehavior(banhamer);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<CollectCashZoneModel>().Duplicate());
                    towerModel.AddBehavior(new MonkeyCityIncomeSupportModel("_MonkeyCityIncomeSupport", true, 9999999f, null, "MonkeyCityBuff", "BuffIconVillagexx4"));
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                }
                public override string Icon => "investor";
                public override string Portrait => "investor";
            }
        }
    }





    [HarmonyPatch(typeof(InGame), "Update")]
            public class Update_Patch
            {
                [HarmonyPostfix]
                public static void Postfix()
                {
                    if (!(InGame.instance != null && InGame.instance.bridge != null)) return;
                    try
                    {
                        foreach (var tts in InGame.Bridge.GetAllTowers())
                        {

                            if (!tts.namedMonkeyKey.ToLower().Contains("MinerMonkey")) continue;
                            if (tts?.tower?.Node?.graphic?.transform != null)
                            {
                                tts.tower.Node.graphic.transform.localScale = new UnityEngine.Vector3(1.3f, 1.3f, 1.3f);

                            }

                        }
                    }
                    catch
                    {

                    }


                }
            }


        }
