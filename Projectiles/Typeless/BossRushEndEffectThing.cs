﻿using CalamityMod.Events;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Projectiles.Typeless
{
    public class BossRushEndEffectThing : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public ref float Time => ref Projectile.ai[0];
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = BossRushEvent.EndVisualEffectTime;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (Time == 25f)
                SoundEngine.PlaySound(BossRushEvent.VictorySound, Main.LocalPlayer.Center);

            Projectile.Center = Owner.Center;
            BossRushEvent.SyncEndTimer((int)Time);

            float currentShakePower = MathHelper.Lerp(1f, 20f, Utils.GetLerpValue(140f, 180f, Time, true) * Utils.GetLerpValue(10f, 40f, Projectile.timeLeft, true));
            Main.LocalPlayer.Calamity().GeneralScreenShakePower = currentShakePower;

            MoonlordDeathDrama.RequestLight(Utils.GetLerpValue(220f, 265f, Time, true) * Utils.GetLerpValue(10f, 30f, Projectile.timeLeft, true), Main.LocalPlayer.Center);

            Time++;
        }

        public override void Kill(int timeLeft)
        {
            BossRushEvent.End();
            Player p = Main.LocalPlayer;
            p.QuickSpawnItemDirect(p.GetSource_Misc("CalamityMod_BossRushRock"), ModContent.ItemType<Rock>());
            CalamityUtils.DisplayLocalizedText("Mods.CalamityMod.BossRushTierFiveEndText", BossRushEvent.XerocTextColor);
        }
    }
}
