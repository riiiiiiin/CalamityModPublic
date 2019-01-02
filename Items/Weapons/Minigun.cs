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
	public class Minigun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Minigun");
			Tooltip.SetDefault("80% chance to not consume ammo");
		}

	    public override void SetDefaults()
	    {
	        item.damage = 390;
	        item.ranged = true;
	        item.width = 72;
	        item.height = 34;
	        item.useTime = 3;
	        item.useAnimation = 3;
	        item.useStyle = 5;
	        item.noMelee = true;
	        item.knockBack = 2.5f;
	        item.value = 1750000;
	        item.UseSound = SoundID.Item41;
	        item.autoReuse = true;
	        item.shoot = 10;
	        item.shootSpeed = 22f;
	        item.useAmmo = 97;
	    }
	    
	    public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	    
	    public override void ModifyTooltips(List<TooltipLine> list)
	    {
	        foreach (TooltipLine line2 in list)
	        {
	            if (line2.mod == "Terraria" && line2.Name == "ItemName")
	            {
	                line2.overrideColor = new Color(43, 96, 222);
	            }
	        }
	    }
	    
	    public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
		    float SpeedX = speedX + (float) Main.rand.Next(-15, 16) * 0.05f;
		    float SpeedY = speedY + (float) Main.rand.Next(-15, 16) * 0.05f;
		    Projectile.NewProjectile(position.X, position.Y, SpeedX, SpeedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
		    return false;
		}
	    
	    public override bool ConsumeAmmo(Player player)
	    {
	    	if (Main.rand.Next(0, 100) <= 80)
	    		return false;
	    	return true;
	    }
	
	    public override void AddRecipes()
	    {
	        ModRecipe recipe = new ModRecipe(mod);
	        recipe.AddIngredient(ItemID.ChainGun);
	        recipe.AddIngredient(null, "CosmiliteBar", 5);
	        recipe.AddIngredient(null, "Phantoplasm", 5);
	        recipe.AddTile(null, "DraedonsForge");
	        recipe.SetResult(this);
	        recipe.AddRecipe();
	    }
	}
}