﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.Potions;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.NPCs.SupremeCalamitas
{
    public class SupremeCalamitas : ModNPC
    {
        internal enum FrameAnimationType
        {
            // The numbering of these values correspond to a frame on the sprite. If this enumeration or the sprite itself
            // is updated, these numbers will need to be too.
            UpwardDraft = 0,
            FasterUpwardDraft = 1,
            Casting = 2,
            BlastCast = 3,
            BlastPunchCast = 4,
            OutwardHandCast = 5,
            PunchHandCast = 6,
            Count = 7
        }

        private float bossLife;
        private float uDieLul = 1f;
        private float passedVar = 0f;

        private bool protectionBoost = false;
        private bool canDespawn = false;
        private bool despawnProj = false;
        private bool startText = false;
        private bool startBattle = false; //100%
        private bool hasSummonedSepulcher1 = false; //100%
        private bool startSecondAttack = false; //80%
        private bool startThirdAttack = false; //60%
        private bool halfLife = false; //40%
        private bool startFourthAttack = false; //30%
        private bool secondStage = false; //20%
        private bool startFifthAttack = false; //10%
        private bool gettingTired = false; //8%
        private bool hasSummonedSepulcher2 = false; //8%
        private bool gettingTired2 = false; //6%
        private bool gettingTired3 = false; //4%
        private bool gettingTired4 = false; //2%
        private bool gettingTired5 = false; //1%
        private bool willCharge = false;
        private bool canFireSplitingFireball = true;
        private bool spawnArena = false;
        private bool enteredBrothersPhase = false;
        private bool hasSummonedBrothers = false;

        private int giveUpCounter = 1200;
        private int phaseChange = 0;
        private int spawnX = 0;
        private int spawnX2 = 0;
        private int spawnXReset = 0;
        private int spawnXReset2 = 0;
        private int spawnXAdd = 200;
        private int spawnY = 0;
        private int spawnYReset = 0;
        private int spawnYAdd = 0;
        public int bulletHellCounter = 0;
        public int bulletHellCounter2 = 0;
        private int attackCastDelay = 0;
        private int hitTimer = 0;

        private float shieldOpacity = 1f;
        private float shieldRotation = 0f;
        private float forcefieldOpacity = 1f;
        private float forcefieldScale = 1;
        private FrameAnimationType FrameType
        {
            get => (FrameAnimationType)(int)NPC.localAI[2];
            set => NPC.localAI[2] = (int)value;
        }
        private bool AttackCloseToBeingOver
        {
            get
            {
                int attackLength = 0;

                // First phase.
                if (NPC.ai[0] == 0f)
                {
                    if (NPC.ai[1] == 0f)
                        attackLength = 300;
                    if (NPC.ai[1] == 2f)
                        attackLength = 70;
                    if (NPC.ai[1] == 3f)
                        attackLength = 480;
                    if (NPC.ai[1] == 4f)
                        attackLength = 300;
                }
                else
                {
                    if (NPC.ai[1] == 0f)
                        attackLength = 240;
                    if (NPC.ai[1] == 2f)
                        attackLength = 70;
                    if (NPC.ai[1] == 3f)
                        attackLength = 300;
                    if (NPC.ai[1] == 4f)
                        attackLength = 240;
                }
                return NPC.ai[2] >= attackLength - 30f;
            }
        }
        private ref float FrameChangeSpeed => ref NPC.localAI[3];

        private Vector2 cataclysmSpawnPosition;
        private Vector2 catastropheSpawnPosition;
        private Vector2 initialRitualPosition;
        private Rectangle safeBox = default;

        public static int hoodedHeadIconIndex;
        public static int hoodedHeadIconP2Index;
        public static int hoodlessHeadIconIndex;
        public static int hoodlessHeadIconP2Index;
        public static float normalDR = 0.25f;
        public static float enragedDR = 0.9999f;

        private static readonly Color textColor = Color.Orange;
        private const int sepulcherSpawnCastTime = 75;
        private const int brothersSpawnCastTime = 150;

        // TODO -- This is cumbersome. Change it to be better in 1.4.
        internal static void LoadHeadIcons()
        {
            string hoodedIconPath = "CalamityMod/NPCs/SupremeCalamitas/HoodedHeadIcon";
            string hoodedIconP2Path = "CalamityMod/NPCs/SupremeCalamitas/HoodedHeadIconP2";
            string hoodlessIconPath = "CalamityMod/NPCs/SupremeCalamitas/HoodlessHeadIcon";
            string hoodlessIconP2Path = "CalamityMod/NPCs/SupremeCalamitas/HoodlessHeadIconP2";
            CalamityMod.Instance.AddBossHeadTexture(hoodedIconPath, -1);
            hoodedHeadIconIndex = ModContent.GetModBossHeadSlot(hoodedIconPath);

            CalamityMod.Instance.AddBossHeadTexture(hoodedIconP2Path, -1);
            hoodedHeadIconP2Index = ModContent.GetModBossHeadSlot(hoodedIconP2Path);

            CalamityMod.Instance.AddBossHeadTexture(hoodlessIconPath, -1);
            hoodlessHeadIconIndex = ModContent.GetModBossHeadSlot(hoodlessIconPath);

            CalamityMod.Instance.AddBossHeadTexture(hoodlessIconP2Path, -1);
            hoodlessHeadIconP2Index = ModContent.GetModBossHeadSlot(hoodlessIconP2Path);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Supreme Calamitas");
            Main.npcFrameCount[NPC.type] = 21;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.GetNPCDamage();
            NPC.npcSlots = 50f;
            NPC.width = NPC.height = 44;
            NPC.defense = 100;
            NPC.DR_NERD(normalDR);
            NPC.value = Item.buyPrice(30, 0, 0, 0);
            NPC.LifeMaxNERB(960000, 1150000, 500000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = false;
            NPC.chaseable = true;
            NPC.boss = true;
            NPC.canGhostHeal = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            Music = CalamityMod.Instance.GetMusicFromMusicMod("SupremeCalamitas1") ?? MusicID.Boss2;
            bossBag = ModContent.ItemType<SCalBag>();
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
        }

        public override void BossHeadSlot(ref int index)
        {
            bool inPhase2 = NPC.ai[0] == 3f;
            if (!DownedBossSystem.downedSCal || BossRushEvent.BossRushActive)
                index = inPhase2 ? hoodedHeadIconP2Index : hoodedHeadIconIndex;
            else
                index = inPhase2 ? hoodlessHeadIconP2Index : hoodlessHeadIconIndex;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(protectionBoost);
            writer.Write(canDespawn);
            writer.Write(despawnProj);
            writer.Write(startText);
            writer.Write(startBattle);
            writer.Write(hasSummonedSepulcher1);
            writer.Write(startSecondAttack);
            writer.Write(startThirdAttack);
            writer.Write(startFourthAttack);
            writer.Write(startFifthAttack);
            writer.Write(halfLife);
            writer.Write(secondStage);
            writer.Write(hasSummonedSepulcher2);
            writer.Write(gettingTired);
            writer.Write(gettingTired2);
            writer.Write(gettingTired3);
            writer.Write(gettingTired4);
            writer.Write(gettingTired5);
            writer.Write(willCharge);
            writer.Write(canFireSplitingFireball);
            writer.Write(spawnArena);
            writer.Write(hasSummonedBrothers);
            writer.Write(enteredBrothersPhase);
            writer.Write(NPC.dontTakeDamage);
            writer.Write(NPC.chaseable);

            writer.Write(giveUpCounter);
            writer.Write(phaseChange);
            writer.Write(spawnX);
            writer.Write(spawnX2);
            writer.Write(spawnXReset);
            writer.Write(spawnXReset2);
            writer.Write(spawnXAdd);
            writer.Write(spawnY);
            writer.Write(spawnYReset);
            writer.Write(spawnYAdd);
            writer.Write(bulletHellCounter);
            writer.Write(bulletHellCounter2);
            writer.Write(hitTimer);
            writer.Write(attackCastDelay);

            writer.Write(shieldRotation);

            writer.Write(safeBox.X);
            writer.Write(safeBox.Y);
            writer.Write(safeBox.Width);
            writer.Write(safeBox.Height);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            protectionBoost = reader.ReadBoolean();
            canDespawn = reader.ReadBoolean();
            despawnProj = reader.ReadBoolean();
            startText = reader.ReadBoolean();
            startBattle = reader.ReadBoolean();
            hasSummonedSepulcher1 = reader.ReadBoolean();
            startSecondAttack = reader.ReadBoolean();
            startThirdAttack = reader.ReadBoolean();
            startFourthAttack = reader.ReadBoolean();
            startFifthAttack = reader.ReadBoolean();
            halfLife = reader.ReadBoolean();
            secondStage = reader.ReadBoolean();
            hasSummonedSepulcher2 = reader.ReadBoolean();
            gettingTired = reader.ReadBoolean();
            gettingTired2 = reader.ReadBoolean();
            gettingTired3 = reader.ReadBoolean();
            gettingTired4 = reader.ReadBoolean();
            gettingTired5 = reader.ReadBoolean();
            willCharge = reader.ReadBoolean();
            canFireSplitingFireball = reader.ReadBoolean();
            spawnArena = reader.ReadBoolean();
            hasSummonedBrothers = reader.ReadBoolean();
            enteredBrothersPhase = reader.ReadBoolean();
            NPC.dontTakeDamage = reader.ReadBoolean();
            NPC.chaseable = reader.ReadBoolean();

            giveUpCounter = reader.ReadInt32();
            phaseChange = reader.ReadInt32();
            spawnX = reader.ReadInt32();
            spawnX2 = reader.ReadInt32();
            spawnXReset = reader.ReadInt32();
            spawnXReset2 = reader.ReadInt32();
            spawnXAdd = reader.ReadInt32();
            spawnY = reader.ReadInt32();
            spawnYReset = reader.ReadInt32();
            spawnYAdd = reader.ReadInt32();
            bulletHellCounter = reader.ReadInt32();
            bulletHellCounter2 = reader.ReadInt32();
            hitTimer = reader.ReadInt32();
            attackCastDelay = reader.ReadInt32();

            shieldRotation = reader.ReadSingle();

            safeBox = new Rectangle(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        }

        public override void AI()
        {
            #region Resets

            // Use an ordinary upward draft by default.
            FrameType = FrameAnimationType.UpwardDraft;
            FrameChangeSpeed = 0.15f;
            #endregion
            #region StartUp

            CalamityGlobalNPC.SCal = NPC.whoAmI;

            bool wormAlive = false;
            if (CalamityGlobalNPC.SCalWorm != -1)
            {
                wormAlive = Main.npc[CalamityGlobalNPC.SCalWorm].active;
            }

            bool cataclysmAlive = false;
            if (CalamityGlobalNPC.SCalCataclysm != -1)
            {
                cataclysmAlive = Main.npc[CalamityGlobalNPC.SCalCataclysm].active;
            }

            bool catastropheAlive = false;
            if (CalamityGlobalNPC.SCalCatastrophe != -1)
            {
                catastropheAlive = Main.npc[CalamityGlobalNPC.SCalCatastrophe].active;
            }

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            CalamityMod.StopRain();

            bool malice = CalamityWorld.malice || BossRushEvent.BossRushActive;
            bool expertMode = Main.expertMode || BossRushEvent.BossRushActive;
            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

            // Projectile damage values
            int bulletHellblastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneHellblast2>());
            int firstBulletHellblastDamage = (int)Math.Round(bulletHellblastDamage * 1.25);
            int barrageDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneBarrage>());
            int gigablastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneGigaBlast>());
            int fireblastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneFireblast>());
            int monsterDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneMonster>());
            int waveDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneWave>());
            int hellblastDamage = NPC.GetProjectileDamage(ModContent.ProjectileType<BrimstoneHellblast>());
            int bodyWidth = 44;
            int bodyHeight = 42;
            int baseBulletHellProjectileGateValue = revenge ? 8 : expertMode ? 9 : 10;

            Vector2 vectorCenter = NPC.Center;

            // Get a target
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            // Despawn safety, make sure to target another player if the current player target is too far away
            if (Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) > CalamityGlobalNPC.CatchUpDistance200Tiles)
                NPC.TargetClosest();

            Player player = Main.player[NPC.target];

            if (!startText)
            {
                if (!BossRushEvent.BossRushActive)
                {
                    string key = "Mods.CalamityMod.SCalSummonText";
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                startText = true;
            }
            #endregion
            #region Directioning

            bool currentlyCharging = NPC.ai[1] == 2f;
            if (!currentlyCharging && Math.Abs(player.Center.X - NPC.Center.X) > 16f)
                NPC.spriteDirection = (player.Center.X < NPC.Center.X).ToDirectionInt();
            #endregion
            #region Forcefield and Shield Logic

            if (hitTimer > 0)
                hitTimer--;

            Vector2 hitboxSize = new Vector2(forcefieldScale * 216f / 1.4142f);
            hitboxSize = Vector2.Max(hitboxSize, new Vector2(42, 44));
            if (NPC.Size != hitboxSize)
                NPC.Size = hitboxSize;
            bool shouldNotUseShield = bulletHellCounter2 % 900 != 0 || attackCastDelay > 0 ||
                NPC.AnyNPCs(ModContent.NPCType<SupremeCataclysm>()) || NPC.AnyNPCs(ModContent.NPCType<SupremeCatastrophe>()) ||
                NPC.ai[0] == 1f || NPC.ai[0] == 2f;

            // Make the shield and forcefield fade away in SCal's acceptance phase.
            if (NPC.life <= NPC.lifeMax * 0.01)
            {
                shieldOpacity = MathHelper.Lerp(shieldOpacity, 0f, 0.08f);
                forcefieldScale = MathHelper.Lerp(forcefieldScale, 0f, 0.08f);
            }

            // Summon a shield if the next attack will be a charge.
            // Make it go away if certain triggers happen during this, such as a bullet hell starting, however.
            else if (((willCharge && AttackCloseToBeingOver) || NPC.ai[1] == 2f) && !shouldNotUseShield)
            {
                if (NPC.ai[1] != 2f)
                {
                    float idealRotation = NPC.AngleTo(player.Center);
                    float angularOffset = Math.Abs(MathHelper.WrapAngle(shieldRotation - idealRotation));

                    if (angularOffset > 0.04f)
                    {
                        shieldRotation = shieldRotation.AngleLerp(idealRotation, 0.125f);
                        shieldRotation = shieldRotation.AngleTowards(idealRotation, 0.18f);
                    }
                }
                else
                {
                    // Emit dust off the skull at the position of its eye socket.
                    for (float num6 = 1f; num6 < 16f; num6 += 1f)
                    {
                        Dust dust = Dust.NewDustPerfect(NPC.Center, 182);
                        dust.position = Vector2.Lerp(NPC.position, NPC.oldPosition, num6 / 16f) + NPC.Size * 0.5f;
                        dust.position += shieldRotation.ToRotationVector2() * 42f;
                        dust.position += (shieldRotation - MathHelper.PiOver2).ToRotationVector2() * (float)Math.Cos(NPC.velocity.ToRotation()) * -4f;
                        dust.noGravity = true;
                        dust.velocity = NPC.velocity;
                        dust.color = Color.Red;
                        dust.scale = MathHelper.Lerp(0.6f, 0.85f, 1f - num6 / 16f);
                    }
                }

                // Shrink the force-field since it looks strange when charging.
                forcefieldScale = MathHelper.Lerp(forcefieldScale, 0.45f, 0.08f);
                shieldOpacity = MathHelper.Lerp(shieldOpacity, 1f, 0.08f);
            }
            // Make the shield disappear if it is no longer relevant and regenerate the forcefield.
            else
            {
                shieldOpacity = MathHelper.Lerp(shieldOpacity, 0f, 0.08f);
                forcefieldScale = MathHelper.Lerp(forcefieldScale, 1f, 0.08f);
            }

            #endregion
            #region ArenaCreation

            // Create the arena on the first frame. This does not run client-side.
            // If this is done on the server, a sync must be performed so that the arena box is
            // known to the clients. Not doing this results in significant desyncs in regards to things like DR.
            if (!spawnArena)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (death)
                    {
                        safeBox.X = spawnX = spawnXReset = (int)(NPC.Center.X - 1000f);
                        spawnX2 = spawnXReset2 = (int)(NPC.Center.X + 1000f);
                        safeBox.Y = spawnY = spawnYReset = (int)(NPC.Center.Y - 1000f);
                        safeBox.Width = 2000;
                        safeBox.Height = 2000;
                        spawnYAdd = 100;
                    }
                    else
                    {
                        safeBox.X = spawnX = spawnXReset = (int)(NPC.Center.X - 1250f);
                        spawnX2 = spawnXReset2 = (int)(NPC.Center.X + 1250f);
                        safeBox.Y = spawnY = spawnYReset = (int)(NPC.Center.Y - 1250f);
                        safeBox.Width = 2500;
                        safeBox.Height = 2500;
                        spawnYAdd = 125;
                    }

                    int num52 = (int)(safeBox.X + (float)(safeBox.Width / 2)) / 16;
                    int num53 = (int)(safeBox.Y + (float)(safeBox.Height / 2)) / 16;
                    int num54 = safeBox.Width / 2 / 16 + 1;
                    for (int num55 = num52 - num54; num55 <= num52 + num54; num55++)
                    {
                        for (int num56 = num53 - num54; num56 <= num53 + num54; num56++)
                        {
                            if (!WorldGen.InWorld(num55, num56, 2))
                                continue;

                            if ((num55 == num52 - num54 || num55 == num52 + num54 || num56 == num53 - num54 || num56 == num53 + num54) && !Main.tile[num55, num56].HasTile)
                            {
                                Main.tile[num55, num56].TileType = (ushort)ModContent.TileType<Tiles.ArenaTile>();
                                Main.tile[num55, num56].Get<TileWallWireStateData>().HasTile = true;
                            }
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendTileSquare(-1, num55, num56, 1, TileChangeType.None);
                            }
                            else
                            {
                                WorldGen.SquareTileFrame(num55, num56, true);
                            }
                        }
                    }

                    if (initialRitualPosition == Vector2.Zero)
                    {
                        initialRitualPosition = NPC.Center + Vector2.UnitY * 24f;
                        NPC.netUpdate = true;
                    }

                    // Sync to update all clients on the state of the arena.
                    // Only after this will enrages be registered.
                    spawnArena = true;
                    NPC.netUpdate = true;
                }
            }
            #endregion
            #region Enrage and DR
            if ((spawnArena && !player.Hitbox.Intersects(safeBox)) || malice)
            {
                float projectileVelocityMultCap = !player.Hitbox.Intersects(safeBox) && spawnArena ? 2f : 1.5f;
                uDieLul = MathHelper.Clamp(uDieLul * 1.01f, 1f, projectileVelocityMultCap);
                protectionBoost = !malice;
            }
            else
            {
                uDieLul = MathHelper.Clamp(uDieLul * 0.99f, 1f, 2f);
                protectionBoost = false;
            }
            NPC.Calamity().CurrentlyEnraged = !player.Hitbox.Intersects(safeBox);

            // Set DR to be 99% and unbreakable if enraged. Boost DR during the 5th attack.
            CalamityGlobalNPC global = NPC.Calamity();
            if (protectionBoost && !gettingTired5)
            {
                global.DR = enragedDR;
                global.unbreakableDR = true;
            }
            else
            {
                global.DR = normalDR;
                global.unbreakableDR = false;
                if (startFifthAttack)
                    global.DR *= 1.2f;
            }
            #endregion
            #region Despawn
            if (!player.active || player.dead)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];

                // Slow down and disappear in a burst of fire if should despawn.
                if (!player.active || player.dead)
                {
                    canDespawn = true;

                    NPC.Opacity = MathHelper.Lerp(NPC.Opacity, 0f, 0.065f);
                    NPC.velocity = Vector2.Lerp(Vector2.UnitY * -4f, Vector2.Zero, (float)Math.Sin(MathHelper.Pi * NPC.Opacity));
                    forcefieldOpacity = Utils.GetLerpValue(0.1f, 0.6f, NPC.Opacity, true);
                    if (NPC.alpha >= 230)
                    {
                        if (DownedBossSystem.downedSCal && !BossRushEvent.BossRushActive)
                        {
                            // Create a teleport line effect
                            Dust.QuickDustLine(NPC.Center, initialRitualPosition, 500f, Color.Red);
                            NPC.Center = initialRitualPosition;

                            // Make the town NPC spawn.
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 12, ModContent.NPCType<WITCH>());
                        }

                        NPC.active = false;
                        NPC.netUpdate = true;
                    }

                    for (int i = 0; i < MathHelper.Lerp(2f, 6f, 1f - NPC.Opacity); i++)
                    {
                        Dust brimstoneFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-24f, 24f), DustID.Torch);
                        brimstoneFire.color = Color.Red;
                        brimstoneFire.velocity = Vector2.UnitY * -Main.rand.NextFloat(2f, 3.25f);
                        brimstoneFire.scale = Main.rand.NextFloat(0.95f, 1.15f);
                        brimstoneFire.noGravity = true;
                    }
                }
            }
            else
                canDespawn = false;
            #endregion
            #region Cast Charge Countdown
            if (attackCastDelay > 0)
            {
                attackCastDelay--;
                NPC.velocity *= 0.94f;
                NPC.damage = 0;
                NPC.dontTakeDamage = true;

                // Make a magic effect over time.
                for (int i = 0; i < (attackCastDelay == 0 ? 16 : 1); i++)
                {
                    Vector2 dustSpawnPosition = NPC.Bottom;
                    float horizontalSpawnOffset = bodyWidth * Main.rand.NextFloat(0.42f, 0.5f);
                    if (Main.rand.NextBool(2))
                        horizontalSpawnOffset *= -1f;
                    dustSpawnPosition.X += horizontalSpawnOffset;

                    Dust magic = Dust.NewDustPerfect(dustSpawnPosition, 267);
                    magic.color = Color.Lerp(Color.Red, Color.Orange, Main.rand.NextFloat(0.8f));
                    magic.noGravity = true;
                    magic.velocity = Vector2.UnitY * -Main.rand.NextFloat(5f, 9f);
                    magic.scale = 1f + NPC.velocity.Y * 0.35f;
                }

                if ((startBattle && !hasSummonedSepulcher1) || (gettingTired && !hasSummonedSepulcher2))
                    DoHeartsSpawningCastAnimation(player, death);

                if (enteredBrothersPhase && !hasSummonedBrothers)
                    DoBrothersSpawningCastAnimation(bodyWidth, bodyHeight);

                if (attackCastDelay == 0)
                {
                    NPC.dontTakeDamage = false;
                    NPC.netUpdate = true;
                }

                FrameType = FrameAnimationType.Casting;
                return;
            }
            #endregion
            #region FirstAttack
            if (bulletHellCounter2 < 900)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

                if (!canDespawn)
                    NPC.velocity *= 0.95f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    bulletHellCounter += 1;
                    if (bulletHellCounter >= baseBulletHellProjectileGateValue)
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 % (baseBulletHellProjectileGateValue * 6) == 0)
                        {
                            float distance = Main.rand.NextBool() ? -1000f : 1000f;
                            float velocity = (distance == -1000f ? 4f : -4f) * uDieLul;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        if (bulletHellCounter2 < 300) // Blasts from above
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else if (bulletHellCounter2 < 600) // Blasts from left and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else // Blasts from above, left, and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 3f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                    }
                }
                FrameType = FrameAnimationType.Casting;
                return;
            }
            else if (!startBattle)
            {
                attackCastDelay = sepulcherSpawnCastTime;
                for (int i = 0; i < 40; i++)
                {
                    Dust castFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-70f, 70f), (int)CalamityDusts.Brimstone);
                    castFire.velocity = Vector2.UnitY.RotatedByRandom(0.08f) * -Main.rand.NextFloat(3f, 4.45f);
                    castFire.scale = Main.rand.NextFloat(1.35f, 1.6f);
                    castFire.fadeIn = 1.25f;
                    castFire.noGravity = true;
                }

                NPC.Center = safeBox.TopRight() + new Vector2(-120f, 620f);

                for (int i = 0; i < 40; i++)
                {
                    Dust castFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-70f, 70f), (int)CalamityDusts.Brimstone);
                    castFire.velocity = Vector2.UnitY.RotatedByRandom(0.08f) * -Main.rand.NextFloat(3f, 4.45f);
                    castFire.scale = Main.rand.NextFloat(1.35f, 1.6f);
                    castFire.fadeIn = 1.25f;
                    castFire.noGravity = true;
                }

                SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, player.Center);
                startBattle = true;
            }
            #endregion
            #region SecondAttack
            if (bulletHellCounter2 < 1800 && startSecondAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

                if (!canDespawn)
                    NPC.velocity *= 0.95f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (bulletHellCounter2 < 1200)
                    {
                        if (bulletHellCounter2 % 180 == 0) // Blasts from top
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer);
                    }
                    else if (bulletHellCounter2 < 1500 && bulletHellCounter2 > 1200)
                    {
                        if (bulletHellCounter2 % 180 == 0) // Blasts from right
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer);
                    }
                    else if (bulletHellCounter2 > 1500)
                    {
                        if (bulletHellCounter2 % 180 == 0) // Blasts from top
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer);
                    }
                    bulletHellCounter += 1;
                    if (bulletHellCounter >= baseBulletHellProjectileGateValue + 1)
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 % ((baseBulletHellProjectileGateValue + 1) * 6) == 0)
                        {
                            float distance = Main.rand.NextBool() ? -1000f : 1000f;
                            float velocity = (distance == -1000f ? 4f : -4f) * uDieLul;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        if (bulletHellCounter2 < 1200) // Blasts from below
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y + 1000f, 0f, -4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else if (bulletHellCounter2 < 1500) // Blasts from left
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else // Blasts from left and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                    }
                }
                FrameType = FrameAnimationType.Casting;
                return;
            }
            if (!startSecondAttack && (NPC.life <= NPC.lifeMax * 0.75))
            {
                if (!BossRushEvent.BossRushActive)
                {
                    string key = "Mods.CalamityMod.SCalBH2Text";
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                startSecondAttack = true;
                return;
            }
            #endregion
            #region ThirdAttack
            if (bulletHellCounter2 < 2700 && startThirdAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

                if (!canDespawn)
                    NPC.velocity *= 0.95f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (bulletHellCounter2 % 180 == 0) // Blasts from top
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer);

                    if (bulletHellCounter2 % 240 == 0) // Fireblasts from above
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 10f * uDieLul, ModContent.ProjectileType<BrimstoneFireblast>(), fireblastDamage, 0f, Main.myPlayer);

                    bulletHellCounter += 1;
                    if (bulletHellCounter >= baseBulletHellProjectileGateValue + 4)
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 % ((baseBulletHellProjectileGateValue + 4) * 6) == 0)
                        {
                            float distance = Main.rand.NextBool() ? -1000f : 1000f;
                            float velocity = (distance == -1000f ? 4f : -4f) * uDieLul;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        if (bulletHellCounter2 < 2100) // Blasts from above
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else if (bulletHellCounter2 < 2400) // Blasts from right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else // Blasts from left and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                    }
                }
                FrameType = FrameAnimationType.Casting;
                return;
            }
            if (!startThirdAttack && (NPC.life <= NPC.lifeMax * 0.5))
            {
                // Switch from the Grief section of Stained, Brutal Calamity to the Lament section.
                Music = CalamityMod.Instance.GetMusicFromMusicMod("SupremeCalamitas2") ?? MusicID.Boss3;

                if (!BossRushEvent.BossRushActive)
                {
                    string key = "Mods.CalamityMod.SCalBH3Text";
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                startThirdAttack = true;
                return;
            }
            #endregion
            #region FourthAttack
            if (bulletHellCounter2 < 3600 && startFourthAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

                if (!canDespawn)
                    NPC.velocity *= 0.95f;

                if (Main.netMode != NetmodeID.MultiplayerClient) // More clustered attack
                {
                    if (bulletHellCounter2 % 180 == 0) // Blasts from top
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer);

                    if (bulletHellCounter2 % 240 == 0) // Fireblasts from above
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 10f * uDieLul, ModContent.ProjectileType<BrimstoneFireblast>(), fireblastDamage, 0f, Main.myPlayer);

                    int divisor = revenge ? 225 : expertMode ? 450 : 675;

                    // TODO -- Resprite Brimstone Monsters to be something else. WIPs posted by Iban.
                    if (bulletHellCounter2 % divisor == 0 && expertMode) // Giant homing fireballs
                    {
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 1f * uDieLul, ModContent.ProjectileType<BrimstoneMonster>(), monsterDamage, 0f, Main.myPlayer, 0f, passedVar);
                        passedVar += 1f;
                    }

                    bulletHellCounter += 1;
                    if (bulletHellCounter >= baseBulletHellProjectileGateValue + 6)
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 % ((baseBulletHellProjectileGateValue + 6) * 6) == 0)
                        {
                            float distance = Main.rand.NextBool() ? -1000f : 1000f;
                            float velocity = (distance == -1000f ? 4f : -4f) * uDieLul;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        if (bulletHellCounter2 < 3000) // Blasts from below
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y + 1000f, 0f, -4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else if (bulletHellCounter2 < 3300) // Blasts from left
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else // Blasts from left and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                    }
                }
                FrameType = FrameAnimationType.Casting;
                return;
            }
            if (!startFourthAttack && (NPC.life <= NPC.lifeMax * 0.3))
            {
                // Switch from the Lament section of Stained, Brutal Calamity to the Epiphany section.
                Music = CalamityMod.Instance.GetMusicFromMusicMod("SupremeCalamitas3") ?? MusicID.LunarBoss;

                if (!BossRushEvent.BossRushActive)
                {
                    string key = "Mods.CalamityMod.SCalBH4Text";
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                startFourthAttack = true;
                return;
            }
            #endregion
            #region FifthAttack
            if (bulletHellCounter2 < 4500 && startFifthAttack)
            {
                despawnProj = true;
                bulletHellCounter2 += 1;
                NPC.damage = 0;
                NPC.chaseable = false;
                NPC.dontTakeDamage = true;

                if (!canDespawn)
                    NPC.velocity *= 0.95f;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (bulletHellCounter2 % 240 == 0) // Blasts from top
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f * uDieLul, ModContent.ProjectileType<BrimstoneGigaBlast>(), gigablastDamage, 0f, Main.myPlayer);

                    if (bulletHellCounter2 % 360 == 0) // Fireblasts from above
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 10f * uDieLul, ModContent.ProjectileType<BrimstoneFireblast>(), fireblastDamage, 0f, Main.myPlayer);

                    if (bulletHellCounter2 % 30 == 0) // Projectiles that move in wave pattern
                    {
                        int random = Main.rand.Next(-500, 501);
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + random, -5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneWave>(), waveDamage, 0f, Main.myPlayer);
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y - random, 5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneWave>(), waveDamage, 0f, Main.myPlayer);
                    }

                    bulletHellCounter += 1;
                    if (bulletHellCounter >= baseBulletHellProjectileGateValue + 8)
                    {
                        bulletHellCounter = 0;
                        if (bulletHellCounter2 % ((baseBulletHellProjectileGateValue + 8) * 6) == 0)
                        {
                            float distance = Main.rand.NextBool() ? -1000f : 1000f;
                            float velocity = (distance == -1000f ? 4f : -4f) * uDieLul;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + distance, player.position.Y, velocity, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), firstBulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        if (bulletHellCounter2 < 3900) // Blasts from above
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 4f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else if (bulletHellCounter2 < 4200) // Blasts from left and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3.5f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                        else // Blasts from above, left, and right
                        {
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 3f * uDieLul, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), player.position.X - 1000f, player.position.Y + Main.rand.Next(-1000, 1001), 3f * uDieLul, 0f, ModContent.ProjectileType<BrimstoneHellblast2>(), bulletHellblastDamage, 0f, Main.myPlayer);
                        }
                    }
                }
                FrameType = FrameAnimationType.Casting;
                return;
            }
            if (!startFifthAttack && (NPC.life <= NPC.lifeMax * 0.1))
            {
                string key = "Mods.CalamityMod.SCalBH5Text";

                if (!BossRushEvent.BossRushActive)
                {
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                startFifthAttack = true;
                return;
            }
            #endregion
            #region EndSections
            if (startFifthAttack)
            {
                if (gettingTired5)
                {
                    // Switch from the Epiphany section of Stained, Brutal Calamity to the Acceptance section.
                    Music = CalamityMod.Instance.GetMusicFromMusicMod("SupremeCalamitas4") ?? MusicID.Eerie;

                    if (NPC.velocity.Y < 9f)
                        NPC.velocity.Y += 0.185f;
                    NPC.noTileCollide = false;
                    NPC.noGravity = false;
                    NPC.damage = 0;

                    if (!canDespawn)
                        NPC.velocity.X *= 0.96f;

                    if (DownedBossSystem.downedSCal && !BossRushEvent.BossRushActive)
                    {
                        if (giveUpCounter == 720)
                        {
                            for (int i = 0; i < 24; i++)
                            {
                                Dust brimstoneFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-24f, 24f), DustID.Torch);
                                brimstoneFire.color = Color.Red;
                                brimstoneFire.velocity = Vector2.UnitY * -Main.rand.NextFloat(2f, 3.25f);
                                brimstoneFire.scale = Main.rand.NextFloat(0.95f, 1.15f);
                                brimstoneFire.fadeIn = 1.25f;
                                brimstoneFire.noGravity = true;
                            }

                            NPC.active = false;
                            NPC.netUpdate = true;
                            NPCLoot();
                        }
                    }
                    else if (giveUpCounter == 900 && !BossRushEvent.BossRushActive)
                        CalamityUtils.DisplayLocalizedText("Mods.CalamityMod.SCalAcceptanceText1", textColor);
                    else if(giveUpCounter == 600 && !BossRushEvent.BossRushActive)
                        CalamityUtils.DisplayLocalizedText("Mods.CalamityMod.SCalAcceptanceText2", textColor);
                    else if(giveUpCounter == 300 && !BossRushEvent.BossRushActive)
                        CalamityUtils.DisplayLocalizedText("Mods.CalamityMod.SCalAcceptanceText3", textColor);
                    if (giveUpCounter <= 0)
                    {
                        if (BossRushEvent.BossRushActive)
                        {
                            NPC.chaseable = true;
                            NPC.dontTakeDamage = false;
                            return;
                        }

                        for (int i = 0; i < 24; i++)
                        {
                            Dust brimstoneFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-24f, 24f), DustID.Torch);
                            brimstoneFire.color = Color.Red;
                            brimstoneFire.velocity = Vector2.UnitY * -Main.rand.NextFloat(2f, 3.25f);
                            brimstoneFire.scale = Main.rand.NextFloat(0.95f, 1.15f);
                            brimstoneFire.fadeIn = 1.25f;
                            brimstoneFire.noGravity = true;
                        }

                        NPC.active = false;
                        NPC.netUpdate = true;
                        NPCLoot();
                        return;
                    }
                    giveUpCounter--;
                    NPC.chaseable = false;
                    NPC.dontTakeDamage = true;
                    return;
                }
                if (!gettingTired5 && (NPC.life <= NPC.lifeMax * 0.01))
                {
                    for (int x = 0; x < Main.maxProjectiles; x++)
                    {
                        Projectile projectile = Main.projectile[x];
                        if (projectile.active && projectile.type == ModContent.ProjectileType<BrimstoneMonster>())
                        {
                            if (projectile.timeLeft > 90)
                                projectile.timeLeft = 90;
                        }
                    }

                    if (!BossRushEvent.BossRushActive)
                    {
                        string key = "Mods.CalamityMod.SCalDesparationText4";
                        if (DownedBossSystem.downedSCal)
                            key += "Rematch";
                        CalamityUtils.DisplayLocalizedText(key, textColor);
                    }
                    gettingTired5 = true;
                    return;
                }
                else if (!gettingTired4 && (NPC.life <= NPC.lifeMax * 0.02))
                {
                    if (!BossRushEvent.BossRushActive)
                    {
                        string key = "Mods.CalamityMod.SCalDesparationText3";
                        if (DownedBossSystem.downedSCal)
                            key += "Rematch";
                        CalamityUtils.DisplayLocalizedText(key, textColor);
                    }
                    gettingTired4 = true;
                    return;
                }
                else if (!gettingTired3 && (NPC.life <= NPC.lifeMax * 0.04))
                {
                    if (!BossRushEvent.BossRushActive)
                    {
                        string key = "Mods.CalamityMod.SCalDesparationText2";
                        if (DownedBossSystem.downedSCal)
                            key += "Rematch";
                        CalamityUtils.DisplayLocalizedText(key, textColor);
                    }
                    gettingTired3 = true;
                    return;
                }
                else if (!gettingTired2 && (NPC.life <= NPC.lifeMax * 0.06))
                {
                    if (!BossRushEvent.BossRushActive)
                    {
                        string key = "Mods.CalamityMod.SCalDesparationText1";
                        if (DownedBossSystem.downedSCal)
                            key += "Rematch";
                        CalamityUtils.DisplayLocalizedText(key, textColor);
                    }
                    gettingTired2 = true;
                    return;
                }
                else if (!gettingTired && (NPC.life <= NPC.lifeMax * 0.08))
                {
                    attackCastDelay = sepulcherSpawnCastTime;
                    for (int i = 0; i < 40; i++)
                    {
                        Dust castFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-70f, 70f), (int)CalamityDusts.Brimstone);
                        castFire.velocity = Vector2.UnitY.RotatedByRandom(0.08f) * -Main.rand.NextFloat(3f, 4.45f);
                        castFire.scale = Main.rand.NextFloat(1.35f, 1.6f);
                        castFire.fadeIn = 1.25f;
                        castFire.noGravity = true;
                    }

                    NPC.Center = safeBox.TopRight() + new Vector2(-120f, 620f);

                    for (int i = 0; i < 40; i++)
                    {
                        Dust castFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-70f, 70f), (int)CalamityDusts.Brimstone);
                        castFire.velocity = Vector2.UnitY.RotatedByRandom(0.08f) * -Main.rand.NextFloat(3f, 4.45f);
                        castFire.scale = Main.rand.NextFloat(1.35f, 1.6f);
                        castFire.fadeIn = 1.25f;
                        castFire.noGravity = true;
                    }

                    SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, player.Center);
                    gettingTired = true;
                    return;
                }
            }
            #endregion
            #region DespawnProjectiles
            if (bulletHellCounter2 % 900 == 0 && despawnProj)
            {
                for (int x = 0; x < Main.maxProjectiles; x++)
                {
                    Projectile projectile = Main.projectile[x];
                    if (projectile.active)
                    {
                        if (projectile.type == ModContent.ProjectileType<BrimstoneHellblast2>() ||
                            projectile.type == ModContent.ProjectileType<BrimstoneBarrage>() ||
                            projectile.type == ModContent.ProjectileType<BrimstoneWave>())
                        {
                            if (projectile.timeLeft > 60)
                                projectile.timeLeft = 60;
                        }
                        else if (projectile.type == ModContent.ProjectileType<BrimstoneGigaBlast>() || projectile.type == ModContent.ProjectileType<BrimstoneFireblast>())
                        {
                            projectile.ai[1] = 1f;

                            if (projectile.timeLeft > 60)
                                projectile.timeLeft = 60;
                        }
                    }
                }
                despawnProj = false;
            }
            #endregion
            #region TransformSeekerandBrotherTriggers
            if (!halfLife && (NPC.life <= NPC.lifeMax * 0.4))
            {
                if (!BossRushEvent.BossRushActive)
                {
                    string key = "Mods.CalamityMod.SCalPhase2Text";
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                halfLife = true;
            }

            if (NPC.life <= NPC.lifeMax * 0.2)
            {
                if (!secondStage)
                {
                    if (!BossRushEvent.BossRushActive)
                    {
                        string key = "Mods.CalamityMod.SCalSeekerRingText";
                        if (DownedBossSystem.downedSCal)
                            key += "Rematch";
                        CalamityUtils.DisplayLocalizedText(key, textColor);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        SoundEngine.PlaySound(SoundID.Item74, NPC.position);
                        for (int I = 0; I < 20; I++)
                        {
                            int FireEye = NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), (int)(vectorCenter.X + (Math.Sin(I * 18) * 300)), (int)(vectorCenter.Y + (Math.Cos(I * 18) * 300)), ModContent.NPCType<SoulSeekerSupreme>(), NPC.whoAmI, 0, 0, 0, -1);
                            NPC Eye = Main.npc[FireEye];
                            Eye.ai[0] = I * 18;
                            Eye.ai[3] = I * 18;
                        }
                    }
                    SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, player.Center);
                    secondStage = true;
                }
            }

            if (bossLife == 0f && NPC.life > 0)
            {
                bossLife = NPC.lifeMax;
            }
            if (NPC.life > 0)
            {
                if (NPC.life < NPC.lifeMax * 0.45f && !enteredBrothersPhase)
                {
                    enteredBrothersPhase = true;
                    attackCastDelay = brothersSpawnCastTime;
                    NPC.netUpdate = true;
                }
            }

            #endregion
            #region FirstStage
            if (NPC.ai[0] == 0f)
            {
                NPC.damage = NPC.defDamage;
                if (wormAlive)
                {
                    NPC.dontTakeDamage = true;
                    NPC.chaseable = false;
                }
                else
                {
                    if (cataclysmAlive || catastropheAlive)
                    {
                        NPC.dontTakeDamage = true;
                        NPC.chaseable = false;
                        NPC.damage = 0;

                        if (!canDespawn)
                            NPC.velocity *= 0.95f;
                        return;
                    }
                    else
                    {
                        NPC.dontTakeDamage = false;
                        NPC.chaseable = true;
                    }
                }

                if (NPC.ai[1] == -1f)
                {
                    phaseChange++;
                    if (phaseChange > 23)
                        phaseChange = 0;

                    int phase = 0; //0 = shots above 1 = charge 2 = nothing 3 = hellblasts 4 = fireblasts
                    switch (phaseChange)
                    {
                        case 0:
                            phase = 0;
                            willCharge = false;
                            break; //0341
                        case 1:
                            phase = 3;
                            break;
                        case 2:
                            phase = 4;
                            willCharge = true;
                            break;
                        case 3:
                            phase = 1;
                            break;
                        case 4:
                            phase = 1;
                            break; //1430
                        case 5:
                            phase = 4;
                            willCharge = false;
                            break;
                        case 6:
                            phase = 3;
                            break;
                        case 7:
                            phase = 0;
                            willCharge = true;
                            break;
                        case 8:
                            phase = 1;
                            break; //1034
                        case 9:
                            phase = 0;
                            willCharge = false;
                            break;
                        case 10:
                            phase = 3;
                            break;
                        case 11:
                            phase = 4;
                            break;
                        case 12:
                            phase = 4;
                            break; //4310
                        case 13:
                            phase = 3;
                            willCharge = true;
                            break;
                        case 14:
                            phase = 1;
                            break;
                        case 15:
                            phase = 0;
                            willCharge = false;
                            break;
                        case 16:
                            phase = 4;
                            break; //4411
                        case 17:
                            phase = 4;
                            willCharge = true;
                            break;
                        case 18:
                            phase = 1;
                            break;
                        case 19:
                            phase = 1;
                            break;
                        case 20:
                            phase = 0;
                            break; //0101
                        case 21:
                            phase = 1;
                            break;
                        case 22:
                            phase = 0;
                            break;
                        case 23:
                            phase = 1;
                            break;
                    }

                    NPC.ai[1] = phase;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                }
                else
                {
                    if (NPC.ai[1] == 0f)
                    {
                        float num823 = 12f;
                        float num824 = 0.12f;

                        // Reduce acceleration if target is holding a true melee weapon
                        Item targetSelectedItem = player.inventory[player.selectedItem];
                        if (targetSelectedItem.DamageType == DamageClass.Melee && (targetSelectedItem.shoot == ProjectileID.None || targetSelectedItem.Calamity().trueMelee))
                        {
                            num824 *= 0.5f;
                        }

                        Vector2 vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num825 = player.position.X + (player.width / 2) - vector82.X;
                        float num826 = player.position.Y + (player.height / 2) - 550f - vector82.Y;
                        float num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);

                        num827 = num823 / num827;
                        num825 *= num827;
                        num826 *= num827;

                        if (!canDespawn)
                        {
                            if (NPC.velocity.X < num825)
                            {
                                NPC.velocity.X += num824;
                                if (NPC.velocity.X < 0f && num825 > 0f)
                                    NPC.velocity.X += num824;
                            }
                            else if (NPC.velocity.X > num825)
                            {
                                NPC.velocity.X -= num824;
                                if (NPC.velocity.X > 0f && num825 < 0f)
                                    NPC.velocity.X -= num824;
                            }
                            if (NPC.velocity.Y < num826)
                            {
                                NPC.velocity.Y += num824;
                                if (NPC.velocity.Y < 0f && num826 > 0f)
                                    NPC.velocity.Y += num824;
                            }
                            else if (NPC.velocity.Y > num826)
                            {
                                NPC.velocity.Y -= num824;
                                if (NPC.velocity.Y > 0f && num826 < 0f)
                                    NPC.velocity.Y -= num824;
                            }
                        }

                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 300f)
                        {
                            NPC.ai[1] = -1f;
                            NPC.TargetClosest();
                            NPC.netUpdate = true;
                        }

                        vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        num825 = player.position.X + (player.width / 2) - vector82.X;
                        num826 = player.position.Y + (player.height / 2) - vector82.Y;

                        NPC.localAI[1] += wormAlive ? 0.5f : 1f;
                        if (NPC.localAI[1] > 90f)
                        {
                            NPC.localAI[1] = 0f;

                            float num828 = 10f * uDieLul;
                            Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                            float num180 = player.position.X + player.width * 0.5f - value9.X;
                            float num181 = Math.Abs(num180) * 0.1f;
                            float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
                            float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);

                            num183 = num828 / num183;
                            num180 *= num183;
                            num182 *= num183;
                            value9.X += num180;
                            value9.Y += num182;

                            int randomShot = Main.rand.Next(6);
                            if (randomShot == 0 && canFireSplitingFireball)
                            {
                                canFireSplitingFireball = false;
                                randomShot = ModContent.ProjectileType<BrimstoneFireblast>();
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
                                num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
                                num827 = num828 / num827;
                                num825 *= num827;
                                num826 *= num827;
                                vector82.X += num825 * 8f;
                                vector82.Y += num826 * 8f;

                                for (int i = 0; i < 15; i++)
                                {
                                    Dust magic = Dust.NewDustPerfect(value9, 264);
                                    magic.velocity = new Vector2(num180, num182).RotatedByRandom(0.36f) * Main.rand.NextFloat(0.9f, 1.1f);
                                    magic.color = Color.OrangeRed;
                                    magic.scale = 1.2f;
                                    magic.fadeIn = 0.6f;
                                    magic.noLight = true;
                                    magic.noGravity = true;
                                }

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector82.X, vector82.Y, num825, num826, randomShot, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
                                    NPC.netUpdate = true;
                                }
                            }
                            else if (randomShot == 1 && canFireSplitingFireball)
                            {
                                canFireSplitingFireball = false;
                                randomShot = ModContent.ProjectileType<BrimstoneGigaBlast>();
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneShoot"), NPC.Center);
                                num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
                                num827 = num828 / num827;
                                num825 *= num827;
                                num826 *= num827;
                                vector82.X += num825 * 8f;
                                vector82.Y += num826 * 8f;

                                for (int i = 0; i < 20; i++)
                                {
                                    Dust magic = Dust.NewDustPerfect(value9, (int)CalamityDusts.Brimstone);
                                    magic.velocity = new Vector2(num180, num182).RotatedByRandom(0.36f) * Main.rand.NextFloat(0.9f, 1.1f) * 1.3f;
                                    magic.color = Color.Red;
                                    magic.scale = 1.425f;
                                    magic.fadeIn = 0.75f;
                                    magic.noLight = true;
                                    magic.noGravity = true;
                                }

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector82.X, vector82.Y, num825, num826, randomShot, gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
                                    NPC.netUpdate = true;
                                }
                            }
                            else
                            {
                                canFireSplitingFireball = true;
                                randomShot = ModContent.ProjectileType<BrimstoneBarrage>();
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
                                for (int num186 = 0; num186 < 8; num186++)
                                {
                                    num180 = player.position.X + player.width * 0.5f - value9.X;
                                    num182 = player.position.Y + player.height * 0.5f - value9.Y;
                                    num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
                                    float speedBoost = num186 > 3 ? -(num186 - 3) : num186;
                                    num183 = (8f + speedBoost) / num183;
                                    num180 *= num183;
                                    num182 *= num183;

                                    for (int i = 0; i < 7; i++)
                                    {
                                        Vector2 magicDustVelocity = new Vector2(num180 + speedBoost, num182 + speedBoost);
                                        magicDustVelocity *= MathHelper.Lerp(0.3f, 1f, i / 7f);

                                        Dust magic = Dust.NewDustPerfect(value9, 264);
                                        magic.velocity = magicDustVelocity;
                                        magic.color = Color.Red;
                                        magic.scale = MathHelper.Lerp(0.85f, 1.5f, i / 7f);
                                        magic.fadeIn = 0.67f;
                                        magic.noLight = true;
                                        magic.noGravity = true;
                                    }

                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), value9.X, value9.Y, num180 + speedBoost, num182 + speedBoost, randomShot, barrageDamage, 0f, Main.myPlayer, 0f, 0f);
                                        NPC.netUpdate = true;
                                    }
                                }
                            }
                        }

                        FrameType = FrameAnimationType.FasterUpwardDraft;
                    }
                    else if (NPC.ai[1] == 1f)
                    {
                        float num383 = wormAlive ? 26f : 30f;
                        if (NPC.life < NPC.lifeMax * 0.95)
                            num383 += 1f;
                        if (NPC.life < NPC.lifeMax * 0.85)
                            num383 += 1f;
                        if (NPC.life < NPC.lifeMax * 0.7)
                            num383 += 1f;
                        if (NPC.life < NPC.lifeMax * 0.6)
                            num383 += 1f;
                        if (NPC.life < NPC.lifeMax * 0.5)
                            num383 += 1f;

                        Vector2 vector37 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num384 = player.position.X + (player.width / 2) - vector37.X;
                        float num385 = player.position.Y + (player.height / 2) - vector37.Y;
                        float num386 = (float)Math.Sqrt(num384 * num384 + num385 * num385);
                        num386 = num383 / num386;

                        if (!canDespawn)
                        {
                            NPC.velocity.X = num384 * num386;
                            NPC.velocity.Y = num385 * num386;
                            shieldRotation = NPC.velocity.ToRotation();
                            NPC.netUpdate = true;

                            SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/SCalDash"), NPC.Center);
                        }

                        NPC.ai[1] = 2f;
                    }
                    else if (NPC.ai[1] == 2f)
                    {
                        NPC.ai[2] += 1f;

                        if (Math.Abs(NPC.velocity.X) > 0.15f)
                            NPC.spriteDirection = (NPC.velocity.X < 0f).ToDirectionInt();
                        if (NPC.ai[2] >= 25f)
                        {
                            if (!canDespawn)
                            {
                                NPC.velocity *= 0.96f;

                                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                                    NPC.velocity.X = 0f;
                                if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                                    NPC.velocity.Y = 0f;
                            }
                        }

                        bool willChargeAgain = NPC.ai[3] + 1 < 2;

                        if (NPC.ai[2] >= 70f)
                        {
                            NPC.ai[3] += 1f;
                            NPC.ai[2] = 0f;
                            NPC.TargetClosest();

                            if (!willChargeAgain)
                                NPC.ai[1] = -1f;
                            else
                                NPC.ai[1] = 1f;
                        }

                        if (willChargeAgain && NPC.ai[2] > 50f)
                        {
                            float idealRotation = NPC.AngleTo(player.Center);
                            shieldRotation = shieldRotation.AngleLerp(idealRotation, 0.125f);
                            shieldRotation = shieldRotation.AngleTowards(idealRotation, 0.18f);
                        }

                        FrameType = FrameAnimationType.FasterUpwardDraft;
                    }
                    else if (NPC.ai[1] == 3f)
                    {
                        float num412 = 32f;
                        float num413 = 1.2f;

                        // Reduce acceleration if target is holding a true melee weapon
                        Item targetSelectedItem = player.inventory[player.selectedItem];
                        if (targetSelectedItem.DamageType == DamageClass.Melee && (targetSelectedItem.shoot == ProjectileID.None || targetSelectedItem.Calamity().trueMelee))
                        {
                            num413 *= 0.5f;
                        }

                        int num414 = 1;
                        if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
                            num414 = -1;

                        Vector2 handPosition = NPC.Center + new Vector2(NPC.spriteDirection * -18f, 2f);
                        Vector2 vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num415 = player.position.X + (player.width / 2) + (num414 * 600) - vector40.X;
                        float num416 = player.position.Y + (player.height / 2) - vector40.Y;
                        float num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);

                        num417 = num412 / num417;
                        num415 *= num417;
                        num416 *= num417;

                        if (!canDespawn)
                        {
                            if (NPC.velocity.X < num415)
                            {
                                NPC.velocity.X += num413;
                                if (NPC.velocity.X < 0f && num415 > 0f)
                                    NPC.velocity.X += num413;
                            }
                            else if (NPC.velocity.X > num415)
                            {
                                NPC.velocity.X -= num413;
                                if (NPC.velocity.X > 0f && num415 < 0f)
                                    NPC.velocity.X -= num413;
                            }
                            if (NPC.velocity.Y < num416)
                            {
                                NPC.velocity.Y += num413;
                                if (NPC.velocity.Y < 0f && num416 > 0f)
                                    NPC.velocity.Y += num413;
                            }
                            else if (NPC.velocity.Y > num416)
                            {
                                NPC.velocity.Y -= num413;
                                if (NPC.velocity.Y > 0f && num416 < 0f)
                                    NPC.velocity.Y -= num413;
                            }
                        }

                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 480f)
                        {
                            NPC.ai[1] = -1f;
                            NPC.TargetClosest();
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            if (!player.dead)
                                NPC.ai[3] += wormAlive ? 0.5f : 1f;

                            if (NPC.ai[3] >= 20f)
                            {
                                NPC.ai[3] = 0f;
                                vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                                num415 = player.position.X + (player.width / 2) - vector40.X;
                                num416 = player.position.Y + (player.height / 2) - vector40.Y;
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneHellblastSound"), NPC.Center);

                                // Release a burst of magic dust along with a brimstone hellblast skull.
                                for (int i = 0; i < 25; i++)
                                {
                                    Dust brimstoneMagic = Dust.NewDustPerfect(handPosition, 264);
                                    brimstoneMagic.velocity = NPC.DirectionTo(player.Center).RotatedByRandom(0.31f) * Main.rand.NextFloat(3f, 5f) + NPC.velocity;
                                    brimstoneMagic.scale = Main.rand.NextFloat(1.25f, 1.35f);
                                    brimstoneMagic.noGravity = true;
                                    brimstoneMagic.color = Color.OrangeRed;
                                    brimstoneMagic.fadeIn = 1.5f;
                                    brimstoneMagic.noLight = true;
                                }

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    float num418 = 10f * uDieLul;
                                    int num420 = ModContent.ProjectileType<BrimstoneHellblast>();
                                    num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);
                                    num417 = num418 / num417;
                                    num415 *= num417;
                                    num416 *= num417;
                                    vector40.X += num415 * 2f;
                                    vector40.Y += num416 * 2f;
                                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector40.X, vector40.Y, num415, num416, num420, hellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                                }
                            }
                        }

                        if (Main.rand.NextBool(2))
                        {
                            Dust brimstoneMagic = Dust.NewDustPerfect(handPosition, 264);
                            brimstoneMagic.velocity = Vector2.UnitY.RotatedByRandom(0.14f) * Main.rand.NextFloat(-3.5f, -3f) + NPC.velocity;
                            brimstoneMagic.scale = Main.rand.NextFloat(1.25f, 1.35f);
                            brimstoneMagic.noGravity = true;
                            brimstoneMagic.noLight = true;
                        }

                        FrameType = FrameAnimationType.OutwardHandCast;
                    }
                    else if (NPC.ai[1] == 4f)
                    {
                        int num831 = 1;
                        if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
                            num831 = -1;

                        float num832 = 32f;
                        float num833 = 1.2f;

                        // Reduce acceleration if target is holding a true melee weapon
                        Item targetSelectedItem = player.HeldItem;
                        if (targetSelectedItem.DamageType == DamageClass.Melee && (targetSelectedItem.shoot == ProjectileID.None || targetSelectedItem.Calamity().trueMelee))
                        {
                            num833 *= 0.5f;
                        }

                        Vector2 vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num834 = player.position.X + (player.width / 2) + (num831 * 750) - vector83.X; //600
                        float num835 = player.position.Y + (player.height / 2) - vector83.Y;
                        float num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);

                        num836 = num832 / num836;
                        num834 *= num836;
                        num835 *= num836;

                        if (!canDespawn)
                        {
                            if (NPC.velocity.X < num834)
                            {
                                NPC.velocity.X += num833;
                                if (NPC.velocity.X < 0f && num834 > 0f)
                                    NPC.velocity.X += num833;
                            }
                            else if (NPC.velocity.X > num834)
                            {
                                NPC.velocity.X -= num833;
                                if (NPC.velocity.X > 0f && num834 < 0f)
                                    NPC.velocity.X -= num833;
                            }
                            if (NPC.velocity.Y < num835)
                            {
                                NPC.velocity.Y += num833;
                                if (NPC.velocity.Y < 0f && num835 > 0f)
                                    NPC.velocity.Y += num833;
                            }
                            else if (NPC.velocity.Y > num835)
                            {
                                NPC.velocity.Y -= num833;
                                if (NPC.velocity.Y > 0f && num835 < 0f)
                                    NPC.velocity.Y -= num833;
                            }
                        }

                        vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        num834 = player.position.X + (player.width / 2) - vector83.X;
                        num835 = player.position.Y + (player.height / 2) - vector83.Y;

                        int shootRate = wormAlive ? 280 : 140;
                        NPC.localAI[1]++;
                        FrameChangeSpeed = 0.175f;
                        FrameType = FrameAnimationType.BlastCast;

                        if (NPC.localAI[1] > shootRate)
                        {
                            Vector2 handPosition = NPC.Center + new Vector2(NPC.spriteDirection * -22f, 2f);

                            // Release a burst of magic dust when punching.
                            for (int i = 0; i < 25; i++)
                            {
                                Dust brimstoneMagic = Dust.NewDustPerfect(handPosition, 264);
                                brimstoneMagic.velocity = NPC.DirectionTo(player.Center).RotatedByRandom(0.24f) * Main.rand.NextFloat(5f, 7f) + NPC.velocity;
                                brimstoneMagic.scale = Main.rand.NextFloat(1.25f, 1.35f);
                                brimstoneMagic.noGravity = true;
                                brimstoneMagic.color = Color.OrangeRed;
                                brimstoneMagic.fadeIn = 1.5f;
                                brimstoneMagic.noLight = true;
                            }
                            NPC.localAI[1] = 0f;

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
                                float num837 = 5f * uDieLul;
                                int num839 = ModContent.ProjectileType<BrimstoneFireblast>();
                                num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
                                num836 = num837 / num836;
                                num834 *= num836;
                                num835 *= num836;
                                vector83.X += num834 * 8f;
                                vector83.Y += num835 * 8f;
                                Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector83.X, vector83.Y, num834, num835, num839, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            }
                        }

                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 300f)
                        {
                            NPC.ai[1] = -1f;
                            NPC.TargetClosest();
                            NPC.netUpdate = true;
                        }
                    }
                }

                if (NPC.life < NPC.lifeMax * 0.4)
                {
                    NPC.ai[0] = 1f;
                    NPC.ai[1] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }
            }
            #endregion
            #region Transition

            else if (NPC.ai[0] == 1f || NPC.ai[0] == 2f)
            {
                NPC.dontTakeDamage = true;
                NPC.chaseable = false;

                if (NPC.ai[0] == 1f)
                {
                    NPC.ai[2] += 0.005f;
                    if (NPC.ai[2] > 0.5)
                        NPC.ai[2] = 0.5f;
                }
                else
                {
                    NPC.ai[2] -= 0.005f;
                    if (NPC.ai[2] < 0f)
                        NPC.ai[2] = 0f;
                }

                NPC.ai[1] += 1f;
                if (NPC.ai[1] == 100f)
                {
                    NPC.ai[0] += 1f;
                    NPC.ai[1] = 0f;

                    if (NPC.ai[0] == 3f)
                        NPC.ai[2] = 0f;
                    else
                    {
                        for (int num388 = 0; num388 < 50; num388++)
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f, 0, default, 1f);

                        SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SupremeCalamitasSpawn"), NPC.Center);
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    Dust brimstoneFire = Dust.NewDustPerfect(NPC.Center + Main.rand.NextVector2Square(-24f, 24f), (int)CalamityDusts.Brimstone);
                    brimstoneFire.velocity = Vector2.UnitY * -Main.rand.NextFloat(2.75f, 4.25f);
                    brimstoneFire.noGravity = true;
                }

                if (!canDespawn)
                {
                    NPC.velocity *= 0.98f;

                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                        NPC.velocity.X = 0f;
                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                        NPC.velocity.Y = 0f;
                }
            }
            #endregion
            #region LastStage
            else
            {
                NPC.damage = NPC.defDamage;
                if (wormAlive)
                {
                    NPC.dontTakeDamage = true;
                    NPC.chaseable = false;
                }
                else
                {
                    if (NPC.AnyNPCs(ModContent.NPCType<SoulSeekerSupreme>()))
                    {
                        NPC.dontTakeDamage = true;
                        NPC.chaseable = false;
                    }
                    else
                    {
                        NPC.dontTakeDamage = false;
                        NPC.chaseable = true;
                    }
                }

                if (NPC.ai[1] == -1f)
                {
                    phaseChange++;
                    if (phaseChange > 23)
                        phaseChange = 0;

                    int phase = 0; //0 = shots above 1 = charge 2 = nothing 3 = hellblasts 4 = fireblasts
                    switch (phaseChange)
                    {
                        case 0:
                            phase = 0;
                            willCharge = false;
                            break; //0341
                        case 1:
                            phase = 3;
                            break;
                        case 2:
                            phase = 4;
                            willCharge = true;
                            break;
                        case 3:
                            phase = 1;
                            break;
                        case 4:
                            phase = 1;
                            break; //1430
                        case 5:
                            phase = 4;
                            willCharge = false;
                            break;
                        case 6:
                            phase = 3;
                            break;
                        case 7:
                            phase = 0;
                            willCharge = true;
                            break;
                        case 8:
                            phase = 1;
                            break; //1034
                        case 9:
                            phase = 0;
                            willCharge = false;
                            break;
                        case 10:
                            phase = 3;
                            break;
                        case 11:
                            phase = 4;
                            break;
                        case 12:
                            phase = 4;
                            break; //4310
                        case 13:
                            phase = 3;
                            willCharge = true;
                            break;
                        case 14:
                            phase = 1;
                            break;
                        case 15:
                            phase = 0;
                            willCharge = false;
                            break;
                        case 16:
                            phase = 4;
                            break; //4411
                        case 17:
                            phase = 4;
                            willCharge = true;
                            break;
                        case 18:
                            phase = 1;
                            break;
                        case 19:
                            phase = 1;
                            break;
                        case 20:
                            phase = 0;
                            break; //0101
                        case 21:
                            phase = 1;
                            break;
                        case 22:
                            phase = 0;
                            break;
                        case 23:
                            phase = 1;
                            break;
                    }

                    NPC.ai[1] = phase;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                }
                else
                {
                    if (NPC.ai[1] == 0f)
                    {
                        float num823 = 12f;
                        float num824 = 0.12f;

                        // Reduce acceleration if target is holding a true melee weapon
                        Item targetSelectedItem = player.inventory[player.selectedItem];
                        if (targetSelectedItem.DamageType == DamageClass.Melee && (targetSelectedItem.shoot == ProjectileID.None || targetSelectedItem.Calamity().trueMelee))
                        {
                            num824 *= 0.5f;
                        }

                        Vector2 vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num825 = player.position.X + (player.width / 2) - vector82.X;
                        float num826 = player.position.Y + (player.height / 2) - 550f - vector82.Y;
                        float num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);

                        num827 = num823 / num827;
                        num825 *= num827;
                        num826 *= num827;

                        if (!canDespawn)
                        {
                            if (NPC.velocity.X < num825)
                            {
                                NPC.velocity.X += num824;
                                if (NPC.velocity.X < 0f && num825 > 0f)
                                    NPC.velocity.X += num824;
                            }
                            else if (NPC.velocity.X > num825)
                            {
                                NPC.velocity.X -= num824;
                                if (NPC.velocity.X > 0f && num825 < 0f)
                                    NPC.velocity.X -= num824;
                            }

                            if (NPC.velocity.Y < num826)
                            {
                                NPC.velocity.Y += num824;
                                if (NPC.velocity.Y < 0f && num826 > 0f)
                                    NPC.velocity.Y += num824;
                            }
                            else if (NPC.velocity.Y > num826)
                            {
                                NPC.velocity.Y -= num824;
                                if (NPC.velocity.Y > 0f && num826 < 0f)
                                    NPC.velocity.Y -= num824;
                            }
                        }

                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 240f)
                        {
                            NPC.ai[1] = -1f;
                            NPC.TargetClosest();
                            NPC.netUpdate = true;
                        }

                        vector82 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        num825 = player.position.X + (player.width / 2) - vector82.X;
                        num826 = player.position.Y + (player.height / 2) - vector82.Y;

                        NPC.localAI[1] += wormAlive ? 0.5f : 1f;
                        if (NPC.localAI[1] > 60f)
                        {
                            NPC.localAI[1] = 0f;

                            float num828 = 10f * uDieLul;
                            Vector2 value9 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                            float num180 = player.position.X + player.width * 0.5f - value9.X;
                            float num181 = Math.Abs(num180) * 0.1f;
                            float num182 = player.position.Y + player.height * 0.5f - value9.Y - num181;
                            float num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);

                            num183 = num828 / num183;
                            num180 *= num183;
                            num182 *= num183;
                            value9.X += num180;
                            value9.Y += num182;

                            int randomShot = Main.rand.Next(6);
                            if (randomShot == 0 && canFireSplitingFireball)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
                                canFireSplitingFireball = false;
                                randomShot = ModContent.ProjectileType<BrimstoneFireblast>();
                                num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
                                num827 = num828 / num827;
                                num825 *= num827;
                                num826 *= num827;
                                vector82.X += num825 * 8f;
                                vector82.Y += num826 * 8f;

                                for (int i = 0; i < 16; i++)
                                {
                                    Dust magic = Dust.NewDustPerfect(value9, 264);
                                    magic.velocity = new Vector2(num180, num182).RotatedByRandom(0.36f) * Main.rand.NextFloat(0.9f, 1.1f) * 1.2f;
                                    magic.color = Color.OrangeRed;
                                    magic.scale = 1.3f;
                                    magic.fadeIn = 0.6f;
                                    magic.noLight = true;
                                    magic.noGravity = true;
                                }

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector82.X, vector82.Y, num825, num826, randomShot, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
                                    NPC.netUpdate = true;
                                }
                            }
                            else if (randomShot == 1 && canFireSplitingFireball)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneShoot"), NPC.Center);
                                canFireSplitingFireball = false;
                                randomShot = ModContent.ProjectileType<BrimstoneGigaBlast>();
                                num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
                                num827 = num828 / num827;
                                num825 *= num827;
                                num826 *= num827;
                                vector82.X += num825 * 8f;
                                vector82.Y += num826 * 8f;

                                for (int i = 0; i < 24; i++)
                                {
                                    Dust magic = Dust.NewDustPerfect(value9, (int)CalamityDusts.Brimstone);
                                    magic.velocity = new Vector2(num180, num182).RotatedByRandom(0.36f) * Main.rand.NextFloat(0.9f, 1.1f) * 1.6f;
                                    magic.color = Color.Red;
                                    magic.scale = 1.5f;
                                    magic.fadeIn = 0.8f;
                                    magic.noLight = true;
                                    magic.noGravity = true;
                                }

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector82.X, vector82.Y, num825, num826, randomShot, gigablastDamage, 0f, Main.myPlayer, 0f, 0f);
                                    NPC.netUpdate = true;
                                }
                            }
                            else
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
                                canFireSplitingFireball = true;
                                randomShot = ModContent.ProjectileType<BrimstoneBarrage>();
                                for (int num186 = 0; num186 < 8; num186++)
                                {
                                    num180 = player.position.X + player.width * 0.5f - value9.X;
                                    num182 = player.position.Y + player.height * 0.5f - value9.Y;
                                    num183 = (float)Math.Sqrt(num180 * num180 + num182 * num182);
                                    float speedBoost = num186 > 3 ? -(num186 - 3) : num186;
                                    num183 = (8f + speedBoost) / num183;
                                    num180 *= num183;
                                    num182 *= num183;

                                    for (int i = 0; i < 10; i++)
                                    {
                                        Vector2 magicDustVelocity = new Vector2(num180 + speedBoost, num182 + speedBoost);
                                        magicDustVelocity *= MathHelper.Lerp(0.3f, 1f, i / 10f);

                                        Dust magic = Dust.NewDustPerfect(value9, 264);
                                        magic.velocity = magicDustVelocity;
                                        magic.color = Color.Red;
                                        magic.scale = MathHelper.Lerp(0.975f, 1.7f, i / 10f);
                                        magic.fadeIn = 0.9f;
                                        magic.noLight = true;
                                        magic.noGravity = true;
                                    }

                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), value9.X, value9.Y, num180 + speedBoost, num182 + speedBoost, randomShot, barrageDamage, 0f, Main.myPlayer, 0f, 0f);
                                        NPC.netUpdate = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (NPC.ai[1] == 1f)
                    {
                        float num383 = wormAlive ? 31f : 35f;
                        if (NPC.life < NPC.lifeMax * 0.3)
                            num383 += 1f;
                        if (NPC.life < NPC.lifeMax * 0.2)
                            num383 += 1f;
                        if (NPC.life < NPC.lifeMax * 0.1)
                            num383 += 1f;

                        Vector2 vector37 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num384 = player.position.X + (player.width / 2) - vector37.X;
                        float num385 = player.position.Y + (player.height / 2) - vector37.Y;
                        float num386 = (float)Math.Sqrt(num384 * num384 + num385 * num385);
                        num386 = num383 / num386;

                        if (!canDespawn)
                        {
                            NPC.velocity.X = num384 * num386;
                            NPC.velocity.Y = num385 * num386;
                            shieldRotation = NPC.velocity.ToRotation();
                            NPC.netUpdate = true;

                            SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/SCalDash"), NPC.Center);
                        }

                        NPC.ai[1] = 2f;
                    }
                    else if (NPC.ai[1] == 2f)
                    {
                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 25f)
                        {
                            if (!canDespawn)
                            {
                                NPC.velocity *= 0.96f;

                                if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                                    NPC.velocity.X = 0f;
                                if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                                    NPC.velocity.Y = 0f;
                            }
                        }


                        bool willChargeAgain = NPC.ai[3] + 1 < 1;

                        if (NPC.ai[2] >= 70f)
                        {
                            NPC.ai[3] += 1f;
                            NPC.ai[2] = 0f;
                            NPC.TargetClosest();

                            if (!willChargeAgain)
                                NPC.ai[1] = -1f;
                            else
                                NPC.ai[1] = 1f;
                        }

                        if (willChargeAgain && NPC.ai[2] > 50f)
                        {
                            float idealRotation = NPC.AngleTo(player.Center);
                            shieldRotation = shieldRotation.AngleLerp(idealRotation, 0.125f);
                            shieldRotation = shieldRotation.AngleTowards(idealRotation, 0.18f);
                        }

                        FrameType = FrameAnimationType.FasterUpwardDraft;
                    }
                    else if (NPC.ai[1] == 3f)
                    {
                        float num412 = 32f;
                        float num413 = 1.2f;

                        // Reduce acceleration if target is holding a true melee weapon
                        Item targetSelectedItem = player.inventory[player.selectedItem];
                        if (targetSelectedItem.DamageType == DamageClass.Melee && (targetSelectedItem.shoot == ProjectileID.None || targetSelectedItem.Calamity().trueMelee))
                        {
                            num413 *= 0.5f;
                        }

                        int num414 = 1;
                        if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
                            num414 = -1;

                        Vector2 handPosition = NPC.Center + new Vector2(NPC.spriteDirection * -18f, 2f);
                        Vector2 vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num415 = player.position.X + (player.width / 2) + (num414 * 600) - vector40.X;
                        float num416 = player.position.Y + (player.height / 2) - vector40.Y;
                        float num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);

                        num417 = num412 / num417;
                        num415 *= num417;
                        num416 *= num417;

                        if (!canDespawn)
                        {
                            if (NPC.velocity.X < num415)
                            {
                                NPC.velocity.X += num413;
                                if (NPC.velocity.X < 0f && num415 > 0f)
                                    NPC.velocity.X += num413;
                            }
                            else if (NPC.velocity.X > num415)
                            {
                                NPC.velocity.X -= num413;
                                if (NPC.velocity.X > 0f && num415 < 0f)
                                    NPC.velocity.X -= num413;
                            }
                            if (NPC.velocity.Y < num416)
                            {
                                NPC.velocity.Y += num413;
                                if (NPC.velocity.Y < 0f && num416 > 0f)
                                    NPC.velocity.Y += num413;
                            }
                            else if (NPC.velocity.Y > num416)
                            {
                                NPC.velocity.Y -= num413;
                                if (NPC.velocity.Y > 0f && num416 < 0f)
                                    NPC.velocity.Y -= num413;
                            }
                        }

                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 300f)
                        {
                            NPC.ai[1] = -1f;
                            NPC.TargetClosest();
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            if (!player.dead)
                                NPC.ai[3] += wormAlive ? 0.5f : 1f;

                            if (NPC.ai[3] >= 24f)
                            {
                                NPC.ai[3] = 0f;
                                vector40 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                                num415 = player.position.X + (player.width / 2) - vector40.X;
                                num416 = player.position.Y + (player.height / 2) - vector40.Y;
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneHellblastSound"), NPC.Center);

                                // Release a burst of magic dust along with a brimstone hellblast skull.
                                for (int i = 0; i < 25; i++)
                                {
                                    Dust brimstoneMagic = Dust.NewDustPerfect(handPosition, 264);
                                    brimstoneMagic.velocity = NPC.DirectionTo(player.Center).RotatedByRandom(0.31f) * Main.rand.NextFloat(3f, 5f) + NPC.velocity;
                                    brimstoneMagic.scale = Main.rand.NextFloat(1.25f, 1.35f);
                                    brimstoneMagic.noGravity = true;
                                    brimstoneMagic.color = Color.OrangeRed;
                                    brimstoneMagic.fadeIn = 1.5f;
                                    brimstoneMagic.noLight = true;
                                }

                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    float num418 = 10f * uDieLul;
                                    int num420 = ModContent.ProjectileType<BrimstoneHellblast>();
                                    num417 = (float)Math.Sqrt(num415 * num415 + num416 * num416);
                                    num417 = num418 / num417;
                                    num415 *= num417;
                                    num416 *= num417;
                                    vector40.X += num415 * 4f;
                                    vector40.Y += num416 * 4f;
                                    Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector40.X, vector40.Y, num415, num416, num420, hellblastDamage, 0f, Main.myPlayer, 0f, 0f);
                                }
                            }
                        }

                        if (Main.rand.NextBool(2))
                        {
                            Dust brimstoneMagic = Dust.NewDustPerfect(handPosition, 264);
                            brimstoneMagic.velocity = Vector2.UnitY.RotatedByRandom(0.14f) * Main.rand.NextFloat(-3.5f, -3f) + NPC.velocity;
                            brimstoneMagic.scale = Main.rand.NextFloat(1.25f, 1.35f);
                            brimstoneMagic.noGravity = true;
                            brimstoneMagic.noLight = true;
                        }

                        FrameChangeSpeed = 0.245f;
                        FrameType = FrameAnimationType.PunchHandCast;
                    }
                    else if (NPC.ai[1] == 4f)
                    {
                        int num831 = 1;
                        if (NPC.position.X + (NPC.width / 2) < player.position.X + player.width)
                            num831 = -1;

                        float num832 = 32f;
                        float num833 = 1.2f;

                        // Reduce acceleration if target is holding a true melee weapon
                        Item targetSelectedItem = player.inventory[player.selectedItem];
                        if (targetSelectedItem.DamageType == DamageClass.Melee && (targetSelectedItem.shoot == ProjectileID.None || targetSelectedItem.Calamity().trueMelee))
                        {
                            num833 *= 0.5f;
                        }

                        Vector2 vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        float num834 = player.position.X + (player.width / 2) + (num831 * 750) - vector83.X; //600
                        float num835 = player.position.Y + (player.height / 2) - vector83.Y;
                        float num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);

                        num836 = num832 / num836;
                        num834 *= num836;
                        num835 *= num836;

                        if (!canDespawn)
                        {
                            if (NPC.velocity.X < num834)
                            {
                                NPC.velocity.X += num833;
                                if (NPC.velocity.X < 0f && num834 > 0f)
                                    NPC.velocity.X += num833;
                            }
                            else if (NPC.velocity.X > num834)
                            {
                                NPC.velocity.X -= num833;
                                if (NPC.velocity.X > 0f && num834 < 0f)
                                    NPC.velocity.X -= num833;
                            }
                            if (NPC.velocity.Y < num835)
                            {
                                NPC.velocity.Y += num833;
                                if (NPC.velocity.Y < 0f && num835 > 0f)
                                    NPC.velocity.Y += num833;
                            }
                            else if (NPC.velocity.Y > num835)
                            {
                                NPC.velocity.Y -= num833;
                                if (NPC.velocity.Y > 0f && num835 < 0f)
                                    NPC.velocity.Y -= num833;
                            }
                        }

                        vector83 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
                        num834 = player.position.X + (player.width / 2) - vector83.X;
                        num835 = player.position.Y + (player.height / 2) - vector83.Y;

                        int shootRate = wormAlive ? 200 : 100;
                        NPC.localAI[1]++;
                        if (NPC.ai[2] > 40f && (NPC.localAI[1] > shootRate - 18 || NPC.localAI[1] <= 15f))
                        {
                            FrameChangeSpeed = 0f;
                            FrameType = FrameAnimationType.BlastPunchCast;
                        }

                        if (NPC.localAI[1] > shootRate)
                        {
                            Vector2 handPosition = NPC.Center + new Vector2(NPC.spriteDirection * -22f, 2f);

                            // Release a burst of magic dust when punching.
                            for (int i = 0; i < 25; i++)
                            {
                                Dust brimstoneMagic = Dust.NewDustPerfect(handPosition, 264);
                                brimstoneMagic.velocity = NPC.DirectionTo(player.Center).RotatedByRandom(0.24f) * Main.rand.NextFloat(5f, 7f) + NPC.velocity;
                                brimstoneMagic.scale = Main.rand.NextFloat(1.25f, 1.35f);
                                brimstoneMagic.noGravity = true;
                                brimstoneMagic.color = Color.OrangeRed;
                                brimstoneMagic.fadeIn = 1.5f;
                                brimstoneMagic.noLight = true;
                            }
                            NPC.localAI[1] = 0f;

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/BrimstoneBigShoot"), NPC.Center);
                                NPC.localAI[1] = 0f;
                                float num837 = 5f * uDieLul;
                                int num839 = ModContent.ProjectileType<BrimstoneFireblast>();
                                num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
                                num836 = num837 / num836;
                                num834 *= num836;
                                num835 *= num836;
                                vector83.X += num834 * 8f;
                                vector83.Y += num835 * 8f;
                                Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), vector83.X, vector83.Y, num834, num835, num839, fireblastDamage, 0f, Main.myPlayer, 0f, 0f);
                            }
                        }

                        NPC.ai[2] += 1f;
                        if (NPC.ai[2] >= 240f)
                        {
                            NPC.ai[1] = -1f;
                            NPC.TargetClosest();
                            NPC.netUpdate = true;
                        }
                    }
                }
            }
            #endregion
        }

        public void DoHeartsSpawningCastAnimation(Player target, bool death)
        {
            int tempSpawnY = spawnY;
            tempSpawnY += 250;
            if (death)
                tempSpawnY -= 50;

            List<Vector2> heartSpawnPositions = new List<Vector2>();
            for (int i = 0; i < 5; i++)
            {
                heartSpawnPositions.Add(new Vector2(spawnX + spawnXAdd * i + 50, tempSpawnY + spawnYAdd * i));
                heartSpawnPositions.Add(new Vector2(spawnX2 - spawnXAdd * i - 50, tempSpawnY + spawnYAdd * i));
            }

            float castCompletion = Utils.GetLerpValue(sepulcherSpawnCastTime - 25f, 0f, attackCastDelay, true);
            Vector2 armPosition = NPC.Center + Vector2.UnitX * NPC.spriteDirection * -8f;

            // Emit dust at the arm position as a sort of magic effect.
            Dust magic = Dust.NewDustPerfect(armPosition, 264);
            magic.velocity = Vector2.UnitY.RotatedByRandom(0.17f) * -Main.rand.NextFloat(2.7f, 4.1f);
            magic.color = Color.OrangeRed;
            magic.noLight = true;
            magic.fadeIn = 0.6f;
            magic.noGravity = true;

            foreach (Vector2 heartSpawnPosition in heartSpawnPositions)
            {
                Vector2 leftDustPosition = Vector2.CatmullRom(armPosition + Vector2.UnitY * 1000f, armPosition, heartSpawnPosition, heartSpawnPosition + Vector2.UnitY * 1000f, castCompletion);

                Dust castMagicDust = Dust.NewDustPerfect(leftDustPosition, 267);
                castMagicDust.scale = 1.67f;
                castMagicDust.velocity = Main.rand.NextVector2CircularEdge(0.2f, 0.2f);
                castMagicDust.fadeIn = 0.67f;
                castMagicDust.color = Color.Red;
                castMagicDust.noGravity = true;
            }

            if (attackCastDelay == 0)
            {
                string key = "Mods.CalamityMod.SCalStartText";
                if (NPC.life <= NPC.lifeMax * 0.08)
                    key = "Mods.CalamityMod.SCalSepulcher2Text";

                if (!BossRushEvent.BossRushActive)
                {
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                foreach (Vector2 heartSpawnPosition in heartSpawnPositions)
                {
                    // Make the hearts appear in a burst of flame.
                    for (int i = 0; i < 20; i++)
                    {
                        Dust castFire = Dust.NewDustPerfect(heartSpawnPosition + Main.rand.NextVector2Square(-30f, 30f), (int)CalamityDusts.Brimstone);
                        castFire.velocity = Vector2.UnitY.RotatedByRandom(0.08f) * -Main.rand.NextFloat(3f, 4.45f);
                        castFire.scale = Main.rand.NextFloat(1.35f, 1.6f);
                        castFire.fadeIn = 1.25f;
                        castFire.noGravity = true;
                    }
                }

                // And play a fire-like sound effect.
                hasSummonedSepulcher1 = true;
                hasSummonedSepulcher2 = NPC.life <= NPC.lifeMax * 0.08;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    List<int> hearts = new List<int>();
                    for (int x = 0; x < 5; x++)
                    {
                        hearts.Add(NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), spawnX + 50, tempSpawnY, ModContent.NPCType<BrimstoneHeart>(), 0, 0f, 0f, 0f, 0f, 255));
                        spawnX += spawnXAdd;

                        hearts.Add(NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), spawnX2 - 50, tempSpawnY, ModContent.NPCType<BrimstoneHeart>(), 0, 0f, 0f, 0f, 0f, 255));
                        spawnX2 -= spawnXAdd;
                        tempSpawnY += spawnYAdd;
                    }

                    ConnectAllBrimstoneHearts(hearts);

                    spawnX = spawnXReset;
                    spawnX2 = spawnXReset2;
                    spawnY = spawnYReset;
                    NPC.SpawnOnPlayer(NPC.FindClosestPlayer(), ModContent.NPCType<SCalWormHead>());
                    NPC.netUpdate = true;
                }

                SoundEngine.PlaySound(SoundLoader.GetLegacySoundSlot(Mod, "Sounds/Custom/SCalSounds/SepulcherSpawn"), target.Center);
            }
        }

        public void DoBrothersSpawningCastAnimation(int bodyWidth, int bodyHeight)
        {
            Vector2 leftOfCircle = NPC.Center + Vector2.UnitY * bodyHeight * 0.5f - Vector2.UnitX * bodyWidth * 0.45f;
            Vector2 rightOfCircle = NPC.Center + Vector2.UnitY * bodyHeight * 0.5f + Vector2.UnitX * bodyWidth * 0.45f;

            if (Main.netMode != NetmodeID.MultiplayerClient && catastropheSpawnPosition == Vector2.Zero)
            {
                catastropheSpawnPosition = NPC.Center - Vector2.UnitX * 500f;
                cataclysmSpawnPosition = NPC.Center + Vector2.UnitX * 500f;
                NPC.netUpdate = true;
            }

            // Draw some magic dust much like the sandstorm elemental cast that approaches where the brothers will spawn.
            if (attackCastDelay < brothersSpawnCastTime - 45f && attackCastDelay >= 60f)
            {
                float castCompletion = Utils.GetLerpValue(brothersSpawnCastTime - 45f, 60f, attackCastDelay);

                Vector2 leftDustPosition = Vector2.CatmullRom(leftOfCircle + Vector2.UnitY * 1000f, leftOfCircle, catastropheSpawnPosition, catastropheSpawnPosition + Vector2.UnitY * 1000f, castCompletion);
                Vector2 rightDustPosition = Vector2.CatmullRom(rightOfCircle + Vector2.UnitY * 1000f, rightOfCircle, cataclysmSpawnPosition, cataclysmSpawnPosition + Vector2.UnitY * 1000f, castCompletion);

                Dust castMagicDust = Dust.NewDustPerfect(leftDustPosition, 267);
                castMagicDust.scale = 1.67f;
                castMagicDust.velocity = Main.rand.NextVector2CircularEdge(0.2f, 0.2f);
                castMagicDust.color = Color.Red;
                castMagicDust.noGravity = true;

                castMagicDust = Dust.CloneDust(castMagicDust);
                castMagicDust.position = rightDustPosition;
            }

            // Make some magic effects at where the bros will spawn.
            if (attackCastDelay < 60f)
            {
                float burnPower = Utils.GetLerpValue(60f, 20f, attackCastDelay);
                if (attackCastDelay == 0f)
                    burnPower = 4f;

                for (int i = 0; i < MathHelper.Lerp(5, 25, burnPower); i++)
                {
                    Dust fire = Dust.NewDustPerfect(catastropheSpawnPosition + Main.rand.NextVector2Circular(60f, 60f), 264);
                    fire.velocity = Vector2.UnitY * -Main.rand.NextFloat(2f, 4f);
                    if (attackCastDelay == 0)
                    {
                        fire.velocity += Main.rand.NextVector2Circular(4f, 4f);
                        fire.fadeIn = 1.6f;
                    }
                    fire.noGravity = true;
                    fire.noLight = true;
                    fire.color = Color.OrangeRed;
                    fire.scale = 1.4f + fire.velocity.Y * 0.16f;

                    fire = Dust.NewDustPerfect(cataclysmSpawnPosition + Main.rand.NextVector2Circular(60f, 60f), 264);
                    fire.velocity = Vector2.UnitY * -Main.rand.NextFloat(2f, 4f);
                    if (attackCastDelay == 0)
                    {
                        fire.velocity += Main.rand.NextVector2Circular(4f, 4f);
                        fire.fadeIn = 1.6f;
                    }
                    fire.noGravity = true;
                    fire.noLight = true;
                    fire.color = Color.OrangeRed;
                    fire.scale = 1.4f + fire.velocity.Y * 0.16f;
                }
            }

            // And spawn them.
            if (attackCastDelay == 0)
            {
                if (!BossRushEvent.BossRushActive)
                {
                    string key = "Mods.CalamityMod.SCalBrothersText";
                    if (DownedBossSystem.downedSCal)
                        key += "Rematch";
                    CalamityUtils.DisplayLocalizedText(key, textColor);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    CalamityUtils.SpawnBossBetter(catastropheSpawnPosition, ModContent.NPCType<SupremeCatastrophe>());
                    CalamityUtils.SpawnBossBetter(cataclysmSpawnPosition, ModContent.NPCType<SupremeCataclysm>());
                }
                SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, NPC.Center);
                hasSummonedBrothers = true;
            }
        }

        public void ConnectAllBrimstoneHearts(List<int> heartIndices)
        {
            int heartType = ModContent.NPCType<BrimstoneHeart>();

            // Ensure that the hearts go in order based on the arena.
            IEnumerable<NPC> hearts = heartIndices.Select(i => Main.npc[i]);
            hearts = hearts.OrderByDescending(heart => Math.Abs(heart.Center.X - safeBox.Left)).ToList();

            int firstHeartIndex = heartIndices.First();
            int lastHeartIndex = heartIndices.Last();

            heartIndices = heartIndices.OrderByDescending(heart => Math.Abs(Main.npc[heart].Center.X - safeBox.Left)).ToList();

            for (int i = 0; i < hearts.Count(); i++)
            {
                NPC heart = hearts.ElementAt(i);

                Vector2 endpoint = safeBox.TopLeft();
                Vector2 oppositePosition = Vector2.Zero;

                for (int j = 0; j < 2; j++)
                {
                    int tries = 0;
                    do
                    {
                        endpoint.X = heart.Center.X + (j == 0).ToDirectionInt() * Main.rand.NextFloat(75f, 250f);
                        tries++;
                        if (tries >= 100)
                            break;
                    }
                    while (Math.Abs(endpoint.X - safeBox.Center.X) > safeBox.Width * 0.48f);

                    if (tries >= 100)
                        endpoint.X = MathHelper.Clamp(endpoint.X, safeBox.Left, safeBox.Right);

                    heart.ModNPC<BrimstoneHeart>().ChainEndpoints.Add(endpoint);
                }

                if (Main.rand.NextBool(2))
                {
                    endpoint.X = heart.Center.X + Main.rand.NextBool(2).ToDirectionInt() * Main.rand.NextFloat(45f, 360f);
                    endpoint.X = MathHelper.Clamp(endpoint.X, safeBox.Left, safeBox.Right);
                    heart.ModNPC<BrimstoneHeart>().ChainEndpoints.Add(endpoint);
                }

                heart.netUpdate = true;
            }
        }

        #region Loot
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ModContent.ItemType<OmegaHealingPotion>();
        }

        public override void NPCLoot()
        {
            // Create a teleport line effect
            Dust.QuickDustLine(NPC.Center, initialRitualPosition, 500f, Color.Red);
            NPC.Center = initialRitualPosition;

            CalamityGlobalNPC.SetNewBossJustDowned(NPC);

            DropHelper.DropBags(NPC);

            // Only drops in Malice because this is Leviathan's item
            DropHelper.DropItemCondition(NPC, ModContent.ItemType<GaelsGreatsword>(), true, CalamityWorld.malice);

            // Levi drops directly from the boss so that you cannot obtain it by difficulty swapping bags
            // It is one of extremely few Deathmode exclusive drops in the game
            DropHelper.DropItemCondition(NPC, ModContent.ItemType<Levi>(), true, CalamityWorld.death);

            // All other drops are contained in the bag, so they only drop directly on Normal
            if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItem(NPC, ModContent.ItemType<CalamitousEssence>(), true, 18, 27);

                // Weapons
                float w = DropHelper.NormalWeaponDropRateFloat;
                DropHelper.DropEntireWeightedSet(NPC,
                    DropHelper.WeightStack<Violence>(w),
                    DropHelper.WeightStack<Condemnation>(w),
                    DropHelper.WeightStack<Heresy>(w),
                    DropHelper.WeightStack<Vehemenc>(w),
                    DropHelper.WeightStack<Perdition>(w),
                    DropHelper.WeightStack<Vigilance>(w),
                    DropHelper.WeightStack<Sacrifice>(w)
                );

                // Equipment
                DropHelper.DropItem(NPC, ModContent.ItemType<Calamity>(), true);

                // SCal vanity set (This drops all at once, or not at all)
                if (Main.rand.NextBool(7))
                {
                    DropHelper.DropItem(NPC, ModContent.ItemType<AshenHorns>());
                    DropHelper.DropItem(NPC, ModContent.ItemType<SCalMask>());
                    DropHelper.DropItem(NPC, ModContent.ItemType<SCalRobes>());
                    DropHelper.DropItem(NPC, ModContent.ItemType<SCalBoots>());
                }
            }

            // Other
            DropHelper.DropItemChance(NPC, ModContent.ItemType<SupremeCalamitasTrophy>(), 10);
            DropHelper.DropItemCondition(NPC, ModContent.ItemType<KnowledgeCalamitas>(), true, !DownedBossSystem.downedSCal);

            // Increase the player's SCal kill count
            if (Main.player[NPC.target].Calamity().sCalKillCount < 5)
                Main.player[NPC.target].Calamity().sCalKillCount++;

            // Spawn the SCal NPC directly where the boss was
            NPC.NewNPC(NPC.GetSpawnSourceForNPCFromNPCAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 12, ModContent.NPCType<WITCH>());

            // Mark Supreme Calamitas as defeated
            DownedBossSystem.downedSCal = true;
            CalamityNetcode.SyncWorld();
        }
        #endregion

        // Prevent the player from accidentally killing SCal instead of having her turn into a town NPC.
        public override bool CheckDead()
        {
            if (BossRushEvent.BossRushActive)
                return true;

            NPC.life = 1;
            NPC.active = true;
            NPC.dontTakeDamage = true;
            NPC.netUpdate = true;
            return false;
        }

        public override bool CheckActive()
        {
            return canDespawn;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = 1;

            Vector2 shieldCenter = NPC.Center + shieldRotation.ToRotationVector2() * 24f;
            Vector2 shieldTop = shieldCenter - (shieldRotation + MathHelper.PiOver2).ToRotationVector2() * 61f;
            Vector2 shieldBottom = shieldCenter - (shieldRotation + MathHelper.PiOver2).ToRotationVector2() * 61f;

            float _ = 0f;
            bool collidingWithShield = Collision.CheckAABBvLineCollision(target.TopLeft, target.Size, shieldTop, shieldBottom, 64f, ref _) && shieldOpacity > 0.55f;
            return collidingWithShield || NPC.Hitbox.Intersects(target.Hitbox);
        }

        public override void FindFrame(int frameHeight)
        {
            bool wormAlive = false;
            if (CalamityGlobalNPC.SCalWorm != -1)
                wormAlive = Main.npc[CalamityGlobalNPC.SCalWorm].active;
            int shootRate = wormAlive ? 200 : 100;

            // Special punch logic for the blast attack.
            if (FrameType == FrameAnimationType.BlastPunchCast && (NPC.localAI[1] > shootRate - 18 || NPC.localAI[1] <= 15f))
            {
                if (NPC.localAI[1] > shootRate - 18)
                    NPC.frame.Y = (int)MathHelper.Lerp(0, 3, Utils.GetLerpValue(shootRate - 18, shootRate, NPC.localAI[1], true));
                else
                    NPC.frame.Y = (int)MathHelper.Lerp(3, 5, Utils.GetLerpValue(0f, 15f, NPC.localAI[1], true));
                NPC.frame.Y += (int)FrameType * 6;
            }
            else
            {
                NPC.frameCounter += FrameChangeSpeed;
                NPC.frameCounter %= 6;
                NPC.frame.Y = (int)NPC.frameCounter + (int)FrameType * 6;
            }
        }

        public override Color? GetAlpha(Color drawColor)
        {
            if (willCharge)
                return drawColor * NPC.Opacity * 0.45f;
            return null;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture2D15 = DownedBossSystem.downedSCal && !BossRushEvent.BossRushActive ? TextureAssets.Npc[NPC.type].Value : ModContent.Request<Texture2D>("CalamityMod/NPCs/SupremeCalamitas/SupremeCalamitasHooded").Value;

            Vector2 vector11 = new Vector2(texture2D15.Width / 2f, texture2D15.Height / Main.npcFrameCount[NPC.type] / 2f);
            Color color36 = Color.White;
            float amount9 = 0.5f;
            int num153 = 7;

            Rectangle frame = texture2D15.Frame(2, Main.npcFrameCount[NPC.type], NPC.frame.Y / Main.npcFrameCount[NPC.type], NPC.frame.Y % Main.npcFrameCount[NPC.type]);

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int num155 = 1; num155 < num153; num155 += 2)
                {
                    Color color38 = lightColor;
                    color38 = Color.Lerp(color38, color36, amount9);
                    color38 = NPC.GetAlpha(color38);
                    color38 *= (num153 - num155) / 15f;
                    Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                    vector41 -= new Vector2(texture2D15.Width / 2f, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    vector41 += vector11 * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, vector41, frame, color38, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
                }
            }

            bool inPhase2 = NPC.ai[0] >= 3f && NPC.life > NPC.lifeMax * 0.01;
            Vector2 vector43 = NPC.Center - Main.screenPosition;
            vector43 -= new Vector2(texture2D15.Width / 2f, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, NPC.gfxOffY);

            if (inPhase2)
            {
                // Make the sprite jitter with rage in phase 2. This does not happen in rematches since it would make little sense logically.
                if (!DownedBossSystem.downedSCal)
                    vector43 += Main.rand.NextVector2Circular(0.25f, 0.7f);

                // And gain a flaming aura.
                Color auraColor = NPC.GetAlpha(Color.Red) * 0.4f;
                for (int i = 0; i < 7; i++)
                {
                    Vector2 rotationalDrawOffset = (MathHelper.TwoPi * i / 7f + Main.GlobalTimeWrappedHourly * 4f).ToRotationVector2();
                    rotationalDrawOffset *= MathHelper.Lerp(3f, 4.25f, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 4f) * 0.5f + 0.5f);
                    spriteBatch.Draw(texture2D15, vector43 + rotationalDrawOffset, frame, auraColor, NPC.rotation, vector11, NPC.scale * 1.1f, spriteEffects, 0f);
                }
            }
            spriteBatch.Draw(texture2D15, vector43, frame, NPC.GetAlpha(lightColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            DrawForcefield(spriteBatch);
            DrawShield(spriteBatch);
            return false;
        }

        public void DrawForcefield(SpriteBatch spriteBatch)
        {
            spriteBatch.EnterShaderRegion();

            float intensity = hitTimer / 35f;

            // Shield intensity is always high during invincibility, except during cast animations, so that she can be more easily seen.
            if (NPC.dontTakeDamage && attackCastDelay <= 0)
                intensity = 0.75f + Math.Abs((float)Math.Cos(Main.GlobalTimeWrappedHourly * 1.7f)) * 0.1f;

            // Make the forcefield weaker in the second phase as a means of showing desparation.
            if (NPC.ai[0] >= 3f)
                intensity *= 0.6f;

            float lifeRatio = NPC.life / (float)NPC.lifeMax;
            float flickerPower = 0f;
            if (lifeRatio < 0.6f)
                flickerPower += 0.1f;
            if (lifeRatio < 0.3f)
                flickerPower += 0.15f;
            if (lifeRatio < 0.1f)
                flickerPower += 0.2f;
            if (lifeRatio < 0.05f)
                flickerPower += 0.1f;
            float opacity = forcefieldOpacity;
            opacity *= MathHelper.Lerp(1f, MathHelper.Max(1f - flickerPower, 0.56f), (float)Math.Pow(Math.Cos(Main.GlobalTimeWrappedHourly * MathHelper.Lerp(3f, 5f, flickerPower)), 24D));

            // During/prior to a charge the forcefield is always darker than usual and thus its intensity is also higher.
            if (!NPC.dontTakeDamage && (willCharge || NPC.ai[1] == 2f))
                intensity = 1.1f;

            // Dampen the opacity and intensity slightly, to allow SCal to be more easily visible inside of the forcefield.
            intensity *= 0.75f;
            opacity *= 0.75f;

            Texture2D forcefieldTexture = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/CalamitasShield").Value;
            GameShaders.Misc["CalamityMod:SupremeShield"].UseImage("Images/Misc/Perlin");

            Color forcefieldColor = Color.DarkViolet;
            Color secondaryForcefieldColor = Color.Red * 1.4f;

            if (!NPC.dontTakeDamage && (willCharge || NPC.ai[1] == 2f))
            {
                forcefieldColor *= 0.25f;
                secondaryForcefieldColor = Color.Lerp(secondaryForcefieldColor, Color.Black, 0.7f);
            }

            forcefieldColor *= opacity;
            secondaryForcefieldColor *= opacity;

            GameShaders.Misc["CalamityMod:SupremeShield"].UseSecondaryColor(secondaryForcefieldColor);
            GameShaders.Misc["CalamityMod:SupremeShield"].UseColor(forcefieldColor);
            GameShaders.Misc["CalamityMod:SupremeShield"].UseSaturation(intensity);
            GameShaders.Misc["CalamityMod:SupremeShield"].UseOpacity(opacity);
            GameShaders.Misc["CalamityMod:SupremeShield"].Apply();

            spriteBatch.Draw(forcefieldTexture, NPC.Center - Main.screenPosition, null, Color.White * opacity, 0f, forcefieldTexture.Size() * 0.5f, forcefieldScale * 3f, SpriteEffects.None, 0f);

            spriteBatch.ExitShaderRegion();
        }

        public void DrawShield(SpriteBatch spriteBatch)
        {
            float jawRotation = shieldRotation;
            float jawRotationOffset = 0f;

            // Have an agape mouth when charging.
            if (NPC.ai[1] == 2f)
                jawRotationOffset -= 0.71f;

            // And a laugh right before the charge.
            else if (willCharge && NPC.ai[1] != 2f && AttackCloseToBeingOver)
                jawRotationOffset += MathHelper.Lerp(0.04f, -0.82f, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 17.2f) * 0.5f + 0.5f);

            Color shieldColor = Color.White * shieldOpacity;
            Texture2D shieldSkullTexture = ModContent.Request<Texture2D>("CalamityMod/NPCs/SupremeCalamitas/SupremeShieldTop").Value;
            Texture2D shieldJawTexture = ModContent.Request<Texture2D>("CalamityMod/NPCs/SupremeCalamitas/SupremeShieldBottom").Value;
            Vector2 drawPosition = NPC.Center + shieldRotation.ToRotationVector2() * 24f - Main.screenPosition;
            Vector2 jawDrawPosition = drawPosition;
            SpriteEffects direction = Math.Cos(shieldRotation) > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
            if (direction == SpriteEffects.FlipVertically)
                jawDrawPosition += (shieldRotation - MathHelper.PiOver2).ToRotationVector2() * 42f;
            else
            {
                jawDrawPosition += (shieldRotation + MathHelper.PiOver2).ToRotationVector2() * 42f;
                jawRotationOffset *= -1f;
            }

            spriteBatch.Draw(shieldJawTexture, jawDrawPosition, null, shieldColor, jawRotation + jawRotationOffset, shieldJawTexture.Size() * 0.5f, 1f, direction, 0f);
            spriteBatch.Draw(shieldSkullTexture, drawPosition, null, shieldColor, shieldRotation, shieldSkullTexture.Size() * 0.5f, 1f, direction, 0f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                hitTimer = 35;
                NPC.netUpdate = true;
            }

            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                NPC.position.X = NPC.position.X + (NPC.width / 2);
                NPC.position.Y = NPC.position.Y + (NPC.height / 2);
                NPC.width = 100;
                NPC.height = 100;
                NPC.position.X = NPC.position.X - (NPC.width / 2);
                NPC.position.Y = NPC.position.Y - (NPC.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * NPC.GetExpertDamageMultiplier());
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(ModContent.BuffType<VulnerabilityHex>(), 600, true);
        }
    }
}
