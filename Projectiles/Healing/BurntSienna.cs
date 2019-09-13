﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Healing
{
    public class BurntSienna : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sienna");
		}

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
			projectile.velocity.X *= 0.95f;
			projectile.velocity.Y *= 0.95f;
			int num487 = (int)projectile.ai[0];
			Vector2 vector36 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
			float num489 = Main.player[num487].Center.X - vector36.X;
			float num490 = Main.player[num487].Center.Y - vector36.Y;
			float num491 = (float)Math.Sqrt((double)(num489 * num489 + num490 * num490));
			if (num491 < 50f && projectile.position.X < Main.player[num487].position.X + (float)Main.player[num487].width && projectile.position.X + (float)projectile.width > Main.player[num487].position.X && projectile.position.Y < Main.player[num487].position.Y + (float)Main.player[num487].height && projectile.position.Y + (float)projectile.height > Main.player[num487].position.Y)
			{
				if (projectile.owner == Main.myPlayer)
				{
					int num492 = 3;
					Main.player[num487].HealEffect(num492, false);
					Main.player[num487].statLife += num492;
					if (Main.player[num487].statLife > Main.player[num487].statLifeMax2)
					{
						Main.player[num487].statLife = Main.player[num487].statLifeMax2;
					}
					NetMessage.SendData(66, -1, -1, null, num487, (float)num492, 0f, 0f, 0, 0, 0);
				}
				projectile.Kill();
			}
			for (int num497 = 0; num497 < 1; num497++)
			{
				float num498 = projectile.velocity.X * 0.2f * (float)num497;
				float num499 = -(projectile.velocity.Y * 0.2f) * (float)num497;
				int num500 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 246, 0f, 0f, 100, default, 1f);
				Main.dust[num500].noGravity = true;
				Main.dust[num500].velocity *= 0f;
				Dust expr_154F9_cp_0 = Main.dust[num500];
				expr_154F9_cp_0.position.X = expr_154F9_cp_0.position.X - num498;
				Dust expr_15518_cp_0 = Main.dust[num500];
				expr_15518_cp_0.position.Y = expr_15518_cp_0.position.Y - num499;
			}
        }
    }
}
