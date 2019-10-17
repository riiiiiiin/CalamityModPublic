using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Projectiles.Rogue
{
    public class EpidemicShredderProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Epidemic Shredder");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = 6;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 4;
            projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            projectile.rotation += Math.Sign(projectile.velocity.X) * MathHelper.ToRadians(10f);
            if (projectile.ai[0] > 0f)
            {
                projectile.ai[0] -= 1f;
            }
            if (projectile.timeLeft < 580f)
            {
                projectile.velocity = (projectile.velocity * 18f + projectile.DirectionTo(Main.player[projectile.owner].Center) * 18f) / 19f;
                if (Main.player[projectile.owner].Hitbox.Intersects(projectile.Hitbox))
                {
                    projectile.Kill();
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            CalamityGlobalProjectile.DrawCenteredAndAfterimage(projectile, lightColor, ProjectileID.Sets.TrailingMode[projectile.type], 3);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.penetrate > 1)
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                if (projectile.ai[0] == 0f)
                {
                    Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("PlagueSeeker"), (int)(projectile.damage * 0.1f), 2f, projectile.owner);
                    projectile.ai[0] = 12f; //0.2th of a second cooldown
                }
                projectile.penetrate--;
            }
            else
            {
                projectile.penetrate = -1;
                projectile.tileCollide = false;
            }
            return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] == 0f)
            {
                Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("PlagueSeeker"), (int)(projectile.damage * 0.1f), 2f, projectile.owner);
                projectile.ai[0] = 12f; //0.2th of a second cooldown
            }
            target.AddBuff(mod.BuffType("Plague"), 300);
        }
    }
}