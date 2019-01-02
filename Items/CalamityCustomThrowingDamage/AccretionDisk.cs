using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.CalamityCustomThrowingDamage
{
	public class AccretionDisk : CalamityDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Disk");
			Tooltip.SetDefault("Shred the fabric of reality!");
		}

		public override void SafeSetDefaults()
		{
			item.width = 38;
			item.damage = 157;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.useTime = 15;
			item.knockBack = 9f;
			item.UseSound = SoundID.Item1;
			item.height = 38;
			item.value = 10000000;
			item.shoot = mod.ProjectileType("AccretionDisk");
			item.shootSpeed = 13f;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MangroveChakram");
			recipe.AddIngredient(null, "FlameScythe");
			recipe.AddIngredient(null, "SeashellBoomerang");
			recipe.AddIngredient(null, "GalacticaSingularity", 5);
			recipe.AddIngredient(null, "BarofLife", 5);
			recipe.AddIngredient(ItemID.LunarBar, 5);
	        recipe.AddTile(TileID.LunarCraftingStation);
	        recipe.SetResult(this);
	        recipe.AddRecipe();
		}
	}
}
