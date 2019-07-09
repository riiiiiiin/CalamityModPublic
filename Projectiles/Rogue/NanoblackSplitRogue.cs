﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles;

namespace CalamityMod.Projectiles.Rogue
{
    public class NanoblackSplitRogue : ModProjectile
    {
        private static int SpriteWidth = 52;
        private static float MaxRotationSpeed = 0.25f;
        private static int Lifetime = 90;
        private static float HomingStartRange = 100f;
        private static float HomingBreakRange = 600f;
        private static float HomingBonusRange = 100f;
        private static float MaxSpeed = 12f;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nanoblack Blade");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.GetGlobalProjectile<CalamityGlobalProjectile>(mod).rogue = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = 6;
            projectile.extraUpdates = 1;
            projectile.timeLeft = Lifetime;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
        }

        // ai[0] = Index of current NPC target. If 0 or negative, the projectile has no target
        // ai[1] = Current spin speed. Negative speeds are also allowed.
        public override void AI()
        {
            drawOffsetX = -10;
            drawOriginOffsetY = 0;
            drawOriginOffsetX = 0;

            // On the very first frame, clear any invalid starting target variable and create some dust.
            if (projectile.timeLeft == Lifetime)
            {
                projectile.ai[0] = 0f;
                SpawnDust();
            }

            // Spin in the specified starting direction and slow down spin over time
            // Loses 1.66% of current speed every frame
            // Also update current orientation to reflect current spin direction
            float currentSpin = projectile.ai[1];
            projectile.direction = (currentSpin <= 0f) ? 1 : -1;
            projectile.spriteDirection = projectile.direction;
            projectile.rotation += currentSpin * MaxRotationSpeed;
            float spinReduction = 0.0166f * currentSpin;
            projectile.ai[1] -= spinReduction;

            // If about to disappear, shrink by 8% every frame
            if(projectile.timeLeft < 15)
                projectile.scale *= 0.92f;

            // Search for and home in on nearby targets
            HomingAI();
        }

        private void HomingAI()
        {
            // If we don't currently have a target, go try and get one!
            int targetID = (int)(projectile.ai[0]) - 1;
            if (targetID < 0)
                targetID = AcquireTarget();

            // Save the target, whether we have one or not.
            projectile.ai[0] = targetID + 1f;

            // If we don't have a target, then just slow down a bit.
            if (targetID < 0)
            {
                projectile.velocity *= 0.94f;
                return;
            }

            // Homing behavior depends on how far the blade is from its target.
            NPC target = Main.npc[targetID];
            float xDist = projectile.Center.X - target.Center.X;
            float yDist = projectile.Center.Y - target.Center.Y;
            float dist = (float)Math.Sqrt(xDist * xDist + yDist * yDist);

            // If the target is too far away, stop homing in on it.
            if(dist > HomingBreakRange)
            {
                projectile.ai[0] = 0f;
                return;
            }

            // Adds a multiple of the towards-target vector to its velocity every frame.
            float homingFactor = CalcHomingFactor(dist);
            Vector2 posDiff = target.Center - projectile.Center;
            posDiff = posDiff.SafeNormalize(Vector2.Zero);
            posDiff *= homingFactor;
            Vector2 newVelocity = projectile.velocity += posDiff;

            // Caps speed to make sure it doesn't go too fast.
            if(newVelocity.Length() >= MaxSpeed)
            {
                newVelocity = newVelocity.SafeNormalize(Vector2.Zero);
                newVelocity *= MaxSpeed;
            }

            projectile.velocity = newVelocity;
        }

        // Returns the ID of the NPC to be targeted by this energy blade.
        // It chooses the closest target which can be chased, ignoring invulnerable NPCs.
        // Nanoblack Blades prefer to target bosses whenever possible.
        private int AcquireTarget()
        {
            bool bossFound = false;
            int target = -1;
            float minDist = HomingStartRange * 2f;
            for (int i = 0; i < 200; ++i)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.type == NPCID.TargetDummy)
                    continue;

                // If we've found a valid boss target, ignore ALL targets which aren't bosses.
                if (bossFound && !npc.boss)
                    continue;

                if (npc.CanBeChasedBy(projectile, false))
                {
                    float xDist = projectile.Center.X - npc.Center.X;
                    float yDist = projectile.Center.Y - npc.Center.Y;
                    float distToNPC = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
                    if (distToNPC < minDist)
                    {
                        // If this target within range is a boss, set the boss found flag.
                        if (npc.boss)
                            bossFound = true;
                        minDist = distToNPC;
                        target = i;
                    }
                }
            }
            return target;
        }

        // Energy blades home more aggressively the closer they are to their target.
        // The homing factor ranges from 0.6 to 3.6 at point blank.
        private float CalcHomingFactor(float dist)
        {
            float baseFactor = 0.6f;
            float bonus = 3f * (1f - dist / HomingBonusRange);
            if (bonus < 0f)
                bonus = 0f;
            return baseFactor + bonus;
        }

        // Draws the energy blade's glowmask.
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float fWidthOverTwo = SpriteWidth / 2f;
            float fHeightOverTwo = projectile.height / 2f;

            // Make sure the glowmask matches the blade's own orientation
            SpriteEffects eff = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
                eff = SpriteEffects.FlipHorizontally;
            Vector2 origin = new Vector2(fWidthOverTwo, fHeightOverTwo);
            spriteBatch.Draw(mod.GetTexture("Projectiles/Rogue/NanoblackSplitRogueGlow"),
                projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation,
                origin, projectile.scale, eff, 0f);
        }

        // Spawns a tiny bit of dust when the energy blade vanishes.
        public override void Kill(int timeLeft)
        {
            SpawnDust();
        }

        // Spawns a small bit of Luminite themed dust.
        private void SpawnDust()
        {
            int dustCount = Main.rand.Next(3, 6);
            Vector2 corner = projectile.position;
            for (int i = 0; i < dustCount; ++i)
            {
                int dustType = 229;
                float scale = 0.6f + Main.rand.NextFloat(0.4f);
                int idx = Dust.NewDust(corner, projectile.width, projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = scale;
            }
        }
    }
}
