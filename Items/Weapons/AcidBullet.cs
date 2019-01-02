using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.Weapons
{
	public class AcidBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Round");
			Tooltip.SetDefault("Explodes into acid that inflicts the plague\n" +
                "Does more damage the higher the target's defense");
		}

		public override void SetDefaults()
		{
			item.damage = 36;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1.5f;
			item.value = 1250;
			item.rare = 8;
			item.shoot = mod.ProjectileType("AcidBullet");
			item.shootSpeed = 10f;
			item.ammo = 97;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MusketBall, 150);
			recipe.AddIngredient(null, "PlagueCellCluster");
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}
}