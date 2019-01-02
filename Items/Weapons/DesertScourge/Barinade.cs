using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
//using TerrariaOverhaul;

namespace CalamityMod.Items.Weapons.DesertScourge
{
	public class Barinade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Barinade");
			Tooltip.SetDefault("Shoots electric bolt arrows that explode");
		}

	    public override void SetDefaults()
	    {
	        item.damage = 8;
	        item.ranged = true;
	        item.width = 30;
	        item.height = 44;
	        item.useTime = 20;
	        item.useAnimation = 20;
	        item.useStyle = 5;
	        item.noMelee = true;
	        item.knockBack = 2f;
	        item.value = 35000;
	        item.rare = 2;
	        item.UseSound = SoundID.Item5;
	        item.autoReuse = true;
	        item.shoot = 10;
	        item.shootSpeed = 14f;
	        item.useAmmo = 40;
	    }

        /*public void OverhaulInit()
        {
            this.SetTag("bow");
        }*/

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
	    	Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("BoltArrow"), damage, knockBack, player.whoAmI, 0.0f, 0.0f);
	    	return false;
		}
	}
}