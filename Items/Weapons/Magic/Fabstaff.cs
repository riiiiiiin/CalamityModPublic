using CalamityMod.Items.Materials;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalamityMod.Items.Weapons.Magic
{
    public class Fabstaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fabstaff");
            Tooltip.SetDefault("Casts a bouncing beam that splits when enemies are near it");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 125;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 50;
            Item.width = 84;
            Item.height = 84;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;

            Item.value = CalamityGlobalItem.Rarity16BuyPrice;
            Item.Calamity().customRarity = CalamityRarity.HotPink;
            Item.Calamity().devItem = true;

            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FabRay>();
            Item.shootSpeed = 13.5f;
        }

        public override Vector2? HoldoutOrigin() => new Vector2(15, 15);

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.RainbowRod).AddIngredient(ModContent.ItemType<Phantoplasm>(), 10).AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5).AddTile(ModContent.TileType<DraedonsForge>()).Register();
        }
    }
}
