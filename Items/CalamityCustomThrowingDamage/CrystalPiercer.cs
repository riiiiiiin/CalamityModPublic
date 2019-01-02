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
	public class CrystalPiercer : CalamityDamageItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Piercer");
		}

		public override void SafeSetDefaults()
		{
			item.width = 62;
			item.damage = 43;
			item.noMelee = true;
			item.consumable = true;
			item.noUseGraphic = true;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.useTime = 20;
			item.knockBack = 6f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.height = 62;
			item.maxStack = 999;
			item.value = 2500;
			item.rare = 5;
			item.shoot = mod.ProjectileType("CrystalPiercerProjectile");
			item.shootSpeed = 20f;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(null, "VerstaltiteBar");
	        recipe.AddTile(TileID.MythrilAnvil);
	        recipe.SetResult(this, 20);
	        recipe.AddRecipe();
		}
	}
}
