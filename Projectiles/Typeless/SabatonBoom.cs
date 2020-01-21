using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.DamageOverTime;
namespace CalamityMod.Projectiles.Typeless
{
    public class SabatonBoom : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 450;
            projectile.height = 450;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 40;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
        }
    }
}
