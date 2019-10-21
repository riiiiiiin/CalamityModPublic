using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class OmegaBlueLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omega Blue Leggings");
            Tooltip.SetDefault(@"30% increased movement speed
12% increased damage and 8% increased critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 35, 25, 0);
            item.rare = 10;
            item.defense = 22;
            item.Calamity().postMoonLordRarity = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.allDamage += 0.12f;
            player.Calamity().AllCritBoost(8);

            player.moveSpeed += 0.3f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 13);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 6);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 3);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
