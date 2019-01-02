﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Armor;
using CalamityMod.Items.CalamityCustomThrowingDamage;

namespace CalamityMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class TarragonLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tarragon Leggings");
            Tooltip.SetDefault("20% increased movement speed; greater speed boost if health is lower\n" +
                "6% increased damage and critical strike chance\n" +
                "Leggings of a fabled explorer");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 1462500;
            item.defense = 32;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(0, 255, 200);
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.20f;
            if (player.statLife <= (int)((double)player.statLifeMax2 * 0.5))
            {
                player.moveSpeed += 0.15f;
            }
            player.meleeDamage += 0.06f;
            player.meleeCrit += 6;
            player.magicDamage += 0.06f;
            player.magicCrit += 6;
            player.rangedDamage += 0.06f;
            player.rangedCrit += 6;
            CalamityCustomThrowingDamagePlayer.ModPlayer(player).throwingDamage += 0.06f;
            CalamityCustomThrowingDamagePlayer.ModPlayer(player).throwingCrit += 6;
            player.minionDamage += 0.06f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UeliaceBar", 11);
            recipe.AddIngredient(null, "DivineGeode", 12);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}