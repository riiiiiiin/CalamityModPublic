﻿using CalamityMod.Dusts;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Weapons.Melee;
using Terraria.Audio;

namespace CalamityMod.NPCs.AcidRain
{
    public class NuclearTerror : ModNPC
    {
        public enum SpecialAttackState
        {
            DivergingBullets,
            ConeStreamOfBullets,
            ShotgunBurstOfBullets
        }

        public int AttackIndex = 0;
        public int DelayTime = 0;
        public bool Dying = false;
        public bool Walking = false;
        public float JumpTimer = 0f;
        public Vector2 ShootPosition;
        public Player Target => Main.player[NPC.target];
        public ref float AttackTime => ref NPC.ai[0];
        public ref float TeleportCountdown => ref NPC.ai[1];
        public Vector2 TeleportLocation
        {
            get => new Vector2(NPC.ai[2], NPC.ai[3]);
            set
            {
                NPC.ai[2] = value.X;
                NPC.ai[3] = value.Y;
            }
        }
        public ref float HorizontalCollisionCounterDelay => ref NPC.localAI[0];
        public ref float HorizontalCollisionSpamCounter => ref NPC.localAI[1];
        public static readonly SpecialAttackState[] PhaseArray = new SpecialAttackState[]
        {
            SpecialAttackState.ShotgunBurstOfBullets,
            SpecialAttackState.DivergingBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.ShotgunBurstOfBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.DivergingBullets,
            SpecialAttackState.ShotgunBurstOfBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.DivergingBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.ShotgunBurstOfBullets,
            SpecialAttackState.ConeStreamOfBullets,
            SpecialAttackState.DivergingBullets,
            SpecialAttackState.ShotgunBurstOfBullets,
            SpecialAttackState.ConeStreamOfBullets
        };
        public const int AttackCycleTime = 520;
        public const int SpecialAttackTime = 240;
        public const float TeleportTime = 60f;
        public const float TeleportFadeinTime = 10f;
        public const float TeleportCooldown = 60f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nuclear Terror");
            Main.npcFrameCount[NPC.type] = 14;
            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;

            NPC.width = 176;
            NPC.height = 138;
            NPC.aiStyle = AIType = -1;

            NPC.lifeMax = 90000;
            NPC.defense = 50;
            NPC.damage = 135;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.DR_NERD(0.3f);
            NPC.lavaImmune = false;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit56;
            NPC.DeathSound = SoundID.NPCDeath60;
            NPC.Calamity().VulnerableToHeat = false;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.Calamity().VulnerableToWater = false;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.dontTakeDamage);
            writer.Write(Dying);
            writer.Write(Walking);
            writer.Write(AttackIndex);
            writer.Write(DelayTime);
            writer.Write(JumpTimer);
            writer.WriteVector2(ShootPosition);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.dontTakeDamage = reader.ReadBoolean();
            Dying = reader.ReadBoolean();
            Walking = reader.ReadBoolean();
            AttackIndex = reader.ReadInt32();
            DelayTime = reader.ReadInt32();
            JumpTimer = reader.ReadSingle();
            ShootPosition = reader.ReadVector2();
        }

        public override void AI()
        {
            Lighting.AddLight(NPC.Center, (Dying ? Color.Lime.ToVector3() : Color.White.ToVector3()) * 2f);
            if (Dying)
                return;

            bool phase2 = NPC.life / (float)NPC.lifeMax < 0.5f;
            if (DelayTime > 0)
            {
                DelayTime--;
                NPC.velocity.X *= 0.9f;
                if (NPC.velocity.Y < 18f)
                    NPC.velocity.Y += 0.35f;
                return;
            }

            if (NPC.target < 0 || NPC.target >= 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
                NPC.netUpdate = true;
            }

            if (TeleportCountdown > -TeleportCooldown)
                TeleportEffects();

            NPC.defDamage = 170;
            NPC.damage = Dying ? 0 : NPC.defDamage;
            TeleportCheck();

            AttackTime++;
            float wrappedAttackTime = AttackTime % AttackCycleTime;

            Walking = false;

            // Teleport if spam-collisions are done, they are pretty good indicators of being stuck.
            if (NPC.collideX)
            {
                if (HorizontalCollisionCounterDelay > 0)
                    HorizontalCollisionSpamCounter++;
                HorizontalCollisionCounterDelay = 20f;
            }

            if (HorizontalCollisionCounterDelay > 0)
                HorizontalCollisionCounterDelay--;

            if (wrappedAttackTime < 240f)
            {
                if (NPC.velocity.Y == 0f)
                {
                    JumpTimer++;
                    NPC.velocity.X *= 0.8f;

                    // Jump towards the target if enough time has passed or they're not in this enemy's line of sight.
                    if (JumpTimer >= 70f || !Collision.CanHit(NPC.position, NPC.width, NPC.height, Target.position, Target.width, Target.height))
                    {
                        JumpTimer = 0f;
                        NPC.velocity.Y -= MathHelper.Clamp(Math.Abs(Target.Center.Y - NPC.Center.Y) / 12.5f, 8f, 18f);
                        NPC.velocity.X = NPC.SafeDirectionTo(Target.Center).X * 18f;
                        NPC.netUpdate = true;
                    }
                    else
                    {
                        if (Walking != Math.Abs(NPC.velocity.X) > 4f)
                        {
                            Walking = Math.Abs(NPC.velocity.X) > 4f;
                            NPC.netUpdate = true;
                        }

                        // Force a jump the next frame to overcome any horizontal obstacles if they exist.
                        if (NPC.collideX)
                        {
                            JumpTimer = 50;
                            NPC.netUpdate = true;
                        }

                        // Otherwise walk towards the target if they're not super close.
                        else if (Math.Abs(Target.Center.X - NPC.Center.X) > 125f)
                        {
                            NPC.velocity.X += Math.Sign(NPC.SafeDirectionTo(Target.Center).X) * 3f;
                            NPC.velocity.X = MathHelper.Clamp(NPC.velocity.X, -28f, 28f);
                        }

                        // If they are close though, slow down horizontally.
                        else
                            NPC.velocity.X *= 0.99f;
                    }
                }
                NPC.spriteDirection = (NPC.velocity.X < 0).ToDirectionInt();
            }
            else
            {
                NPC.velocity.X *= 0.96f;
                if (wrappedAttackTime == 255f)
                {
                    ShootPosition = Target.Center;
                    NPC.netUpdate = true;
                    NPC.spriteDirection = (ShootPosition.X - NPC.Center.X < 0).ToDirectionInt();
                }

                PerformSpecialAttack(wrappedAttackTime);

                if (wrappedAttackTime == AttackCycleTime - 1f)
                {
                    DelayTime = phase2 ? 45 : 75;
                    AttackIndex++;
                    AttackIndex %= PhaseArray.Length;
                }
            }
        }
        public void TeleportCheck()
        {
            float distanceFromTarget = NPC.Distance(Target.Center);
            bool targetIsFarOff = distanceFromTarget > 900f;
            bool targetNotInLineOfSight = !Collision.CanHit(NPC.position, NPC.width, NPC.height, Target.position, Target.width, Target.height);
            if (TeleportCountdown <= -TeleportCooldown)
            {
                if (distanceFromTarget <= 2700f && !(targetIsFarOff && targetNotInLineOfSight) && !StuckOnPlatform() && !NPC.wet && HorizontalCollisionSpamCounter <= 5f)
                    return;

                Point playerPositionTileCoords = Target.position.ToTileCoordinates();
                Point npcPositionTileCoords = NPC.position.ToTileCoordinates();

                for (int tries = 0; tries < 250; tries++)
                {
                    int maxTeleportDistance = 30 + tries / 3;
                    int x = Main.rand.Next(playerPositionTileCoords.X - maxTeleportDistance, playerPositionTileCoords.X + maxTeleportDistance);
                    int yStart = Main.rand.Next(playerPositionTileCoords.Y - maxTeleportDistance, playerPositionTileCoords.Y + maxTeleportDistance);

                    // Have a downward bias for the teleport if stuck on a platform above the target.
                    if (StuckOnPlatform())
                        yStart = Main.rand.Next(playerPositionTileCoords.Y, playerPositionTileCoords.Y + maxTeleportDistance * 4);

                    for (int y = yStart; y < playerPositionTileCoords.Y + maxTeleportDistance; y++)
                    {
                        Tile tileBelow = CalamityUtils.ParanoidTileRetrieval(x, y - 1);
                        bool veryCloseToTarget = Math.Abs(y - playerPositionTileCoords.Y) < 12 || Math.Abs(x - playerPositionTileCoords.X) < 12;
                        bool veryCloseToSelf = Math.Abs(y - npcPositionTileCoords.Y) < 12 || Math.Abs(x - npcPositionTileCoords.X) < 12;
                        bool solidGround = (Main.tileSolid[tileBelow.TileType] || Main.tileSolidTop[tileBelow.TileType]) && tileBelow.HasTile;
                        if (!veryCloseToTarget && !veryCloseToSelf && solidGround)
                        {
                            // If the below tile has lava or honey, skip it.
                            if (CalamityUtils.ParanoidTileRetrieval(x, y - 1).LiquidType != LiquidID.Water)
                                continue;

                            // If there's any tiles in the way, skip it.
                            if (Collision.SolidTiles(x - 12, x + 12, y - 7, y - 7))
                                continue;

                            // If there's any liquid near the tile, skip it.
                            for (int dy = y - 8; dy <= y + 8; dy++)
                            {
                                if (CalamityUtils.ParanoidTileRetrieval(x, dy).LiquidAmount > 0)
                                    goto Continue;
                            }

                            TeleportCountdown = TeleportTime;
                            TeleportLocation = new Vector2(x, y - 6f);
                            HorizontalCollisionSpamCounter = 0f;
                            NPC.netUpdate = true;

                            return;
                        Continue:
                            continue;
                        }
                    }
                }
            }
        }

        public void TeleportEffects()
        {
            if (TeleportCountdown > TeleportTime)
                TeleportCountdown = TeleportTime;
            TeleportCountdown--;
            if (TeleportCountdown >= 0f)
            {
                if (TeleportCountdown == 0f && TeleportLocation != Vector2.Zero)
                {
                    NPC.position = TeleportLocation.ToWorldCoordinates(8f, 0f) - NPC.Size;
                    NPC.netUpdate = true;
                    NPC.velocity = Vector2.Zero;
                }
                else
                {
                    NPC.Opacity = TeleportCountdown / TeleportTime;

                    int totalDust = (int)(30 * NPC.alpha / 255f);
                    for (int i = 0; i < totalDust; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.noGravity = true;
                        dust.velocity = NPC.DirectionFrom(dust.position) * 2f;
                        dust.scale = 1.6f;
                    }

                    // Fall and slow down horizontally.
                    NPC.velocity.X *= 0.95f;
                    if (NPC.velocity.Y < 18f)
                        NPC.velocity.Y += 0.35f;
                }
                return;
            }

            // Release some dust before going back to normal.
            if (TeleportCountdown >= -TeleportFadeinTime)
            {
                NPC.Opacity = TeleportCountdown / -TeleportFadeinTime;
                if (TeleportCountdown == -TeleportFadeinTime)
                {
                    for (int i = 0; i < 48; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.noGravity = true;
                        dust.velocity = NPC.DirectionFrom(dust.position) * Main.rand.NextFloat(2f, 3.6f);
                        dust.scale = 1.8f;
                    }
                }
            }
        }

        public bool StuckOnPlatform()
        {
            for (int i = -12; i < 12; i++)
            {
                Point bottom = (NPC.Bottom + Vector2.UnitY * i).ToTileCoordinates();
                if (TileID.Sets.Platforms[CalamityUtils.ParanoidTileRetrieval(bottom.X, bottom.Y).TileType] && Target.Top.Y > NPC.Bottom.Y + 48)
                    return true;
            }
            return false;
        }

        public void PerformSpecialAttack(float wrappedAttackTime)
        {
            Vector2 mouthPosition = NPC.Center - Vector2.UnitY * 26f;
            mouthPosition.X += NPC.spriteDirection * -54f;

            TeleportCountdown = -TeleportCooldown;
            Vector2 directionToTarget = (Target.Center - mouthPosition).SafeNormalize(Vector2.UnitX * NPC.spriteDirection);
            Vector2 directionToShootPosition = (ShootPosition - mouthPosition).SafeNormalize(Vector2.UnitX * NPC.spriteDirection);
            switch (PhaseArray[AttackIndex])
            {
                case SpecialAttackState.DivergingBullets:
                    NPC.velocity.X *= 0.9f;
                    float shootAdjustedTime = wrappedAttackTime - (AttackCycleTime - SpecialAttackTime);
                    bool shouldShoot = shootAdjustedTime >= 20f;
                    shouldShoot &= (shootAdjustedTime - 20f) % 30f < 12f;
                    shouldShoot &= shootAdjustedTime <= 152f;
                    shouldShoot &= shootAdjustedTime % 3f == 0f;

                    if (shouldShoot)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float angle = (wrappedAttackTime - (AttackCycleTime - SpecialAttackTime + 20f)) % 12f / 12f * MathHelper.ToRadians(15f) - MathHelper.ToRadians(7.5f);
                            int bullet = Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), mouthPosition, directionToShootPosition.RotatedBy(angle) * 14f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 4f);
                            Main.projectile[bullet].localAI[0] = angle;
                        }
                        NPC.spriteDirection = (ShootPosition.X - NPC.Center.X < 0).ToDirectionInt();
                        SoundEngine.PlaySound(SoundID.NPCDeath13, mouthPosition);
                    }
                    if (wrappedAttackTime >= (AttackCycleTime - SpecialAttackTime + 35f) && AttackTime % 10f == 9f)
                        Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), mouthPosition, directionToTarget * 12f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 3f);
                    break;
                case SpecialAttackState.ConeStreamOfBullets:
                    if (wrappedAttackTime >= AttackCycleTime - SpecialAttackTime + 35f && AttackTime % 4f == 3f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            float angle = MathHelper.Lerp(MathHelper.ToRadians(35f), MathHelper.ToRadians(5f), (wrappedAttackTime - (AttackCycleTime - SpecialAttackTime + 35f)) / (SpecialAttackTime + 35f));
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), mouthPosition, directionToTarget.RotatedBy(angle) * 16f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 4.5f);
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), mouthPosition, directionToTarget.RotatedBy(-angle) * 16f, ModContent.ProjectileType<NuclearBulletLarge>(), 48, 4.5f);
                        }
                        NPC.spriteDirection = (Target.Center.X - NPC.Center.X < 0).ToDirectionInt();
                    }
                    break;
                case SpecialAttackState.ShotgunBurstOfBullets:
                    if (wrappedAttackTime >= AttackCycleTime - SpecialAttackTime + 35f && AttackTime % 20f == 19f)
                    {
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                float angle = MathHelper.Lerp(-0.5f, 0.5f, i / 3f);
                                Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), mouthPosition, directionToShootPosition.RotatedBy(angle) * 13f, ModContent.ProjectileType<NuclearBulletMedium>(), 48, 4f);
                            }
                        }
                        NPC.spriteDirection = (ShootPosition.X - NPC.Center.X < 0).ToDirectionInt();
                    }
                    break;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.8f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.85f);
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            int frameChangeRate = Dying ? 7 : 6;

            // Walk faster the faster this thing is moving.
            if (Walking)
                frameChangeRate = 8 - (int)Math.Ceiling(Math.Abs(NPC.velocity.X) / 5f);

            if (NPC.frameCounter >= frameChangeRate)
            {
                NPC.frame.Y += frameHeight;
                NPC.frameCounter = 0;
            }
            if (Dying)
            {
                if (NPC.frame.Y < frameHeight * 8)
                {
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int type = Main.rand.NextBool(4) ? ModContent.ProjectileType<SulphuricAcidMist>() : ModContent.ProjectileType<NuclearBulletLarge>();
                            float angle = MathHelper.TwoPi / 16f * i;
                            Projectile.NewProjectile(NPC.GetSpawnSource_ForProjectile(), NPC.Center, angle.ToRotationVector2() * Main.rand.NextFloat(4f, 11f), type, 48, 3f);
                        }
                    }
                    for (int i = 0; i < 60; i++)
                    {
                        Dust dust = Dust.NewDustDirect(NPC.Center, 45, 45, (int)CalamityDusts.SulfurousSeaAcid);
                        dust.velocity = Utils.NextVector2Unit(Main.rand) * Main.rand.NextFloat(4f, 15f);
                        dust.noGravity = true;
                        dust.scale = Main.rand.NextFloat(2f, 3f);
                    }
                    NPC.frame.Y = frameHeight * 8;
                }
                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                    NPC.StrikeNPCNoInteraction(9999, 0f, 0);
            }
            else if (NPC.frame.Y >= (Walking ? 8 : 4) * frameHeight)
                NPC.frame.Y = Walking ? 4 * frameHeight : 0;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.velocity.Length() > 0f)
            {
                Color endColor = Color.DarkOliveGreen;
                endColor.A = Color.Transparent.A;
                CalamityGlobalNPC.DrawAfterimage(NPC, spriteBatch, drawColor, endColor, directioning: true, invertedDirection: true);
            }
            CalamityGlobalNPC.DrawGlowmask(NPC, spriteBatch, null, true);
            return false;
        }

        public override bool CheckDead()
        {
            if (!Dying)
            {
                Dying = true;
                NPC.active = true;
                NPC.life = 1;
                NPC.dontTakeDamage = true;
                NPC.velocity = Vector2.Zero;
                NPC.netUpdate = true;
                return false;
            }
            return Dying;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 10; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.SulfurousSeaAcid, hitDirection, -1f, 0, default, 1f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(ModContent.BuffType<Irradiated>(), 300);

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<GammaHeart>(), 3);
            npcLoot.Add(ModContent.ItemType<PhosphorescentGauntlet>(), 3);
        }
    }
}
