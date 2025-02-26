﻿using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.Items;
using CalamityMod.NPCs;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Systems;
using CalamityMod.UI;
using CalamityMod.UI.CalamitasEnchants;
using CalamityMod.UI.DraedonSummoning;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace CalamityMod
{
    public class ModCalls
    {
        #region Boss / Event Downed
        /// <summary>
        /// Returns whether the Calamity boss or event corresponding to the given string has been defeated.
        /// </summary>
        /// <param name="boss">The boss or event name to check. Many aliases are accepted.</param>
        /// <returns>Whether the boss or event has been defeated.</returns>
        public static bool GetBossDowned(string boss)
        {
            switch (boss.ToLower())
            {
                default:
                    return false;

                // Because this value will always be true if Acid Rain at any point in the game is beaten.
                case "acid rain":
                case "acidrain":

                case "acid rain 1":
                case "acidrain 1":
                case "acidrain1":
                case "acid rain eoc":
                case "acidrain eoc":
                case "acidraineoc":
                    return DownedBossSystem.downedEoCAcidRain;

                case "desertscourge":
                case "desert scourge":
                    return DownedBossSystem.downedDesertScourge;

                case "clam":
                case "giantclam":
                case "giant clam":
                    return DownedBossSystem.downedCLAM;

                case "crabulon":
                    return DownedBossSystem.downedCrabulon;

                case "hivemind":
                case "hive mind":
                case "thehivemind":
                case "the hive mind":
                    return DownedBossSystem.downedHiveMind;

                case "perforator":
                case "perforators":
                case "theperforators":
                case "the perforators":
                case "perforatorhive":
                case "perforator hive":
                case "theperforatorhive":
                case "the perforator hive":
                    return DownedBossSystem.downedPerforator;

                case "slimegod":
                case "slime god":
                case "theslimegod":
                case "the slime god":
                    return DownedBossSystem.downedSlimeGod;

                case "hmclam":
                case "clamhm":
                case "hm clam":
                case "clam hm":
                case "hmgiantclam":
                case "giantclamhm":
                case "hm giant clam":
                case "giant clam hm":
                case "hardmodeclam":
                case "hardmode clam":
                case "hardmodegiantclam":
                case "hardmode giant clam":
                    return DownedBossSystem.downedCLAMHardMode;

                case "cryogen":
                    return DownedBossSystem.downedCryogen;

                case "acid rain 2":
                case "acidrain 2":
                case "acidrain2":
                case "acid rain scourge":
                case "acid rain aquatic scourge":
                case "acid rain aquaticscourge":
                case "acidrain scourge":
                case "acidrain aquatic scourge":
                case "acidrain aquaticscourge":
                case "acidrainscourge":
                case "acidrainaquaticscourge":
                    return DownedBossSystem.downedAquaticScourgeAcidRain;

                case "aquaticscourge":
                case "aquatic scourge":
                    return DownedBossSystem.downedAquaticScourge;

                case "cragmaw":
                case "cragmawmire":
                case "cragmaw mire":
                case "mire":
                    return DownedBossSystem.downedCragmawMire;

                case "brimstoneelemental":
                case "brimstone elemental":
                    return DownedBossSystem.downedBrimstoneElemental;

                case "clone":
                case "calamitasclone":
                case "calamitas clone":
                case "clonelamitas":
                case "calamitasdoppelganger":
                case "calamitas doppelganger":
                    return DownedBossSystem.downedCalamitasClone;

                case "gss":
                case "greatsandshark":
                case "great sand shark":
                    return DownedBossSystem.downedGSS;

                // Don't remove the old references to "Siren" here to avoid breaking other mods
                case "sirenleviathan":
                case "siren leviathan":
                case "sirenandleviathan":
                case "siren and leviathan":
                case "the siren and the leviathan":
                case "siren":
                case "thesiren":
                case "the siren":
                case "anahita":
                case "leviathan":
                case "theleviathan":
                case "the leviathan":
                case "anahitaleviathan":
                case "anahita leviathan":
                case "anahitaandleviathan":
                case "anahita and leviathan":
                case "anahita and the leviathan":
                    return DownedBossSystem.downedLeviathan;

                case "aureus":
                case "astrumaureus":
                case "astrum aureus":
                    return DownedBossSystem.downedAstrumAureus;

                case "pbg":
                case "plaguebringer":
                case "plaguebringergoliath":
                case "plaguebringer goliath":
                case "theplaguebringergoliath":
                case "the plaguebringer goliath":
                    return DownedBossSystem.downedPlaguebringer;

                case "scavenger": // backwards compatibility
                case "ravager":
                    return DownedBossSystem.downedRavager;

                case "stargod": // backwards compatibility
                case "star god": // backwards compatibility
                case "astrumdeus":
                case "astrum deus":
                    return DownedBossSystem.downedAstrumDeus;

                case "guardians":
                case "donuts":
                case "profanedguardians":
                case "profaned guardians":
                case "theprofanedguardians":
                case "the profaned guardians":
                    return DownedBossSystem.downedGuardians;

                case "dragonfolly":
                case "the dragonfolly":
                case "bumblebirb":
                case "bumblefuck":
                    return DownedBossSystem.downedDragonfolly;

                case "providence":
                case "providencetheprofanedgoddess":
                case "providence the profaned goddess":
                case "providence, the profaned goddess":
                    return DownedBossSystem.downedProvidence;

                case "polterghast":
                case "necroghast":
                case "necroplasm":
                    return DownedBossSystem.downedPolterghast;

                case "mauler":
                    return DownedBossSystem.downedMauler;

                case "nuclear terror":
                case "nuclearterror":
                    return DownedBossSystem.downedNuclearTerror;

                // Old Duke is also Acid Rain tier 3, so he gets those names too
                case "oldduke":
                case "old duke":
                case "theoldduke":
                case "the old duke":
                case "boomerduke":
                case "boomer duke":
                case "sulphurduke":
                case "sulphur duke":
                case "sulfurduke":
                case "sulfur duke":
                case "acid rain 3":
                case "acidrain 3":
                case "acidrain3":
                case "acid rain duke":
                case "acidrain duke":
                case "acidrainduke":
                    return DownedBossSystem.downedBoomerDuke;

                case "sentinel1": // backwards compatibility
                case "void":
                case "ceaselessvoid":
                case "ceaseless void":
                    return DownedBossSystem.downedCeaselessVoid;

                case "sentinel2": // backwards compatibility
                case "stormweaver":
                case "storm weaver":
                    return DownedBossSystem.downedStormWeaver;

                case "sentinel3": // backwards compatibility
                case "cosmicwraith":
                case "cosmic wraith":
                case "signus":
                case "signusenvoyofthedevourer":
                case "signus envoy of the devourer":
                case "signus, envoy of the devourer":
                    return DownedBossSystem.downedSignus;

                case "sentinelany": // backwards compatibility
                case "anysentinel":
                case "any sentinel":
                case "onesentinel":
                case "one sentinel":
                case "sentinel":
                    return DownedBossSystem.downedCeaselessVoid || DownedBossSystem.downedStormWeaver || DownedBossSystem.downedSignus;

                case "sentinelall": // backwards compatibility
                case "sentinels":
                case "allsentinel":
                case "allsentinels":
                case "all sentinels":
                    return DownedBossSystem.downedCeaselessVoid && DownedBossSystem.downedStormWeaver && DownedBossSystem.downedSignus;

                case "dog":
                case "devourerofgods":
                case "devourer of gods":
                case "thedevourerofgods":
                case "the devourer of gods":
                    return DownedBossSystem.downedDoG;

                case "yharon":
                case "jungledragonyharon":
                case "jungle dragon yharon":
                case "jungle dragon, yharon":
                case "yharondragonofrebirth":
                case "yharon, dragon of rebirth":
                case "yharon dragon of rebirth":
                case "yharonresplendentphoenix":
                case "yharon, resplendent phoenix":
                case "yharon resplendent phoenix":
                    return DownedBossSystem.downedYharon;

                case "draedon":
                case "exomechs":
                case "exo mechs":
                case "the exo mechs":
                    return DownedBossSystem.downedExoMechs;

                case "thanatos":
                    return DownedBossSystem.downedThanatos;

                case "ares":
                    return DownedBossSystem.downedAres;

                case "exotwins":
                case "exo twins":
                case "apollo":
                case "artemis":
                    return DownedBossSystem.downedArtemisAndApollo;

                case "calamitas":
                case "scal":
                case "supremecalamitas":
                case "supreme calamitas":
                case "supremewitchcalamitas":
                case "supreme witch calamitas":
                case "supreme witch, calamitas":
                    return DownedBossSystem.downedCalamitas;

                case "adulteidolonwyrm":
                case "adult eidolon wyrm":
                case "adultwyrm":
                case "adult wyrm":
                case "adulteidolon":
                case "adult eidolon":
                case "primordialwyrm":
                case "primordial wyrm":
                    return DownedBossSystem.downedPrimordialWyrm;

                case "bossrush":
                case "boss rush":
                case "terminus":
                    return DownedBossSystem.downedBossRush;
            }
        }
        #endregion

        #region Player in Zone / Area
        /// <summary>
        /// Returns whether the specified player is in the Calamity biome or area corresponding to the given string.
        /// </summary>
        /// <param name="p">The player whose locale is being questioned.</param>
        /// <param name="zone">The zone or area name to check. Many aliases are accepted.</param>
        /// <returns>Whether the player is currently in the zone.</returns>
        public static bool GetInZone(Player p, string zone)
        {
            CalamityPlayer mp = p.Calamity();
            switch (zone.ToLower())
            {
                default:
                    return false;

                case "calamity": // backwards compatibility
                case "calamitybiome":
                case "calamity biome":
                case "crag":
                case "crags":

                case "profanedcrag": // remove these four when the actual profaned biome is added
                case "profaned crag":
                case "profanedcrags":
                case "profaned crags":

                case "brimstone":
                case "brimstonecrag":
                case "brimstone crag":
                case "brimstonecrags":
                case "brimstone crags":
                    return mp.ZoneCalamity;

                case "astral":
                case "astralbiome":
                case "astral biome":
                case "astralinfection":
                case "astral infection":
                    return mp.ZoneAstral;

                case "sunkensea":
                case "sunken sea":
                case "thesunkensea":
                case "the sunken sea":
                    return mp.ZoneSunkenSea;

                case "sulfur":
                case "sulphur":
                case "sulfursea":
                case "sulfur sea":
                case "sulphursea":
                case "sulphur sea":
                case "sulfuroussea":
                case "sulfurous sea":
                case "sulphuroussea":
                case "sulphurous sea":
                    return mp.ZoneSulphur;

                case "abyss":
                case "theabyss":
                case "the abyss":
                case "anyabyss":
                case "any abyss":
                case "abyssany":
                case "any abyss layer":
                    return mp.ZoneAbyss;

                case "abyss1":
                case "abyss 1":
                case "abyss_1":
                case "layer1":
                case "layer 1":
                case "layer_1":
                case "abysslayer1":
                case "abyss layer 1":
                case "sulfur depths":
                case "sulfurdepths":
                case "sulphur depths":
                case "sulphurdepths":
                case "sulfurous depths":
                case "sulfurousdepths":
                case "sulphurous depths":
                case "sulphurousdepths":
                case "sulfuric depths":
                case "sulfuricdepths":
                case "sulphuric depths":
                case "sulphuricdepths":
                    return mp.ZoneAbyssLayer1;

                case "abyss2":
                case "abyss 2":
                case "abyss_2":
                case "layer2":
                case "layer 2":
                case "layer_2":
                case "abysslayer2":
                case "abyss layer 2":
                case "murky water":
                case "murkywater":
                case "murky waters":
                case "murkywaters":
                    return mp.ZoneAbyssLayer2;

                case "abyss3":
                case "abyss 3":
                case "abyss_3":
                case "layer3":
                case "layer 3":
                case "layer_3":
                case "abysslayer3":
                case "abyss layer 3":
                case "thermalvents":
                case "thermal vents":
                case "vents":
                    return mp.ZoneAbyssLayer3;

                case "abyss4":
                case "abyss 4":
                case "abyss_4":
                case "layer4":
                case "layer 4":
                case "layer_4":
                case "abysslayer4":
                case "abyss layer 4":
                case "void":
                case "the void":
                case "thevoid":
                    return mp.ZoneAbyssLayer4;
            }
        }
        #endregion

        #region Difficulty Modes
        /// <summary>
        /// Returns whether the Calamity difficulty modifier corresponding to the given string is currently active.
        /// </summary>
        /// <param name="difficulty">The difficulty modifier to check for.</param>
        /// <returns>Whether the difficulty is currently active.</returns>
        public static bool GetDifficultyActive(string difficulty)
        {
            switch (difficulty.ToLower())
            {
                default:
                    return false;

                case "revengeance":
                case "rev":
                case "revengeancemode":
                case "revengeance mode":
                    return CalamityWorld.revenge;

                case "death":
                case "deathmode":
                case "death mode":
                    return CalamityWorld.death;

                case "br":
                case "bossrush":
                case "boss rush":
                case "bossrushactive":
                case "boss rush active":
                    return BossRushEvent.BossRushActive;

                case "armageddon":
                case "arma":
                case "instakill":
                case "instagib":
                case "armageddonmode":
                case "armageddon mode":
                    return CalamityWorld.armageddon;
            }
        }

        /// <summary>
        /// Either enables or disables the Calamity difficulty modifier corresponding to the given string.<br></br>
        /// Unlike the in-game mode changing items, this has no ancillary effects such as failing if a boss is alive or instantly killing players.
        /// </summary>
        /// <param name="difficulty">The difficulty modifier to edit.</param>
        /// <param name="enabled">Whether to enable or disable the difficulty.</param>
        /// <returns></returns>
        public static bool SetDifficultyActive(string difficulty, bool enabled)
        {
            switch (difficulty.ToLower())
            {
                default:
                    return false;

                case "revengeance":
                case "rev":
                case "revengeancemode":
                case "revengeance mode":
                    return CalamityWorld.revenge = enabled;

                case "death":
                case "deathmode":
                case "death mode":
                    return CalamityWorld.death = enabled;

                case "br":
                case "bossrush":
                case "boss rush":
                case "bossrushactive":
                case "boss rush active":
                    return BossRushEvent.BossRushActive = enabled;

                case "armageddon":
                case "arma":
                case "instakill":
                case "instagib":
                case "armageddonmode":
                case "armageddon mode":
                    return CalamityWorld.armageddon = enabled;
            }
        }

        public static void AddCustomDifficulty(DifficultyMode newMode)
        {
            //Add the new difficulty and recalculate
            DifficultyModeSystem.Difficulties.Add(newMode);
            DifficultyModeSystem.CalculateDifficultyData();
        }

        public static void AddWorldScreenDifficulty(string name, Func<AWorldListItem, bool> function, Color color, int index = -1)
        {
            // Construct the difficulty
            WorldSelectionDifficultySystem.WorldDifficulty difficulty = new WorldSelectionDifficultySystem.WorldDifficulty(name, function, color);

            // Add the new difficulty 
            // If no index was provided, just add it to the end of the list, otherwise insert it at the desired spot on the list
            if (index == -1)
            {
                WorldSelectionDifficultySystem.WorldDifficulties.Add(difficulty);
            }
            else
            {
                WorldSelectionDifficultySystem.WorldDifficulties.Insert(index, difficulty);
            }
        }
        #endregion

        #region Rogue Class
        /// <summary>
        /// Gets a player's current rogue projectile velocity multiplier.
        /// </summary>
        /// <param name="p">The player whose rogue velocity is being queried.</param>
        /// <returns>Current rogue projectile velocity multiplier. 1f is no bonus, 2f doubles projectile speed.</returns>
        public static float GetRogueVelocity(Player p) => p?.Calamity()?.rogueVelocity ?? 1f;

        /// <summary>
        /// Adds a flat amount of rogue velocity stat to a player. This amount can be negative.
        /// </summary>
        /// <param name="p">The player whose rogue velocity is being modified.</param>
        /// <param name="add">The amount of rogue velocity to add or subtract (if negative).</param>
        /// <returns>The player's new rogue velocity stat.</returns>
        public static float AddRogueVelocity(Player p, float add) => p is null ? 1f : (p.Calamity().rogueVelocity += add);

        public static float GetCurrentStealth(Player p) => p?.Calamity()?.rogueStealth ?? 0f;

        public static float GetMaxStealth(Player p) => p?.Calamity()?.rogueStealthMax ?? 0f;

        public static float AddMaxStealth(Player p, float add) => p is null ? 0f : (p.Calamity().rogueStealthMax += add);

        public static bool CanStealthStrike(Player p) => p?.Calamity()?.StealthStrikeAvailable() ?? false;

        public static void SetStealthProjectile(Projectile projectile, bool enabled)
        {
            if (projectile != null)
                projectile.Calamity().stealthStrike = enabled;
        }

        public static bool GetStealthProjectile(Projectile projectile) => projectile?.Calamity()?.stealthStrike ?? false;

        public static void ConsumeStealth(Player player) => player?.Calamity()?.ConsumeStealthByAttacking();
        #endregion

        #region Rippers
        public static float GetRage(Player p) => p?.Calamity()?.rage ?? 0;
        public static float GetAdrenaline(Player p) => p?.Calamity()?.adrenaline ?? 0;
        public static float GetRageMax(Player p) => p?.Calamity()?.rageMax ?? 0;
        public static float GetAdrenalineMax(Player p) => p?.Calamity()?.adrenalineMax ?? 0;
        #endregion

        #region Charge
        public static bool GetChargeable(Item i) => i?.Calamity()?.UsesCharge ?? false;
        public static void SetChargeable(Item i, bool chargeable)
        {
            if (i != null)
                i.Calamity().UsesCharge = chargeable;
        }
        public static float GetCharge(Item i) => i?.Calamity()?.Charge ?? 0;
        public static void SetCharge(Item i, float charge)
        {
            if (i != null)
                i.Calamity().Charge = charge;
        }
        public static float GetMaxCharge(Item i) => i?.Calamity()?.MaxCharge ?? 0;
        public static void SetMaxCharge(Item i, float chargeMax)
        {
            if (i != null)
                i.Calamity().MaxCharge = chargeMax;
        }
        public static float GetChargePerUse(Item i) => i?.Calamity()?.ChargePerUse ?? 0;
        public static void SetChargePerUse(Item i, float chargeUse)
        {
            if (i != null)
                i.Calamity().ChargePerUse = chargeUse;
        }
        public static float GetChargePerAltUse(Item i) => i?.Calamity()?.ChargePerAltUse ?? 0;
        public static void SetChargePerAltUse(Item i, float chargeAltUse)
        {
            if (i != null)
                i.Calamity().ChargePerAltUse = chargeAltUse;
        }
        #endregion

        #region Mouse Listening
        public static bool GetRightClickListener(Player p) => p?.Calamity()?.rightClickListener ?? false;
        public static bool GetMouseWorldListener(Player p) => p?.Calamity()?.mouseWorldListener ?? false;
        public static bool GetRightClick(Player p) => p?.Calamity()?.mouseRight ?? false;
        public static Vector2 GetMouseWorld(Player p) => p?.Calamity()?.mouseWorld ?? Vector2.Zero;
        public static void SetRightClickListener(Player p, bool active)
        {
            if (p != null)
                p.Calamity().rightClickListener = active;
        }
        public static void SetMouseWorldListener(Player p, bool active)
        {
            if (p != null)
                p.Calamity().mouseWorldListener = active;
        }
        #endregion

        #region Other Player Stats
        public static int GetLightStrength(Player p) => p?.GetCurrentAbyssLightLevel() ?? 0;

        public static void AddAbyssLightStrength(Player p, int add)
        {
            if (p != null)
                p.Calamity().externalAbyssLight += add;
        }

        public static void ToggleInfiniteFlight(Player p, bool enabled)
        {
            if (p != null)
                p.Calamity().infiniteFlight = enabled;
        }

        public static bool GetWearingRogueArmor(Player p) => p?.Calamity()?.wearingRogueArmor ?? false;

        public static void SetWearingRogueArmor(Player p, bool enabled)
        {
            if (p != null)
                p.Calamity().wearingRogueArmor = enabled;
        }

        public static bool GetWearingPostMLSummonerArmor(Player p) => p?.Calamity()?.WearingPostMLSummonerSet ?? false;

        public static void SetWearingPostMLSummonerArmor(Player p, bool enabled)
        {
            if (p != null)
                p.Calamity().WearingPostMLSummonerSet = enabled;
        }

        public static bool MakeColdImmune(Player p) => p is null ? false : (p.Calamity().externalColdImmunity = true);
        public static bool MakeHeatImmune(Player p) => p is null ? false : (p.Calamity().externalHeatImmunity = true);
        #endregion

        #region NPC Damage Reduction
        // Sets the damage reduction for an NPC type
        public static float SetDamageReduction(int npcID, float dr)
        {
            CalamityMod.DRValues.TryGetValue(npcID, out float oldDR);
            CalamityMod.DRValues.Remove(npcID);
            CalamityMod.DRValues.Add(npcID, dr);
            return oldDR;
        }
        // Sets a specific NPC's damage reduction
        public static void SetDamageReductionSpecific(NPC npc, float dr)
        {
            if (npc != null)
                npc.Calamity().DR = dr;
        }
        // Gets a specific NPC's current damage reduction
        public static float GetDamageReduction(NPC npc) => npc?.Calamity()?.DR ?? 0f;
        #endregion

        #region Defense Damage
        // Allow an NPC to deal defense damage
        public static void SetDefenseDamageNPC(NPC npc, bool enabled)
        {
            if (npc != null)
                npc.Calamity().canBreakPlayerDefense = enabled;
        }
        // Gets if an NPC can deal defense damage
        public static bool GetDefenseDamageNPC(NPC npc) => npc?.Calamity()?.canBreakPlayerDefense ?? false;
        // Allow a projectile to deal defense damage
        public static void SetDefenseDamageProjectile(Projectile projectile, bool enabled)
        {
            if (projectile != null)
                projectile.Calamity().DealsDefenseDamage = enabled;
        }
        // Gets if a projectile can deal defense damage
        public static bool GetDefenseDamageProjectile(Projectile projectile) => projectile?.Calamity()?.DealsDefenseDamage ?? false;
        #endregion

        #region Debuff Vulnerabilities
        public static void SetDebuffVulnerability(NPC npc, string debuffName, bool? enabled)
        {
            if (npc != null)
            {
                switch (debuffName.ToLower())
                {
                    case "cold":
                    case "ice":
                    case "frozen":
                    case "freezing":
                        npc.Calamity().VulnerableToCold = enabled;
                        break;

                    case "electricity":
                    case "electric":
                    case "lightning":
                    case "thunder":
                        npc.Calamity().VulnerableToElectricity = enabled;
                        break;

                    case "heat":
                    case "hot":
                    case "fire":
                    case "burning":
                        npc.Calamity().VulnerableToHeat = enabled;
                        break;

                    case "sickness":
                    case "sick":
                    case "poison":
                    case "poisoned":
                    case "venom":
                        npc.Calamity().VulnerableToSickness = enabled;
                        break;

                    case "water":
                    case "wet":
                    case "drown":
                    case "drowning":
                        npc.Calamity().VulnerableToWater = enabled;
                        break;
                }
            }
        }
        public static bool? GetDebuffVulnerability(NPC npc, string debuffName)
        {
            if (npc != null)
            {
                switch (debuffName.ToLower())
                {
                    default:
                        return false;

                    case "cold":
                    case "ice":
                    case "frozen":
                    case "freezing":
                        return npc?.Calamity()?.VulnerableToCold ?? null;

                    case "electricity":
                    case "electric":
                    case "lightning":
                    case "thunder":
                        return npc?.Calamity()?.VulnerableToElectricity ?? null;

                    case "heat":
                    case "hot":
                    case "fire":
                    case "burning":
                        return npc?.Calamity()?.VulnerableToHeat ?? null;

                    case "sickness":
                    case "sick":
                    case "poison":
                    case "poisoned":
                    case "venom":
                        return npc?.Calamity()?.VulnerableToSickness ?? null;

                    case "water":
                    case "wet":
                    case "drown":
                    case "drowning":
                        return npc?.Calamity()?.VulnerableToWater ?? null;
                }
            }
            return false;
        }
        #endregion

        #region Calamity AI

        public static float[] GetCalamityAI(NPC npc) => npc?.Calamity()?.newAI ?? new float[0];

        public static void SetCalamityAI(NPC npc, int aiSlot, float value)
        {
            if (npc != null)
            {
                npc.Calamity().newAI[aiSlot] = value;
            }
        }
        #endregion

        #region Boss Health Bars
        public static bool BossHealthBarVisible() => Main.LocalPlayer.Calamity().drawBossHPBar;

        public static bool SetBossHealthBarVisible(bool visible) => Main.LocalPlayer.Calamity().drawBossHPBar = visible;

        public static bool GetShouldCloseBossHealthBar(NPC npc) => npc?.Calamity()?.ShouldCloseHPBar ?? false;
        public static void SetShouldCloseBossHealthBar(NPC npc, bool enabled) => npc.Calamity().ShouldCloseHPBar = enabled;
        #endregion

        #region Dodge Disabling
        public static bool AreDodgesDisabled() => Main.LocalPlayer.Calamity().disableAllDodges;

        public static bool DisableAllDodges(bool disable) => Main.LocalPlayer.Calamity().disableAllDodges = disable;
        #endregion

        #region Can Fire Point Blank Shots
        /// <summary>
        /// Gets whether the given item can fire point blank shots.
        /// </summary>
        /// <param name="it">The item which is being checked.</param>
        /// <returns>Whether the item can fire point blank shots.</returns>
        public static bool CanFirePointBlank(Item it)
        {
            if (it is null || it.Calamity() is null)
                return false;
            CalamityGlobalItem cgi = it.Calamity();
            return cgi.canFirePointBlankShots;
        }

        /// <summary>
        /// Sets whether the given item can fire point blank shots.
        /// </summary>
        /// <param name="it">The item whose point blank capabilities is being toggled.</param>
        /// <param name="enabled">The value to apply.</param>
        /// <returns>Whether the item can fire point blank shots.</returns>
        public static bool SetFirePointBlank(Item it, bool enabled)
        {
            if (it is null || it.Calamity() is null)
                return false;
            CalamityGlobalItem cgi = it.Calamity();
            cgi.canFirePointBlankShots = enabled;
            return cgi.canFirePointBlankShots;
        }

        // Set a projectile's point blank duration
        public static void SetPointBlankDuration(Projectile projectile, int duration)
        {
            if (projectile != null)
                projectile.Calamity().pointBlankShotDuration = duration;
        }

        // Gets a projectile's current point blank duration
        public static int GetPointBlankDuration(Projectile projectile) => projectile?.Calamity()?.pointBlankShotDuration ?? 0;
        #endregion

        #region Amalgam Potion Buff List
        public static bool SetAmalgamBuffList(int type, bool shouldBeListed)
        {
            if (shouldBeListed && !CalamityLists.amalgamBuffList.Contains(type))
            {
                CalamityLists.amalgamBuffList.Add(type);
                return true;
            }
            else if (!shouldBeListed)
            {
                return CalamityLists.amalgamBuffList.Remove(type);
            }

            return false;
        }
        public static bool SetPersistentBuffList(int type, bool isPersistent)
        {
            if (isPersistent && !CalamityLists.persistentBuffList.Contains(type))
            {
                CalamityLists.persistentBuffList.Add(type);
                return true;
            }
            else if (!isPersistent)
            {
                return CalamityLists.persistentBuffList.Remove(type);
            }

            return false;
        }

        public static bool IsOnAmalgamBuffList(int type) => CalamityLists.amalgamBuffList.Contains(type);
        public static bool IsOnPersistentBuffList(int type) => CalamityLists.persistentBuffList.Contains(type);
        #endregion

        #region Venerated Locket Bans
        public static bool AddToVeneratedLocketBanlist(int type)
        {
            if (!CalamityLists.VeneratedLocketBanlist.Contains(type))
            {
                CalamityLists.VeneratedLocketBanlist.Add(type);
                return true;
            }
            return false;
        }
        #endregion

        #region Summoner Cross Class Nerf Disabling
        public static bool SetSummonerNerfDisabledByMinion(int type, bool disableNerf)
        {
            if (disableNerf && !CalamityLists.DisabledSummonerNerfMinions.Contains(type))
            {
                CalamityLists.DisabledSummonerNerfMinions.Add(type);
                return true;
            }
            else if (!disableNerf)
            {
                return CalamityLists.DisabledSummonerNerfMinions.Remove(type);
            }

            return false;
        }
        public static bool SetSummonerNerfDisabledByItem(int type, bool disableNerf)
        {
            if (disableNerf && !CalamityLists.DisabledSummonerNerfItems.Contains(type))
            {
                CalamityLists.DisabledSummonerNerfItems.Add(type);
                return true;
            }
            else if (!disableNerf)
            {
                return CalamityLists.DisabledSummonerNerfItems.Remove(type);
            }

            return false;
        }

        public static bool GetSummonerNerfDisabledByMinion(int type) => CalamityLists.DisabledSummonerNerfMinions.Contains(type);
        public static bool GetSummonerNerfDisabledByItem(int type) => CalamityLists.DisabledSummonerNerfItems.Contains(type);
        #endregion

        #region Debuff Display support
        public static void RegisterDebuff(string texturePath, Predicate<NPC> debuffCheck)
        {
            if (!CalamityGlobalNPC.moddedDebuffTextureList.Contains((texturePath, debuffCheck)))
            {
                CalamityGlobalNPC.moddedDebuffTextureList.Add((texturePath, debuffCheck));
            }
        }
        #endregion

        #region Town NPC Alert support
        public static void RegisterTownNPCShop(int id, Predicate<Player> getShop, Action<Player, bool> setShop)
        {
            if (!CalamityGlobalNPC.npcAlertList.Contains((id, getShop, setShop)))
            {
                CalamityGlobalNPC.npcAlertList.Add((id, getShop, setShop));
            }
        }
        #endregion

        #region Permanent Boosters
        // [Aliases("GetPermanentPowerup", "GetPowerup", "HasPermanentBooster", "GetPermanentBooster", "GetBooster")]
        public static bool HasPermanentPowerup(Player player, string powerupName)
        {
            return powerupName switch
            {
                "BloodOrange" => player.Calamity().bOrange,
                "MiracleFruit" => player.Calamity().mFruit,
                "Elderberry" => player.Calamity().eBerry,
                "Dragonfruit" => player.Calamity().dFruit,

                "CometShard" => player.Calamity().cShard,
                "EtherealCore" => player.Calamity().eCore,
                "PhantomHeart" => player.Calamity().pHeart,

                "MushroomPlasmaRoot" => player.Calamity().rageBoostOne,
                "InfernalBlood" => player.Calamity().rageBoostTwo,
                "RedLightningContainer" => player.Calamity().rageBoostThree,

                "ElectrolyteGelPack" => player.Calamity().adrenalineBoostOne,
                "StarlightFuelCell" => player.Calamity().adrenalineBoostTwo,
                "Ectoheart" => player.Calamity().adrenalineBoostThree,

                "CelestialOnion" => player.Calamity().extraAccessoryML,

                _ => false, // Return false if no case is found
            };
        }

        // [Aliases("SetPowerup", "SetPermanentBooster", "SetBooster")]
        public static void SetPermanentPowerup(Player player, string powerupName, bool value)
        {

            switch (powerupName)
            {
                case "BloodOrange": player.Calamity().bOrange = value; break;
                case "MiracleFruit": player.Calamity().mFruit = value; break;
                case "Elderberry": player.Calamity().eBerry = value; break;
                case "Dragonfruit": player.Calamity().dFruit = value; break;

                case "CometShard": player.Calamity().cShard = value; break;
                case "EtherealCore": player.Calamity().eCore = value; break;
                case "PhantomHeart": player.Calamity().pHeart = value; break;

                case "MushroomPlasmaRoot": player.Calamity().rageBoostOne = value; break;
                case "InfernalBlood": player.Calamity().rageBoostTwo = value; break;
                case "RedLightningContainer": player.Calamity().rageBoostThree = value; break;

                case "ElectrolyteGelPack": player.Calamity().adrenalineBoostOne = value; break;
                case "StarlightFuelCell": player.Calamity().adrenalineBoostTwo = value; break;
                case "Ectoheart": player.Calamity().adrenalineBoostThree = value; break;

                case "CelestialOnion": player.Calamity().extraAccessoryML = value; break;
            };
        }
        #endregion

        #region Call

        public static object Call(params object[] args)
        {
            bool isValidPlayerArg(object o) => o is int || o is Player;
            bool isValidItemArg(object o) => o is int || o is Item;
            bool isValidProjectileArg(object o) => o is int || o is Projectile;
            bool isValidNPCArg(object o) => o is int || o is NPC;

            Player castPlayer(object o)
            {
                if (o is int i)
                    return Main.player[i];
                else if (o is Player p)
                    return p;
                return null;
            }

            Item castItem(object o)
            {
                if (o is int i)
                    return Main.item[i];
                else if (o is Item it)
                    return it;
                return null;
            }

            Projectile castProjectile(object o)
            {
                if (o is int i)
                    return Main.projectile[i];
                else if (o is Projectile p)
                    return p;
                return null;
            }

            NPC castNPC(object o)
            {
                if (o is int i)
                    return Main.npc[i];
                else if (o is NPC n)
                    return n;
                return null;
            }

            // Certain IDs in vanilla's files are shorts instead of ints for some reason.
            // Instead of expecting developers to have to manually cast these IDs, this function is used
            // to handle IDs that are either ints OR shorts, without the worry of missing a cast and wondering why
            // the Mod Call did nothing.
            bool castID(object o, out int id)
            {
                id = -1;
                if (!(o is int) && !(o is short))
                    return false;

                if (o is short shortID)
                    id = shortID;
                if (o is int intID)
                    id = intID;

                return true;
            }

            if (args is null || args.Length <= 0)
                return new ArgumentNullException("ERROR: No function name specified. First argument must be a function name.");
            if (!(args[0] is string))
                return new ArgumentException("ERROR: First argument must be a string function name.");

            string methodName = args[0].ToString();
            switch (methodName)
            {
                case "Downed":
                case "GetDowned":
                case "BossDowned":
                case "GetBossDowned":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a boss or event name as a string.");
                    if (!(args[1] is string))
                        return new ArgumentException("ERROR: The argument to \"Downed\" must be a string.");
                    return GetBossDowned(args[1].ToString());

                case "Zone":
                case "GetZone":
                case "InZone":
                case "GetInZone":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and a zone name as a string.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify a zone name as a string.");
                    if (!(args[2] is string))
                        return new ArgumentException("ERROR: The second argument to \"InZone\" must be a string.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"InZone\" must be a Player or an int.");
                    return GetInZone(castPlayer(args[1]), args[2].ToString());

                case "Difficulty":
                case "GetDifficulty":
                case "DifficultyActive":
                case "GetDifficultyActive":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a difficulty mode name as a string.");
                    if (!(args[1] is string))
                        return new ArgumentException("ERROR: The argument to \"Difficulty\" must be a string.");
                    return GetDifficultyActive(args[1].ToString());

                case "SetDifficulty":
                case "SetDifficultyActive":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a difficulty mode name as a string and a bool.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify a bool.");
                    if (!(args[2] is bool enabled))
                        return new ArgumentException("ERROR: The second argument to \"SetDifficulty\" must be a bool.");
                    if (!(args[1] is string))
                        return new ArgumentException("ERROR: The first argument to \"SetDifficulty\" must be a string.");
                    return SetDifficultyActive(args[1].ToString(), enabled);


                case "AddDifficultyToUI":
                    if (args.Length < 2)
                        return new ArgumentException("ERROR: Not enough arguements provided");

                    if (args[1] is not DifficultyMode mode)
                        return new ArgumentException("ERROR: A class inheriting from 'DifficultyMode' must be provided.");
                    AddCustomDifficulty(mode);
                    return null;

                case "AddWorldScreenDifficulty":
                case "AddWorldSelectionDifficulty":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify a difficulty mode name as a string.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify a Func<AWorldListItem, bool>.");
                        if (args.Length < 4)
                            return new ArgumentNullException("ERROR: Must specify a Color.");

                        if (args.Length >= 5)
                        {
                            if (!(args[4] is int))
                                return new ArgumentException("ERROR: The fourth argument to \"AddWorldScreenDifficulty\" must be an int.");
                        }
                        if (!(args[3] is Color color))
                            return new ArgumentException("ERROR: The third argument to \"AddWorldScreenDifficulty\" must be a Color.");
                        if (!(args[2] is Func<AWorldListItem, bool> worldFunction))
                            return new ArgumentException("ERROR: The second argument to \"AddWorldScreenDifficulty\" must be a Func<AWorldListItem, bool>.");
                        if (!(args[1] is string))
                            return new ArgumentException("ERROR: The first argument to \"AddWorldScreenDifficulty\" must be a string.");

                        if (args.Length >= 5)
                            AddWorldScreenDifficulty(args[1].ToString(), worldFunction, color, (int)args[4]);
                        else
                            AddWorldScreenDifficulty(args[1].ToString(), worldFunction, color);
                        return null;
                    }


                case "GetLight":
                case "GetLightLevel":
                case "GetLightStrength":
                case "GetAbyssLight":
                case "GetAbyssLightLevel":
                case "GetAbyssLightStrength":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The argument to \"GetLightStrength\" must be a Player or an int.");
                    return GetLightStrength(castPlayer(args[1]));

                case "AddLight":
                case "AddLightLevel":
                case "AddLightStrength":
                case "AddAbyssLight":
                case "AddAbyssLightLevel":
                case "AddAbyssLightStrength":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and light strength change as an int.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify light strength change as an int.");
                    if (!(args[2] is int light))
                        return new ArgumentException("ERROR: The second argument to \"AddLightStrength\" must be an int.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"AddLightStrength\" must be a Player or an int.");
                    AddAbyssLightStrength(castPlayer(args[1]), light);
                    return null;

                case "InfiniteFlight":
                case "AddInfiniteFlight":
                case "EnableInfiniteFlight":
                case "ToggleInfiniteFlight":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and if the player should gain infinite flight as a bool.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify if a player should gain infinite flight as a bool.");
                    if (!(args[2] is bool))
                        return new ArgumentException("ERROR: The second argument to \"InfiniteFlight\" must be a bool.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"InfiniteFlight\" must be a Player or an int.");
                    bool fly = (bool)args[2];
                    ToggleInfiniteFlight(castPlayer(args[1]), fly);
                    return null;

                case "GetRogueArmor":
                case "GetWearingRogueArmor":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The argument to \"GetRogueArmor\" must be a Player or an int.");
                    return GetWearingRogueArmor(castPlayer(args[1]));

                case "SetRogueArmor":
                case "SetWearingRogueArmor":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and if the player should be counted as wearing rogue armor as a bool.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify if a player should be counted as wearing rogue armor as a bool.");
                    if (!(args[2] is bool))
                        return new ArgumentException("ERROR: The second argument to \"SetRogueArmor\" must be a bool.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"SetRogueArmor\" must be a Player or an int.");
                    bool roguearmor = (bool)args[2];
                    SetWearingRogueArmor(castPlayer(args[1]), roguearmor);
                    return null;

                case "GetPostMLSummonArmor":
                case "GetWearingPostMLSummonArmor":
                case "GetPostMoonLordSummonArmor":
                case "GetWearingPostMoonLordSummonArmor":
                case "GetPostMLSummonerArmor":
                case "GetWearingPostMLSummonerArmor":
                case "GetPostMoonLordSummonerArmor":
                case "GetWearingPostMoonLordSummonerArmor":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The argument to \"GetPostMoonLordSummonerArmor\" must be a Player or an int.");
                    return GetWearingPostMLSummonerArmor(castPlayer(args[1]));

                case "SetPostMLSummonArmor":
                case "SetWearingPostMLSummonArmor":
                case "SetPostMoonLordSummonArmor":
                case "SetWearingPostMoonLordSummonArmor":
                case "SetPostMLSummonerArmor":
                case "SetWearingPostMLSummonerArmor":
                case "SetPostMoonLordSummonerArmor":
                case "SetWearingPostMoonLordSummonerArmor":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and if the player should be counted as wearing Post-Moon Lord summoner armor as a bool.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify if a player should be counted as wearing Post-Moon Lord summoner armor as a bool.");
                    if (!(args[2] is bool))
                        return new ArgumentException("ERROR: The second argument to \"SetPostMoonLordSummonerArmor\" must be a bool.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"SetPostMoonLordSummonerArmor\" must be a Player or an int.");
                    bool summonarmor = (bool)args[2];
                    SetWearingPostMLSummonerArmor(castPlayer(args[1]), summonarmor);
                    return null;

                case "GetRogueVelocity":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The argument to \"GetRogueVelocity\" must be a Player or an int.");
                    return GetRogueVelocity(castPlayer(args[1]));

                case "AddRogueVelocity":
                case "ModifyRogueVelocity":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and rogue velocity change as a float.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify rogue velocity change as a float.");
                    if (!(args[2] is float velocity))
                        return new ArgumentException("ERROR: The second argument to \"AddRogueVelocity\" must be a float.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"AddRogueVelocity\" must be a Player or an int.");
                    return AddRogueVelocity(castPlayer(args[1]), velocity);

                case "GetStealth":
                case "GetCurrentStealth":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetStealth\" must be a Player or an int.");
                    return GetCurrentStealth(castPlayer(args[1]));

                case "GetMaxStealth":
                case "GetStealthCap":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetMaxStealth\" must be a Player or an int.");
                    return GetMaxStealth(castPlayer(args[1]));

                case "AddMaxStealth":
                case "ModifyMaxStealth":
                case "ModifyStealthCap":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both a Player object (or int index of a Player) and rogue max stealth as a float.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify rogue max stealth as a float.");
                    if (!(args[2] is float maxStealth))
                        return new ArgumentException("ERROR: The second argument to \"AddMaxStealth\" must be a float.");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"AddMaxStealth\" must be a Player or an int.");
                    return AddMaxStealth(castPlayer(args[1]), maxStealth);

                case "CanStealthStrike":
                case "StealthStrikeAvailable":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"CanStealthStrike\" must be a Player or an int.");
                    return CanStealthStrike(castPlayer(args[1]));

                case "SetStealthProjectile":
                case "SetStealthStrikeProjectile":
                case "SetProjectileStealth":
                case "SetProjectileStealthStrike":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both a Projectile and if the Projectile should be counted as a stealth strike as a bool.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify whether or not the projectile was created from a stealth strike as a bool.");
                        if (!(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetStealthProjectile\" must be a bool.");
                        if (!isValidProjectileArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetStealthProjectile\" must be a Projectile.");

                        bool ssEnabled = (bool)args[2];
                        SetStealthProjectile(castProjectile(args[1]), ssEnabled);
                        return null;
                    }

                case "GetStealthProjectile":
                case "GetStealthStrikeProjectile":
                case "GetProjectileStealth":
                case "GetProjectileStealthStrike":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify a Projectile.");
                        if (!isValidProjectileArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetStealthProjectile\" must be a Projectile.");
                        return GetStealthProjectile(castProjectile(args[1]));
                    }

                case "ConsumeStealth":
                case "ConsumeStealthByAttacking":
                case "UseStealth":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                        if (!isValidPlayerArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"ConsumeStealth\" must be a Player or an int.");
                        ConsumeStealth(castPlayer(args[1]));
                        return null;
                    }

                case "GetRage":
                case "GetRageCurrent":
                case "GetCurrentRage":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetRage\" must be a Player or an int.");
                    return GetRage(castPlayer(args[1]));

                case "GetAdrenaline":
                case "GetAdrenalineCurrent":
                case "GetCurrentAdrenaline":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetAdrenaline\" must be a Player or an int.");
                    return GetAdrenaline(castPlayer(args[1]));

                case "GetMaxRage":
                case "GetRageMax":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetMaxRage\" must be a Player or an int.");
                    return GetRageMax(castPlayer(args[1]));

                case "GetMaxAdrenaline":
                case "GetAdrenalineMax":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetMaxAdrenaline\" must be a Player or an int.");
                    return GetAdrenalineMax(castPlayer(args[1]));

                case "GetMaxCharge":
                case "GetChargeMax":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify an Item object (or int index of an Item).");
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetMaxCharge\" must be an Item or an int.");
                    return GetMaxCharge(castItem(args[1]));

                case "SetMaxCharge":
                case "SetChargeMax":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an Item and charge as a float or double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify charge as a float or double.");
                        if (!(args[2] is float) && !(args[2] is double))
                            return new ArgumentException("ERROR: The second argument to \"SetMaxCharge\" must be a float or a double.");
                        if (!isValidItemArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetMaxCharge\" must be an Item.");

                        float Charge = (float)args[2];
                        SetMaxCharge(castItem(args[1]), Charge);
                        return null;
                    }

                case "GetCharge":
                case "GetCurrentCharge":
                case "GetChargeCurrent":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify an Item object (or int index of an Item).");
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetCharge\" must be an Item or an int.");
                    return GetCharge(castItem(args[1]));

                case "SetCharge":
                case "SetCurrentCharge":
                case "SetChargeCurrent":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an Item and charge as a float or double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify charge as a float or double.");
                        if (!(args[2] is float) && !(args[2] is double))
                            return new ArgumentException("ERROR: The second argument to \"SetCharge\" must be a float or a double.");
                        if (!isValidItemArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetCharge\" must be an Item.");

                        float Charge = (float)args[2];
                        SetCharge(castItem(args[1]), Charge);
                        return null;
                    }

                case "GetChargePerUse":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify an Item object (or int index of an Item).");
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetChargePerUse\" must be an Item or an int.");
                    return GetChargePerUse(castItem(args[1]));

                case "SetChargePerUse":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an Item and charge as a float or double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify charge as a float or double.");
                        if (!(args[2] is float) && !(args[2] is double))
                            return new ArgumentException("ERROR: The second argument to \"SetChargePerUse\" must be a float or a double.");
                        if (!isValidItemArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetChargePerUse\" must be an Item.");

                        float Charge = (float)args[2];
                        SetChargePerUse(castItem(args[1]), Charge);
                        return null;
                    }

                case "GetChargePerAltUse":
                case "GetChargePerUseAlt":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify an Item object (or int index of an Item).");
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetChargePerUse\" must be an Item or an int.");
                    return GetChargePerAltUse(castItem(args[1]));

                case "SetChargePerAltUse":
                case "SetChargeUseAlt":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an Item and charge as a float or double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify charge as a float or double.");
                        if (!(args[2] is float) && !(args[2] is double))
                            return new ArgumentException("ERROR: The second argument to \"SetChargePerUseAlt\" must be a float or a double.");
                        if (!isValidItemArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetChargePerUseAlt\" must be an Item.");

                        float Charge = (float)args[2];
                        SetChargePerAltUse(castItem(args[1]), Charge);
                        return null;
                    }

                case "GetChargeable":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify an Item object (or int index of an Item).");
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetChargeable\" must be an Item or an int.");
                    return GetChargeable(castItem(args[1]));

                case "SetChargeable":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an Item and if the item can be charged as a bool.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify the ability to charge as a bool.");
                        if (!(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetChargeable\" must be a bool.");
                        if (!isValidItemArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetChargeable\" must be an Item.");

                        bool Charge = (bool)args[2];
                        SetChargeable(castItem(args[1]), Charge);
                        return null;
                    }

                case "GetRightClickListener":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetRightClickListener\" must be a Player or an int.");
                    return GetRightClickListener(castPlayer(args[1]));

                case "GetMouseWorldListener":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetMouseWorldListener\" must be a Player or an int.");
                    return GetMouseWorldListener(castPlayer(args[1]));

                case "GetRightClick":
                case "GetMouseRight":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetRightClick\" must be a Player or an int.");
                    return GetRightClick(castPlayer(args[1]));

                case "GetMouseWorld":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify a Player object (or int index of a Player).");
                    if (!isValidPlayerArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"GetMouseWorld\" must be a Player or an int.");
                    return GetMouseWorld(castPlayer(args[1]));

                case "SetRightClickListener":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both a Player and whether or not the listener is active.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify status of the listener as a bool.");
                        if (!(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetRightClickListener\" must be a bool.");
                        if (!isValidPlayerArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetRightClickListener\" must be a Player.");

                        bool active = (bool)args[2];
                        SetRightClickListener(castPlayer(args[1]), active);
                        return null;
                    }

                case "SetMouseWorldListener":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both a Player and whether or not the listener is active.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify status of the listener as a bool.");
                        if (!(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetMouseWorldListener\" must be a bool.");
                        if (!isValidPlayerArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetMouseWorldListener\" must be a Player.");

                        bool active = (bool)args[2];
                        SetMouseWorldListener(castPlayer(args[1]), active);
                        return null;
                    }

                case "SetDR":
                case "SetDamageReduction":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both NPC ID as an int and damage reduction as a float or double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify damage reduction as a float or double.");
                        if (!(args[2] is float) && !(args[2] is double))
                            return new ArgumentException("ERROR: The second argument to \"SetDamageReduction\" must be a float or a double.");
                        if (!castID(args[1], out int npcID))
                            return new ArgumentException("ERROR: The first argument to \"SetDamageReduction\" must be an int or short ID.");

                        float DR = (float)args[2];
                        return SetDamageReduction(npcID, DR);
                    }

                case "SetDRSpecific":
                case "SetDamageReductionSpecfic":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an NPC and damage reduction as a float or double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify damage reduction as a float or double.");
                        if (!(args[2] is float) && !(args[2] is double))
                            return new ArgumentException("ERROR: The second argument to \"SetDamageReduction\" must be a float or a double.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetDamageReduction\" must be an NPC.");

                        float DR = (float)args[2];
                        SetDamageReductionSpecific(castNPC(args[1]), DR);
                        return null;
                    }

                case "GetDamageReduction":
                case "GetDR":
                case "GetDRSpecific":
                case "GetDamageReductionSpecific":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetDamageReduction\" must be an NPC.");
                        return GetDamageReduction(castNPC(args[1]));
                    }

                case "SetDefenseDamageNPC":
                case "SetNPCDefenseDamage":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an NPC and if the NPC can deal defense damage as a bool.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify the ability to deal defense damage as a bool.");
                        if (!(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetDefenseDamageNPC\" must be a bool.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetDefenseDamageNPC\" must be an NPC.");

                        bool ddEnabled = (bool)args[2];
                        SetDefenseDamageNPC(castNPC(args[1]), ddEnabled);
                        return null;
                    }

                case "GetDefenseDamageNPC":
                case "GetNPCDefenseDamage":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetDefenseDamageNPC\" must be an NPC.");
                        return GetDefenseDamageNPC(castNPC(args[1]));
                    }

                case "SetDefenseDamageProjectile":
                case "SetProjectileDefenseDamage":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both a Projectile and if the Projectile can deal defense damage as a bool.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify the ability to deal defense damage as a bool.");
                        if (!(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetDefenseDamageProjectile\" must be a bool.");
                        if (!isValidProjectileArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetDefenseDamageProjectile\" must be a Projectile.");

                        bool ddEnabled = (bool)args[2];
                        SetDefenseDamageProjectile(castProjectile(args[1]), ddEnabled);
                        return null;
                    }

                case "GetDefenseDamageProjectile":
                case "GetProjectileDefenseDamage":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify a Projectile.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetDefenseDamageProjectile\" must be a Projectile.");
                        return GetDefenseDamageProjectile(castProjectile(args[1]));
                    }

                case "GetDebuffVulnerability":
                case "GetDebuffVulnerabilities":
                case "GetVulnerableDebuffs":
                case "GetVulnerability":
                case "GetVulnerabilities":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC and a debuff type as a string.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify a debuff type as a string.");
                        if (!(args[2] is string))
                            return new ArgumentException("ERROR: The second argument to \"SetDebuffVulnerability\" must be a string.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetDebuffVulnerability\" must be an NPC.");
                        return GetDebuffVulnerability(castNPC(args[1]), args[2].ToString());
                    }

                case "SetDebuffVulnerability":
                case "SetDebuffVulnerabilities":
                case "SetVulnerableDebuffs":
                case "SetVulnerability":
                case "SetVulnerabilities":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC, debuff type as a string, and whether to add or remove a vulnerability as a bool.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify a debuff type as a string, and whether to add or remove a vulnerability as a bool.");
                        if (args.Length < 4)
                            return new ArgumentNullException("ERROR: Must specify whether to add or remove a vulnerability as a bool.");
                        if ((!(args[3] is bool)) && args[3] != null)
                            return new ArgumentException("ERROR: The third argument to \"SetDebuffVulnerability\" must be a bool.");
                        if (!(args[2] is string))
                            return new ArgumentException("ERROR: The second argument to \"SetDebuffVulnerability\" must be a string.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetDebuffVulnerability\" must be an NPC.");
                        SetDebuffVulnerability(castNPC(args[1]), args[2].ToString(), (bool?)args[3]);
                        return null;
                    }

                case "GetCalamityAI":
                case "GetNewAI":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetCalamityAI\" must be an NPC.");
                        return GetCalamityAI(castNPC(args[1]));
                    }

                case "SetCalamityAI":
                case "SetNewAI":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC, an AI slot as an int, and a value for it as a float or a double.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify an AI slot as an int, and a value for it as a float or a double.");
                        if (args.Length < 4)
                            return new ArgumentNullException("ERROR: Must specify a value for the AI slot as a float or a double.");
                        if (!(args[3] is float) && !(args[3] is double))
                            return new ArgumentException("ERROR: The third argument to \"SetCalamityAI\" must be a float or a double.");
                        if (!(args[2] is int newValue))
                            return new ArgumentException("ERROR: The second argument to \"SetCalamityAI\" must be an int.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetCalamityAI\" must be an NPC.");
                        SetCalamityAI(castNPC(args[1]), newValue, (float)args[3]);
                        return null;
                    }

                case "BossHealthBarVisible":
                case "BossHealthBarsVisible":
                case "GetBossHealthBarVisible":
                case "GetBossHealthBarsVisible":
                    return BossHealthBarVisible();

                case "SetBossHealthBarVisible":
                case "SetBossHealthBarsVisible":
                    if (args.Length < 2 || !(args[1] is bool bossBarEnabled))
                        return new ArgumentNullException("ERROR: Must specify a bool.");
                    return SetBossHealthBarVisible(bossBarEnabled);

                case "SetShouldCloseBossHealthBar":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both an NPC and whether or not the NPC's health bar should be closed as a bool.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify whether or not the NPC's health bar should be closed as a bool.");
                        if (!(args[2] is float) && !(args[2] is bool))
                            return new ArgumentException("ERROR: The second argument to \"SetShouldCloseBossHealthBar\" must be a bool.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetShouldCloseBossHealthBar\" must be an NPC.");

                        SetShouldCloseBossHealthBar(castNPC(args[1]), (bool)args[2]);
                        return null;
                    }

                case "GetShouldCloseBossHealthbar":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify an NPC.");
                        if (!isValidNPCArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetShouldCloseBossHealthBar\" must be an NPC.");
                        return GetShouldCloseBossHealthBar(castNPC(args[1]));
                    }

                case "CanFirePointBlank":
                case "CanFirePointBlankShots":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify an Item object (or int index of an Item in the Main.item array)."); ;
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"CanFirePointBlank\" must be an Item or an int.");
                    return CanFirePointBlank(castItem(args[1]));

                case "SetFirePointBlank":
                case "SetFirePointBlankShots":
                    if (args.Length < 2)
                        return new ArgumentNullException("ERROR: Must specify both an Item object (or int index of an Item in the Main.item array) and a bool.");
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify whether the item can fire point blank as a bool.");
                    if (!(args[2] is bool firePointBlank))
                        return new ArgumentException("ERROR: The second argument to \"SetFirePointBlank\" must be a bool.");
                    if (!isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: The first argument to \"SetFirePointBlank\" must be an Item or an int.");
                    return SetFirePointBlank(castItem(args[1]), firePointBlank);

                case "SetPointBlankDuration":
                case "SetProjectilePointBlank":
                case "SetProjectilePointBlankDuration":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify both a Projectile and point blank duration as an int.");
                        if (args.Length < 3)
                            return new ArgumentNullException("ERROR: Must specify the point blank duration as an int.");
                        if (!(args[2] is int pbDuration))
                            return new ArgumentException("ERROR: The second argument to \"SetPointBlankDuration\" must be an int.");
                        if (!isValidProjectileArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"SetPointBlankDuration\" must be a Projectile.");

                        SetPointBlankDuration(castProjectile(args[1]), pbDuration);
                        return null;
                    }

                case "GetPointBlankDuration":
                case "GetProjectilePointBlank":
                case "GetProjectilePointBlankDuration":
                    {
                        if (args.Length < 2)
                            return new ArgumentNullException("ERROR: Must specify a Projectile.");
                        if (!isValidProjectileArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"GetPointBlankDuration\" must be a Projectile.");
                        return GetPointBlankDuration(castProjectile(args[1]));
                    }

                case "NoDodges":
                case "DodgesDisabled":
                case "GetDodgesDisabled":
                    return AreDodgesDisabled();

                case "DisableDodges":
                case "DisableAllDodges":
                case "SetDodgesDisabled":
                    if (args.Length < 2 || !(args[1] is bool disableDodges))
                        return new ArgumentNullException("ERROR: Must specify a bool.");
                    return DisableAllDodges(disableDodges);

                case "AcidRainActive":
                case "IsAcidRainActive":
                case "GetAcidRainActive":
                    return AcidRainEvent.AcidRainEventIsOngoing;

                case "StartAcidRain":
                    AcidRainEvent.TryStartEvent(true);
                    CalamityNetcode.SyncWorld();
                    return true;

                case "StopAcidRain":
                    if (AcidRainEvent.AcidRainEventIsOngoing)
                    {
                        AcidRainEvent.AccumulatedKillPoints = 0;
                        AcidRainEvent.HasTriedToSummonOldDuke = false;
                        AcidRainEvent.UpdateInvasion(false);
                    }
                    return true;

                // This is intentionally separate from the above because it will stop other events when they are added.
                case "AbominationnClearEvents":
                    bool eventActive = AcidRainEvent.AcidRainEventIsOngoing;
                    bool canClear = Convert.ToBoolean(args[1]); //This is to indicate whether abomm is able to clear the event due to a cooldown
                    if (eventActive && canClear) //adjust based on other events when added.
                    {
                        AcidRainEvent.AccumulatedKillPoints = 0;
                        AcidRainEvent.HasTriedToSummonOldDuke = false;
                        AcidRainEvent.UpdateInvasion(false);
                    }
                    return eventActive;

                case "CreateEnchantment":
                case "RegisterEnchantment":
                    EnchantmentManager.ConstructFromModcall(args.Skip(1));
                    return null;

                case "MakeItemExhumable":
                    if (args.Length != 3)
                        return new ArgumentNullException("ERROR: Must specify two Item types as an int.");
                    if (!castID(args[1], out int toExhume))
                        return new ArgumentException("ERROR: The first argument to \"MakeItemExhumable\" must be an int or short ID.");
                    if (!castID(args[2], out int result))
                        return new ArgumentException("ERROR: The second argument to \"MakeItemExhumable\" must be an int or short ID.");
                    EnchantmentManager.ItemUpgradeRelationship[toExhume] = result;
                    return null;

                case "DeclareMiniboss":
                case "DeclareMinibossForHealthBar":
                    if (args.Length != 2)
                        return new ArgumentNullException("ERROR: Must specify both an NPC type as an int.");
                    if (!castID(args[1], out int npcType))
                        return new ArgumentException("ERROR: The first argument to \"DeclareMiniboss\" must be an int or short ID.");

                    BossHealthBarManager.MinibossHPBarList.Add(npcType);
                    return null;

                case "ExcludeBossFromHealthBar":
                    if (args.Length != 2)
                        return new ArgumentNullException("ERROR: Must specify both an NPC type as an int.");
                    if (!castID(args[1], out int npcType2))
                        return new ArgumentException("ERROR: The first argument to \"ExcludeBossFromHealthBar\" must be an int or short ID.");

                    BossHealthBarManager.BossExclusionList.Add(npcType2);
                    return null;

                case "DeclareOneToManyRelationshipForHealthBar":
                    if (args.Length < 3)
                        return new ArgumentNullException("ERROR: Must specify both an NPC type as an int for the first argument and the other NPC types in the relationship as ints for the rest of the arguments.");
                    if (!args.Skip(1).All(a => castID(a, out _)))
                        return new ArgumentException("ERROR: All secondary and onward arguments to \"DeclareOneToManyRelationshipForHealthBar\" must be int or short IDs.");

                    castID(args[1], out int npcType3);

                    int[] npcsInRelationship = args.Skip(2).Select(a => (int)a).ToArray();
                    BossHealthBarManager.OneToMany[npcType3] = npcsInRelationship;
                    return null;

                // For context, the boolean argument in the second delegate refers to whether the function should be accumulating max life (true) or just life (false), and the returned long should be the accumulated health.
                case "DeclareSpecialHPCalculationDecisionForHealthBar":
                    if (args.Length != 3)
                        return new ArgumentNullException("ERROR: Must specify both a usage requirement as a Func<NPC, bool> and a health calculator function as a Func<NPC, bool, long>.");
                    if (!(args[1] is Func<NPC, bool> usageRequirement))
                        return new ArgumentException("ERROR: The first argument to \"DeclareSpecialHPCalculationDecisionForHealthBar\" must be a Func<NPC, bool>.");
                    if (!(args[2] is Func<NPC, bool, long> healthCalculatorFunction))
                        return new ArgumentException("ERROR: The first argument to \"DeclareSpecialHPCalculationDecisionForHealthBar\" must be a Func<NPC, bool, long>.");

                    BossHealthBarManager.SpecialHPRequirements[new BossHealthBarManager.NPCSpecialHPGetRequirement(usageRequirement)] = new BossHealthBarManager.NPCSpecialHPGetFunction(healthCalculatorFunction);
                    return null;

                case "CreateNameExtensionHandlerForHealthBar":
                    if (args.Length < 4)
                        return new ArgumentNullException("ERROR: Must specify a extension name as a string, the main NPC type as an int, and the other NPC types to check for as ints the rest of the arguments.");
                    if (!(args[1] is LocalizedText name))
                        return new ArgumentException("ERROR: The first argument to \"CreateNameExtensionHandlerForHealthBar\" must be a LocalizedText.");
                    if (!castID(args[1], out int npcType4))
                        return new ArgumentException("ERROR: The second argument to \"CreateNameExtensionHandlerForHealthBar\" must be an int or short ID.");
                    if (!args.Skip(3).All(a => a is int))
                        return new ArgumentException("ERROR: All ternary and onward arguments to \"CreateNameExtensionHandlerForHealthBar\" must be ints.");

                    int[] npcsToCheckFor = args.Skip(3).Select(a => (int)a).ToArray();
                    BossHealthBarManager.EntityExtensionHandler[npcType4] = new BossHealthBarManager.BossEntityExtension(name, npcsToCheckFor);
                    return null;

                // In the following two mod calls, the first argument is the NPC type, the second is the time change context (-1 being night, 0 being nothing, and 1 being day),
                // the third being the boss spawning function, the fourth being the overriding countdown to use, the fifth being whether the boss uses a special sound on spawning,
                // the sixth being the dimness factor that Boss Rush should become once the boss is currently present, the seventh being the array of NPCs present in the battle that
                // should not be deleted by the Boss Rush itself, and the eight being the potential NPCs that will end up killing, assuming the initial boss isn't
                // that (such as P1 Hive Mind turning into its second form and you being expected to kill that).
                case "GetBossRushEntries":
                    var entries = new List<(int, int, Action<int>, int, bool, float, int[], int[])>();
                    foreach (BossRushEvent.Boss boss in BossRushEvent.Bosses)
                    {
                        int[] deathEntries = BossRushEvent.BossIDsAfterDeath.ContainsKey(boss.EntityID) ? BossRushEvent.BossIDsAfterDeath[boss.EntityID] : null;
                        entries.Add((boss.EntityID, (int)boss.ToChangeTimeTo, new Action<int>(boss.SpawnContext), boss.SpecialSpawnCountdown, boss.UsesSpecialSound, boss.DimnessFactor, boss.HostileNPCsToNotDelete.ToArray(), deathEntries));
                    }

                    return entries;

                case "SetBossRushEntries":
                    if (args.Length != 2)
                        return new ArgumentNullException("ERROR: Must specify a list of bosses as a List<(int, int, Action<int>, int, bool, int[], int[])>.");
                    if (!(args[1] is List<(int, int, Action<int>, int, bool, float, int[], int[])> entries2))
                        return new ArgumentException("ERROR: The first argument to \"SetBossRushEntries\" must be a List<(int, int, Action<int>, int, bool, int[], int[])>.");

                    BossRushEvent.Bosses.Clear();
                    BossRushEvent.BossIDsAfterDeath.Clear();
                    foreach (var entry in entries2)
                    {
                        if (entry.Item8 != null)
                            BossRushEvent.BossIDsAfterDeath[entry.Item1] = entry.Item8;
                        BossRushEvent.Bosses.Add(new BossRushEvent.Boss(entry.Item1, (BossRushEvent.TimeChangeContext)entry.Item2, new BossRushEvent.Boss.OnSpawnContext(entry.Item3), entry.Item4, entry.Item5, entry.Item6, entry.Item7));
                    }

                    return null;

                case "CreateCustomDeathEffectForBossRush":
                    if (args.Length != 3)
                        return new ArgumentNullException("ERROR: Must specify both an NPC type and an Action<NPC> that determines what happens when the NPC is killed.");
                    if (!castID(args[1], out int npcType5))
                        return new ArgumentException("ERROR: The first argument to \"CreateCustomDeathEffectForBossRush\" must be an int or short ID.");
                    if (!(args[2] is Action<NPC> deathEffect))
                        return new ArgumentException("ERROR: The first argument to \"CreateCustomDeathEffectForBossRush\" must be an Action<NPC>.");

                    BossRushEvent.BossDeathEffects[npcType5] = deathEffect;
                    return null;

                case "LoadParticleInstances":
                    CalamityMod.Instance.Logger.Warn("This mod call is deprecated. Calamity automatically registers particles.");
                    return null;

                case "RegisterModCooldowns":
                    CalamityMod.Instance.Logger.Warn("This mod call is deprecated. Calamity automatically registers cooldowns.");
                    return null;

                case "GetSummonerNerfDisabledByItem":
                    if (args.Length != 2 || !isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: Must specify a valid item to check status of.");
                    return GetSummonerNerfDisabledByItem(castItem(args[1]).type);

                case "GetSummonerNerfDisabledByMinion":
                    if (args.Length != 2 || !isValidProjectileArg(args[1]))
                        return new ArgumentException("ERROR: Must specify a valid projectile to check status of.");
                    return GetSummonerNerfDisabledByMinion(castProjectile(args[1]).type);

                case "SetSummonerNerfDisabledByItem":
                    if (args.Length < 2 || !isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: Must specify a valid item to set the status of.");
                    if (args.Length != 3 || args[2] is not bool disableNerf)
                        return new ArgumentException("ERROR: Must specify a bool that determines whether the summoner nerf is disabled.");
                    return SetSummonerNerfDisabledByItem(castItem(args[1]).type, disableNerf);

                case "SetSummonerNerfDisabledByMinion":
                    if (args.Length < 2 || !isValidItemArg(args[1]))
                        return new ArgumentException("ERROR: Must specify a valid projectile to set the status of.");
                    if (args.Length != 3 || args[2] is not bool disableNerf2)
                        return new ArgumentException("ERROR: Must specify a bool that determines whether the summoner nerf is disabled.");
                    return SetSummonerNerfDisabledByItem(castItem(args[1]).type, disableNerf2);

                case "IsOnAmalgamBuffList":
                    if (args.Length != 2 || !castID(args[1], out int buffType))
                        return new ArgumentException("ERROR: Must specify a valid buff ID to check status of.");
                    return IsOnAmalgamBuffList(buffType);

                case "IsOnPersistentBuffList":
                case "IsPersistentBuff":
                    if (args.Length != 2 || !castID(args[1], out int buffType2))
                        return new ArgumentException("ERROR: Must specify a valid buff ID to check status of.");
                    return IsOnPersistentBuffList(buffType2);

                case "SetAmalgamBuffList":
                    if (args.Length < 2 || !castID(args[1], out int buffType3))
                        return new ArgumentException("ERROR: Must specify a valid buff ID to set the status of.");
                    if (args.Length != 3 || args[2] is not bool shouldBeListed)
                        return new ArgumentException("ERROR: Must specify a bool that determines whether the amalgam should enable extend the duration of this buff.");
                    return SetAmalgamBuffList(buffType3, shouldBeListed);

                case "SetPersistentBuffList":
                    if (args.Length < 2 || !castID(args[1], out int buffType4))
                        return new ArgumentException("ERROR: Must specify a valid buff ID to set the status of.");
                    if (args.Length != 3 || args[2] is not bool isPersistent)
                        return new ArgumentException("ERROR: Must specify a bool that determines whether the buff is persistent after death for the Amalgam to properly reset.");
                    return SetPersistentBuffList(buffType4, isPersistent);

                case "CreateCodebreakerDialogOption":
                    // NOTE: This is a legacy variant of this call. The variant with three arguments is the standard.
                    if (args.Length == 4)
                    {
                        if (args[1] is not string inquiry || args[2] is not string response || args[3] is not Func<bool> condition)
                            throw new ArgumentException("ERROR: Must specify a string that determines the inquiry, a string that determines the response, and a Func<bool> that determines the condition for the three argument call.");
                        DraedonDialogRegistry.DialogOptions.Add(new(inquiry, response, condition));
                    }
                    else if (args.Length == 3)
                    {
                        if (args[1] is not string localizationKey || args[2] is not Func<bool> condition)
                            throw new ArgumentException("ERROR: Must specify a string that determines the localization key and a Func<bool> that determines the condition for the two argument call.");
                        DraedonDialogRegistry.DialogOptions.Add(new(localizationKey, condition));
                    }
                    else
                        throw new ArgumentException("ERROR: Must specify either two or three arguments.");

                    return null;

                case "AddToVeneratedLocketBanlist":
                    if (args.Length < 2)
                        return new ArgumentException("ERROR: Not enough arguments!");
                    if (args[1] is not int itemType)
                        return new ArgumentException("ERROR: Must specify a valid item type as an int index of the item.");
                    return AddToVeneratedLocketBanlist(itemType);

                case "RegisterDebuff":
                case "AddToDebuffDisplay":
                case "AddDebuffDisplay":
                case "DisplayDebuff":
                case "DebuffIcon":
                    {
                        if (args.Length < 2 || args[1] is not string texturePath)
                            return new ArgumentException("ERROR: The first argument to \"RegisterDebuff\" must be the texture path to the debuff sprite as a string");
                        if (args.Length != 3 || args[2] is not Predicate<NPC> debuffCheck)
                            return new ArgumentException("ERROR: The second argument to \"RegisterDebuff\" Must be a Predicate<NPC> that checks if an NPC meets the conditions for the debuff.");
                        RegisterDebuff(texturePath, debuffCheck);
                        return null;
                    }

                case "RegisterNPCShop":
                case "RegisterShop":
                case "RegisterTownNPCShop":
                case "AddNPCShop":
                case "AddShop":
                case "AddTownNPCShop":
                    {
                        if (args.Length < 2 || args[1] is not int npc)
                            return new ArgumentException("ERROR: The first argument to \"RegisterTownNPCShop\" must be the id of the NPC");
                        if (args.Length < 3 || args[2] is not Predicate<Player> shopCheck)
                            return new ArgumentException("ERROR: The second argument to \"RegisterTownNPCShop\" Must be a Predicate<Player> that checks if the new shop variable bool is true or not.");
                        if (args.Length != 4 || args[3] is not Action<Player, bool> shopSet)
                            return new ArgumentException("ERROR: The third argument to \"RegisterTownNPCShop\" Must be a Action<Player, bool> that is able to get and set the player's shop variable bool.");
                        RegisterTownNPCShop(npc, shopCheck, shopSet);
                        return null;
                    }

                case "SetNewShopVariable":
                case "SendNPCShopAlert":
                case "SendNPCAlert":
                    {
                        if (args.Length < 2 || args[1] is not int[] npcs)
                            return new ArgumentException("ERROR: The first argument to \"SetNewShopVariable\" must be an integer array of npc ids that should be alerted.");
                        if (args.Length != 3 || args[2] is not bool alreadySet)
                            return new ArgumentException("ERROR: The second argument to \"SetNewShopVariable\" Must be a bool that determines if the shop alert should show.");
                        CalamityGlobalNPC.SetNewShopVariable(npcs, alreadySet);
                        return null;
                    }

                case "BossHealthBoost":
                case "GetBossHealthBoost":
                case "BossHealthMultiplier":
                case "GetBossHealthMultiplier":
                    return CalamityConfig.Instance.BossHealthBoost;

                case "HasPermanentPowerup":
                case "GetPermanentPowerup":
                case "GetPowerup":
                case "HasPermanentBooster":
                case "GetPermanentBooster":
                case "GetBooster":
                    {
                        if (!isValidPlayerArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"HasPermanentPowerup\" must be a Player.");
                        if (args[2] is not string powerupName)
                            return new ArgumentException("ERROR: The second argument to \"HasPermanentPowerup\" must be a string.");

                        return HasPermanentPowerup(castPlayer(args[1]), powerupName);
                    }

                case "SetPermanentPowerup":
                case "SetPowerup":
                case "SetPermanentBooster":
                case "SetBooster":
                    {
                        if (!isValidPlayerArg(args[1]))
                            return new ArgumentException("ERROR: The first argument to \"HasPermanentPowerup\" must be a Player.");
                        if (args[2] is not string powerupName)
                            return new ArgumentException("ERROR: The second argument to \"HasPermanentPowerup\" must be a string.");
                        if (args[3] is not bool value)
                            return new ArgumentException("ERROR: The third argument to \"HasPermanentPowerup\" must be a bool.");

                        SetPermanentPowerup(castPlayer(args[1]), powerupName, value);
                        return null;
                    }

                case "RegisterAndroombaSolution":
                case "RegisterAndroombaState":
                case "RegisterAndroomba":
                case "AddAndrombaSolution":
                case "AddAndroombaState":
                case "AddAndroomba":
                    {
                        if (args[1] is not int itemID)
                            return new ArgumentException("ERROR: The first argument to \"RegisterAndroombaSolution\" must be the ID of a solution as an int.");
                        if (args[2] is not string texturePath)
                            return new ArgumentException("ERROR: The second argument to \"RegisterAndroombaSolution\" must be a string path.");
                        if (args[3] is not Action<NPC> NPCaction)
                            return new ArgumentException("ERROR: The third argument to \"RegisterAndroombaSolution\" must be an Action<NPC>.");

                        AndroombaFriendly.customConversionTypes.Add((itemID, texturePath, NPCaction));
                        return null;
                    }
                default:
                    return new ArgumentException("ERROR: Invalid method name.");
            }
        }
        #endregion
    }
}
