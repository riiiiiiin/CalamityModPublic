﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Projectiles;

namespace CalamityMod.NPCs.Leviathan
{
	public class SirenIce : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Shield");
		}
		
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			aiType = -1;
			npc.canGhostHeal = false;
			npc.noTileCollide = true;
			npc.damage = 30;
			npc.width = 160; //324
			npc.height = 160; //216
			npc.defense = 10;
			npc.lifeMax = 650;
            if (CalamityWorld.bossRushActive)
            {
                npc.lifeMax = 400000;
            }
            npc.alpha = 255;
			npc.HitSound = SoundID.NPCHit5;
			npc.DeathSound = SoundID.NPCDeath7;
		}

        public override void AI()
        {
            if (npc.alpha > 100)
            {
                npc.alpha -= 2;
            }
            Player player = Main.player[npc.target];
            int num989 = (int)npc.ai[0];
            if (Main.npc[num989].active && Main.npc[num989].type == mod.NPCType("Siren"))
            {
                npc.rotation = Main.npc[num989].rotation;
                npc.spriteDirection = Main.npc[num989].direction;
                npc.velocity = Vector2.Zero;
                npc.position = Main.npc[num989].Center;
                npc.position.X = npc.position.X - (float)(npc.width / 2) + ((npc.spriteDirection == 1) ? -30f : 30f);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                npc.gfxOffY = Main.npc[num989].gfxOffY;
                Lighting.AddLight((int)npc.Center.X / 16, (int)npc.Center.Y / 16, 0f, 0.8f, 1.1f);
                return;
            }
            npc.life = 0;
            npc.HitEffect(0, 10.0);
            npc.active = false;
        }
		
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
            if (projectile.type != mod.ProjectileType("FlakKraken"))
            {
                if (projectile.penetrate == -1 && !projectile.minion)
                {
                    projectile.penetrate = 1;
                }
                else if (projectile.penetrate >= 1)
                {
                    projectile.penetrate = 1;
                }
            }
		}
		
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(BuffID.Frostburn, 240, true);
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 67, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				for (int k = 0; k < 25; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 67, hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
	}
}