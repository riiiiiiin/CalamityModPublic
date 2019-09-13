﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Boss
{
    public class CosmicFlameBurst : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Flame");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			aiType = 348;
			projectile.hostile = true;
			projectile.ignoreWater = true;
			projectile.penetrate = 1;
			cooldownSlot = 1;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.3f, 0f, 0.4f);
			if (projectile.localAI[0] == 0f)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 73);
				projectile.localAI[0] += 1f;
			}
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 4f)
			{
				for (int num468 = 0; num468 < 5; num468++)
				{
					int num469 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 100, default, 2f);
					Main.dust[num469].noGravity = true;
					Main.dust[num469].velocity *= 0f;
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 74);
			projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
			projectile.width = (projectile.height = 48);
			projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
			projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
			for (int num621 = 0; num621 < 5; num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 100, default, 2f);
				Main.dust[num622].velocity *= 3f;
				if (Main.rand.NextBool(2))
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for (int num623 = 0; num623 < 10; num623++)
			{
				int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 100, default, 3f);
				Main.dust[num624].noGravity = true;
				Main.dust[num624].velocity *= 5f;
				num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 100, default, 2f);
				Main.dust[num624].velocity *= 2f;
			}
			projectile.Damage();
		}
	}
}
