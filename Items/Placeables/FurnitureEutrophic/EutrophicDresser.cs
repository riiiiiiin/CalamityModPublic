﻿using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Placeables.FurnitureEutrophic
{
    public class EutrophicDresser : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Placeables";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.FurnitureEutrophic.EutrophicDresser>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Navystone>(16).
                AddTile<EutrophicShelf>().
                Register();
        }
    }
}
