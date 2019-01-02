using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.Weapons
{
    public class TeardropCleaver : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Teardrop Cleaver");
            Tooltip.SetDefault("Makes your enemies cry");
        }

        public override void SetDefaults()
        {
            item.width = 52;
            item.damage = 18;
            item.melee = true;
            item.useAnimation = 24;
            item.useStyle = 1;
            item.useTime = 24;
            item.useTurn = true;
            item.knockBack = 4.5f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.height = 56;
            item.value = 50000;
            item.rare = 2;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("TemporalSadness"), 60);
        }
    }
}
