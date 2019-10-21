﻿using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.LoreItems;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Pets;
using CalamityMod.Items.Placeables.Furniture.Trophies;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Tiles.Ores;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
namespace CalamityMod.NPCs.Perforator
{
    [AutoloadBossHead]
    public class PerforatorHive : ModNPC
    {
        private bool small = false;
        private bool medium = false;
        private bool large = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Perforator Hive");
            Main.npcFrameCount[npc.type] = 10;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 18f;
            npc.damage = 30;
            npc.width = 110;
            npc.height = 100;
            npc.defense = 4;
            npc.LifeMaxNERD(3750, 5400, 7600, 2700000, 3000000);
            double HPBoost = Config.BossHealthPercentageBoost * 0.01;
            npc.lifeMax += (int)(npc.lifeMax * HPBoost);
            npc.aiStyle = -1;
            aiType = -1;
            npc.buffImmune[ModContent.BuffType<GlacialState>()] = true;
            npc.buffImmune[ModContent.BuffType<TemporalSadness>()] = true;
            npc.knockBackResist = 0f;
            npc.value = Item.buyPrice(0, 6, 0, 0);
            npc.boss = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit13;
            npc.DeathSound = SoundID.NPCDeath19;
            Mod calamityModMusic = ModLoader.GetMod("CalamityModMusic");
            if (calamityModMusic != null)
                music = calamityModMusic.GetSoundSlot(SoundType.Music, "Sounds/Music/BloodCoagulant");
            else
                music = MusicID.Boss2;
            bossBag = ModContent.ItemType<PerforatorBag>();
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(npc.dontTakeDamage);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            npc.dontTakeDamage = reader.ReadBoolean();
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            CalamityGlobalNPC.perfHive = npc.whoAmI;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 1.5f, 0f, 0f);
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            bool isCrimson = player.ZoneCrimson || CalamityWorld.bossRushActive;
            bool expertMode = Main.expertMode || CalamityWorld.bossRushActive;
            bool revenge = CalamityWorld.revenge || CalamityWorld.bossRushActive;
            if (Vector2.Distance(player.Center, npc.Center) > 5600f)
            {
                if (npc.timeLeft > 10)
                    npc.timeLeft = 10;
            }
            if (!player.active || player.dead)
            {
                npc.TargetClosest(false);
                player = Main.player[npc.target];
                if (!player.active || player.dead)
                {
                    npc.velocity = new Vector2(0f, 10f);
                    if (npc.timeLeft > 150)
                    {
                        npc.timeLeft = 150;
                    }
                    return;
                }
            }
            else if (npc.timeLeft < 1800)
            {
                npc.timeLeft = 1800;
            }
            bool wormAlive = false;
            if (NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadLarge>()))
            {
                wormAlive = true;
            }
            if (wormAlive)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = isCrimson ? false : true;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int shoot = revenge ? 6 : 4;
                if (CalamityWorld.death || CalamityWorld.bossRushActive)
                {
                    shoot += 3;
                }
                npc.localAI[0] += (float)Main.rand.Next(shoot);
                if (npc.localAI[0] >= (float)Main.rand.Next(300, 900))
                {
                    npc.localAI[0] = 0f;
                    Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 20);
                    for (int num621 = 0; num621 < 8; num621++)
                    {
                        int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 170, 0f, 0f, 100, default, 1f);
                        Main.dust[num622].velocity *= 3f;
                        if (Main.rand.NextBool(2))
                        {
                            Main.dust[num622].scale = 0.25f;
                            Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        }
                    }
                    for (int num623 = 0; num623 < 16; num623++)
                    {
                        int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0f, 0f, 100, default, 1.5f);
                        Main.dust[num624].noGravity = true;
                        Main.dust[num624].velocity *= 5f;
                        num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0f, 0f, 100, default, 1f);
                        Main.dust[num624].velocity *= 2f;
                    }
                    float num179 = 8f;
                    Vector2 value9 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                    float num180 = player.position.X + (float)player.width * 0.5f - value9.X;
                    float num181 = Math.Abs(num180) * 0.1f;
                    float num182 = player.position.Y + (float)player.height * 0.5f - value9.Y - num181;
                    float num183 = (float)Math.Sqrt((double)(num180 * num180 + num182 * num182));
                    npc.netUpdate = true;
                    num183 = num179 / num183;
                    num180 *= num183;
                    num182 *= num183;
                    int num184 = expertMode ? 14 : 18;
                    int num185 = Main.rand.NextBool(2) ? ModContent.ProjectileType<IchorShot>() : ModContent.ProjectileType<BloodGeyser>();
                    value9.X += num180;
                    value9.Y += num182;
                    for (int num186 = 0; num186 < 20; num186++)
                    {
                        num180 = player.position.X + (float)player.width * 0.5f - value9.X;
                        num182 = player.position.Y + (float)player.height * 0.5f - value9.Y;
                        num183 = (float)Math.Sqrt((double)(num180 * num180 + num182 * num182));
                        num183 = num179 / num183;
                        num180 += (float)Main.rand.Next(-180, 181);
                        num180 *= num183;
                        Projectile.NewProjectile(value9.X, value9.Y, num180, -5f, num185, num184, 0f, Main.myPlayer, 0f, 0f);
                    }
                }
            }
            npc.rotation = npc.velocity.X * 0.04f;
            npc.spriteDirection = (npc.direction > 0) ? 1 : -1;
            if (npc.position.Y > player.position.Y - 160f) //200
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y = npc.velocity.Y * 0.98f;
                }
                npc.velocity.Y = npc.velocity.Y - (CalamityWorld.bossRushActive ? 0.15f : 0.1f);
                if (npc.velocity.Y > 3f)
                {
                    npc.velocity.Y = 3f;
                }
            }
            else if (npc.position.Y < player.position.Y - 400f) //500
            {
                if (npc.velocity.Y < 0f)
                {
                    npc.velocity.Y = npc.velocity.Y * 0.98f;
                }
                npc.velocity.Y = npc.velocity.Y + (CalamityWorld.bossRushActive ? 0.15f : 0.1f);
                if (npc.velocity.Y < -3f)
                {
                    npc.velocity.Y = -3f;
                }
            }
            if (npc.position.X + (float)(npc.width / 2) > player.position.X + (float)(player.width / 2) + 80f)
            {
                if (npc.velocity.X > 0f)
                {
                    npc.velocity.X = npc.velocity.X * 0.98f;
                }
                npc.velocity.X = npc.velocity.X - (CalamityWorld.bossRushActive ? 0.15f : 0.1f);
                if (npc.velocity.X > 8f)
                {
                    npc.velocity.X = 8f;
                }
            }
            if (npc.position.X + (float)(npc.width / 2) < player.position.X + (float)(player.width / 2) - 80f)
            {
                if (npc.velocity.X < 0f)
                {
                    npc.velocity.X = npc.velocity.X * 0.98f;
                }
                npc.velocity.X = npc.velocity.X + (CalamityWorld.bossRushActive ? 0.15f : 0.1f);
                if (npc.velocity.X < -8f)
                {
                    npc.velocity.X = -8f;
                }
            }
            if (npc.ai[3] == 0f && npc.life > 0)
            {
                npc.ai[3] = (float)npc.lifeMax;
            }
            if (npc.life > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int num660 = (int)((double)npc.lifeMax * 0.3);
                    if ((float)(npc.life + num660) < npc.ai[3])
                    {
                        npc.ai[3] = (float)npc.life;
                        int wormType = ModContent.NPCType<PerforatorHeadSmall>();
                        if (!small)
                        {
                            small = true;
                        }
                        else if (!medium)
                        {
                            medium = true;
                            wormType = ModContent.NPCType<PerforatorHeadMedium>();
                        }
                        else if (!large)
                        {
                            large = true;
                            wormType = ModContent.NPCType<PerforatorHeadLarge>();
                        }
                        NPC.SpawnOnPlayer(npc.FindClosestPlayer(), wormType);
                        return;
                    }
                }
            }
        }

        public override bool CheckDead()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<PerforatorHeadLarge>()))
            {
                return false;
            }
            return true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8f);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override void NPCLoot()
        {
            DropHelper.DropBags(npc);

            DropHelper.DropItemChance(npc, ModContent.ItemType<PerforatorTrophy>(), 10);
            DropHelper.DropItemCondition(npc, ModContent.ItemType<KnowledgePerforators>(), true, !CalamityWorld.downedPerforator);
            DropHelper.DropResidentEvilAmmo(npc, CalamityWorld.downedPerforator, 2, 0, 0);

            // All other drops are contained in the bag, so they only drop directly on Normal
            if (!Main.expertMode)
            {
                // Materials
                DropHelper.DropItemSpray(npc, ModContent.ItemType<BloodSample>(), 7, 14);
                DropHelper.DropItemSpray(npc, ItemID.CrimtaneBar, 2, 5);
                DropHelper.DropItemSpray(npc, ItemID.Vertebrae, 3, 9);
                if (Main.hardMode)
                    DropHelper.DropItemSpray(npc, ItemID.Ichor, 10, 20);

                // Weapons
                DropHelper.DropItemChance(npc, ModContent.ItemType<VeinBurster>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<BloodyRupture>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<SausageMaker>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<Aorta>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<Eviscerator>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<BloodBath>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<BloodClotStaff>(), 4);
                DropHelper.DropItemChance(npc, ModContent.ItemType<ToothBall>(), 4, 25, 50);

                // Vanity
                DropHelper.DropItemChance(npc, ModContent.ItemType<PerforatorMask>(), 7);
                DropHelper.DropItemChance(npc, ModContent.ItemType<BloodyVein>(), 10);
            }

            // If neither The Hive Mind nor The Perforator Hive have been killed yet, notify players of Aerialite Ore
            if (!CalamityWorld.downedHiveMind && !CalamityWorld.downedPerforator)
            {
                string key = "Mods.CalamityMod.SkyOreText";
                Color messageColor = Color.Cyan;
                WorldGenerationMethods.SpawnOre(ModContent.TileType<AerialiteOre>(), 12E-05, .4f, .6f);

                if (Main.netMode == NetmodeID.SinglePlayer)
                    Main.NewText(Language.GetTextValue(key), messageColor);
                else if (Main.netMode == NetmodeID.Server)
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
            }

            // Mark The Perforator Hive as dead
            CalamityWorld.downedPerforator = true;
            CalamityMod.UpdateServerBoolean();
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < damage / npc.lifeMax * 100.0; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default, 1f);
            }
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hive"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hive2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hive3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hive4"), 1f);
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 100;
                npc.height = 100;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for (int num621 = 0; num621 < 40; num621++)
                {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num622].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int num623 = 0; num623 < 70; num623++)
                {
                    int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0f, 0f, 100, default, 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 5, 0f, 0f, 100, default, 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }
    }
}
