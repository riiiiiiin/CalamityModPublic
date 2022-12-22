﻿using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.AdultEidolonWyrm
{
    public class AdultEidolonWyrmBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            DisplayName.SetDefault("Adult Eidolon Wyrm");
        }

        public override void SetDefaults()
        {
            NPC.damage = 100;
            NPC.width = 60;
            NPC.height = 88;
            NPC.defense = 0;
            NPC.LifeMaxNERB(2415000, 2898000);
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.Opacity = 0f;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
            NPC.dontCountMe = true;
            NPC.dontTakeDamage = true;
            NPC.chaseable = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[0] = reader.ReadSingle();
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override void AI()
        {
            NPC.damage = 0;

            // Difficulty modes
            bool death = CalamityWorld.death;
            bool revenge = CalamityWorld.revenge;
            bool expertMode = Main.expertMode;

            // Fade in.
            NPC.Opacity = MathHelper.Clamp(NPC.Opacity + 0.2f, 0f, 1f);

            if (NPC.ai[2] > 0f)
                NPC.realLife = (int)NPC.ai[2];

            // Check if other segments are still alive. If not, die.
            bool shouldDespawn = true;
            int wyrmHeadID = ModContent.NPCType<AdultEidolonWyrmHead>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == wyrmHeadID)
                {
                    shouldDespawn = false;
                    break;
                }
            }
            if (!shouldDespawn)
            {
                if (NPC.ai[1] <= 0f)
                    shouldDespawn = true;
                else if (Main.npc[(int)NPC.ai[1]].life <= 0)
                    shouldDespawn = true;
            }
            if (shouldDespawn)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
                NPC.active = false;
            }

            CalamityGlobalNPC calamityGlobalNPC_Head = Main.npc[(int)NPC.ai[2]].Calamity();

            float chargePhaseGateValue = death ? 180f : revenge ? 210f : expertMode ? 240f : 300f;
            float lightningChargePhaseGateValue = death ? 120f : revenge ? 135f : expertMode ? 150f : 180f;

            bool invisiblePartOfChargePhase = calamityGlobalNPC_Head.newAI[2] >= chargePhaseGateValue && calamityGlobalNPC_Head.newAI[2] <= chargePhaseGateValue + 1f && (calamityGlobalNPC_Head.newAI[0] == (float)AdultEidolonWyrmHead.Phase.ChargeOne || calamityGlobalNPC_Head.newAI[0] == (float)AdultEidolonWyrmHead.Phase.ChargeTwo || calamityGlobalNPC_Head.newAI[0] == (float)AdultEidolonWyrmHead.Phase.FastCharge);
            bool invisiblePartOfLightningChargePhase = calamityGlobalNPC_Head.newAI[2] >= lightningChargePhaseGateValue && calamityGlobalNPC_Head.newAI[2] <= lightningChargePhaseGateValue + 1f && calamityGlobalNPC_Head.newAI[0] == (float)AdultEidolonWyrmHead.Phase.LightningCharge;
            bool invisiblePhase = calamityGlobalNPC_Head.newAI[0] == 1f || calamityGlobalNPC_Head.newAI[0] == 5f || calamityGlobalNPC_Head.newAI[0] == 7f;
            if (!invisiblePartOfChargePhase && !invisiblePartOfLightningChargePhase && !invisiblePhase)
            {
                if (Main.npc[(int)NPC.ai[1]].Opacity > 0.5f)
                {
                    NPC.Opacity += 0.2f;
                    if (NPC.Opacity > 1f)
                        NPC.Opacity = 1f;
                }
            }
            else
            {
                NPC.Opacity -= 0.05f;
                if (NPC.Opacity < 0f)
                    NPC.Opacity = 0f;
            }

            bool spawnAncientLights = (calamityGlobalNPC_Head.newAI[0] == (float)AdultEidolonWyrmHead.Phase.ShadowFireballSpin && calamityGlobalNPC_Head.newAI[2] > 0f) ||
                (calamityGlobalNPC_Head.newAI[0] == (float)AdultEidolonWyrmHead.Phase.FinalPhase && calamityGlobalNPC_Head.newAI[1] > 0f);
            if (spawnAncientLights && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Vector2.Distance(NPC.Center, Main.player[Main.npc[(int)NPC.ai[2]].target].Center) > 160f)
                {
                    NPC.localAI[0] += 1f;
                    float spawnAncientLightGateValue = death ? 100f : revenge ? 105f : expertMode ? 110f : 120f;
                    float divisor = 4f;
                    if (NPC.ai[3] % divisor == 0f && NPC.localAI[0] >= spawnAncientLightGateValue)
                    {
                        NPC.localAI[0] = 0f;
                        float distanceVelocityBoost = MathHelper.Clamp((Vector2.Distance(Main.npc[(int)NPC.ai[2]].Center, Main.player[Main.npc[(int)NPC.ai[2]].target].Center) - 1600f) * 0.025f, 0f, 16f);
                        float lightVelocity = (Main.player[Main.npc[(int)NPC.ai[2]].target].Calamity().ZoneAbyssLayer4 ? 6f : 8f) + distanceVelocityBoost;
                        Vector2 destination = Main.player[Main.npc[(int)NPC.ai[2]].target].Center - NPC.Center;
                        Vector2 velocity = Vector2.Normalize(destination) * lightVelocity;
                        int type = NPCID.AncientLight;
                        float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * MathHelper.TwoPi / 60f;
                        int light = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0f, ai, velocity.X, velocity.Y, 255);
                        Main.npc[light].velocity = velocity;
                    }
                }
            }

            // Decide segment offset stuff.
            NPC aheadSegment = Main.npc[(int)NPC.ai[1]];
            Vector2 directionToNextSegment = aheadSegment.Center - NPC.Center;
            if (aheadSegment.rotation != NPC.rotation)
            {
                directionToNextSegment = directionToNextSegment.RotatedBy(MathHelper.WrapAngle(aheadSegment.rotation - NPC.rotation) * 0.08f);
                directionToNextSegment = directionToNextSegment.MoveTowards((aheadSegment.rotation - NPC.rotation).ToRotationVector2(), 1f);
            }

            NPC.rotation = directionToNextSegment.ToRotation() + MathHelper.PiOver2;
            NPC.Center = aheadSegment.Center - directionToNextSegment.SafeNormalize(Vector2.Zero) * NPC.scale * NPC.width;
            NPC.spriteDirection = (directionToNextSegment.X > 0).ToDirectionInt();
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 center = new Vector2(NPC.Center.X, NPC.Center.Y);
            Vector2 vector = center - screenPos;
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("CalamityMod/NPCs/AdultEidolonWyrm/AdultEidolonWyrmBodyGlow").Value, vector,
                new Microsoft.Xna.Framework.Rectangle?(NPC.frame), Color.White, NPC.rotation, NPC.frame.Size() * 0.5f, 1f, spriteEffects, 0f);
        }

        public override bool CheckActive() => false;

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 10; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hitDirection, -1f, 0, default, 1f);

                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("WyrmAdult2").Type, 1f);
                }
            }
        }

        public override void ModifyTypeName(ref string typeName)
        {
            // Move to zenith seed later
            if (Main.getGoodWorld)
            {
                typeName = "Jared";
            }
        }
    }
}
