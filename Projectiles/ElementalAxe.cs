﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles
{
    public class ElementalAxe : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Axe");
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
    	
        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 52;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 18000;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.minion = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
        	bool flag64 = (projectile.type == mod.ProjectileType("ElementalAxe"));
			Player player = Main.player[projectile.owner];
			CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>(mod);
			if (flag64)
			{
				if (player.dead)
				{
					modPlayer.eAxe = false;
				}
				if (modPlayer.eAxe)
				{
					projectile.timeLeft = 2;
				}
			}
			projectile.rotation += 0.075f;
			float num633 = 700f;
			float num634 = 1200f;
			float num635 = 2500f;
			float num636 = 400f;
			float num637 = 0.05f;
			for (int num638 = 0; num638 < 1000; num638++)
			{
				bool flag23 = (Main.projectile[num638].type == mod.ProjectileType("ElementalAxe"));
				if (num638 != projectile.whoAmI && Main.projectile[num638].active && Main.projectile[num638].owner == projectile.owner && flag23 && Math.Abs(projectile.position.X - Main.projectile[num638].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num638].position.Y) < (float)projectile.width)
				{
					if (projectile.position.X < Main.projectile[num638].position.X)
					{
						projectile.velocity.X = projectile.velocity.X - num637;
					}
					else
					{
						projectile.velocity.X = projectile.velocity.X + num637;
					}
					if (projectile.position.Y < Main.projectile[num638].position.Y)
					{
						projectile.velocity.Y = projectile.velocity.Y - num637;
					}
					else
					{
						projectile.velocity.Y = projectile.velocity.Y + num637;
					}
				}
			}
			bool flag24 = false;
			if (projectile.ai[0] == 2f)
			{
				projectile.ai[1] += 1f;
				projectile.extraUpdates = 2;
				if (projectile.ai[1] > 30f)
				{
					projectile.ai[1] = 1f;
					projectile.ai[0] = 0f;
					projectile.extraUpdates = 1;
					projectile.numUpdates = 0;
					projectile.netUpdate = true;
				}
				else
				{
					flag24 = true;
				}
			}
			if (flag24)
			{
				return;
			}
			Vector2 vector46 = projectile.position;
			bool flag25 = false;
			for (int num645 = 0; num645 < 200; num645++)
			{
				NPC nPC2 = Main.npc[num645];
				if (nPC2.CanBeChasedBy(projectile, false))
				{
					float num646 = Vector2.Distance(nPC2.Center, projectile.Center);
					if (((Vector2.Distance(projectile.Center, vector46) > num646 && num646 < num633) || !flag25))
					{
						num633 = num646;
						vector46 = nPC2.Center;
						flag25 = true;
					}
				}
			}
			float num647 = num634;
			if (flag25)
			{
				num647 = num635;
			}
			if (Vector2.Distance(player.Center, projectile.Center) > num647)
			{
				projectile.ai[0] = 1f;
				projectile.netUpdate = true;
			}
			if (flag25 && projectile.ai[0] == 0f)
			{
				Vector2 vector47 = vector46 - projectile.Center;
				float num648 = vector47.Length();
				vector47.Normalize();
				if (num648 > 200f)
				{
					float scaleFactor2 = 24f; //8
					vector47 *= scaleFactor2;
					projectile.velocity = (projectile.velocity * 40f + vector47) / 41f;
				}
				else
				{
					float num649 = 12f; //4
					vector47 *= -num649;
					projectile.velocity = (projectile.velocity * 40f + vector47) / 41f; //41
				}
			}
			else
			{
				bool flag26 = false;
				if (!flag26)
				{
					flag26 = (projectile.ai[0] == 1f);
				}
				float num650 = 6f;
				if (flag26)
				{
					num650 = 15f;
				}
				Vector2 center2 = projectile.Center;
				Vector2 vector48 = player.Center - center2 + new Vector2(0f, -60f);
				float num651 = vector48.Length();
				if (num651 > 200f && num650 < 8f)
				{
					num650 = 8f;
				}
				if (num651 < num636 && flag26 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					projectile.netUpdate = true;
				}
				if (num651 > 2000f)
				{
					projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
					projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.height / 2);
					projectile.netUpdate = true;
				}
				if (num651 > 70f)
				{
					vector48.Normalize();
					vector48 *= num650;
					projectile.velocity = (projectile.velocity * 40f + vector48) / 41f;
				}
				else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
				{
					projectile.velocity.X = -0.15f;
					projectile.velocity.Y = -0.05f;
				}
			}
			if (projectile.ai[1] > 0f)
			{
				projectile.ai[1] += (float)Main.rand.Next(1, 4);
			}
			if (projectile.ai[1] > 30f)
			{
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
			if (projectile.ai[0] == 0f)
			{
				if (projectile.ai[1] == 0f && flag25 && num633 < 500f)
				{
					projectile.ai[1] += 1f;
					if (Main.myPlayer == projectile.owner)
					{
						projectile.ai[0] = 2f;
						Vector2 value20 = vector46 - projectile.Center;
						value20.Normalize();
						projectile.velocity = value20 * 16f; //8
						projectile.netUpdate = true;
						return;
					}
				}
			}
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, projectile.alpha);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor), projectile.rotation, tex.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        	target.immune[projectile.owner] = 4;
            target.AddBuff(mod.BuffType("BrimstoneFlames"), 100);
            target.AddBuff(mod.BuffType("GlacialState"), 100);
            target.AddBuff(mod.BuffType("Plague"), 100);
            target.AddBuff(mod.BuffType("HolyLight"), 100);
        }
    }
}