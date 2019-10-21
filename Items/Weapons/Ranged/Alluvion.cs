using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Weapons.Ranged
{
    public class Alluvion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Alluvion");
            Tooltip.SetDefault("Moderate chance to convert wooden arrows to sharks\n" +
                       "Low chance to convert wooden arrows to typhoon arrows\n" +
                        "Fires a torrent of ten arrows at once");
        }

        public override void SetDefaults()
        {
            item.damage = 90;
            item.ranged = true;
            item.width = 60;
            item.height = 90;
            item.useTime = 9;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.buyPrice(1, 80, 0, 0);
            item.rare = 10;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 1;
            item.shootSpeed = 17f;
            item.useAmmo = 40;
            item.Calamity().postMoonLordRarity = 14;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num117 = 0.314159274f;
            int num118 = 10;
            Vector2 vector7 = new Vector2(speedX, speedY);
            vector7.Normalize();
            vector7 *= 20f;
            bool flag11 = Collision.CanHit(vector2, 0, 0, vector2 + vector7, 0, 0);
            for (int num119 = 0; num119 < num118; num119++)
            {
                float num120 = (float)num119 - ((float)num118 - 1f) / 2f;
                Vector2 value9 = vector7.RotatedBy((double)(num117 * num120), default);
                if (!flag11)
                {
                    value9 -= vector7;
                }
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    if (Main.rand.NextBool(12))
                    {
                        type = ModContent.ProjectileType<TorrentialArrow>();
                    }
                    if (Main.rand.NextBool(25))
                    {
                        type = 408;
                    }
                    if (Main.rand.NextBool(100))
                    {
                        type = ModContent.ProjectileType<TyphoonArrow>();
                    }
                    int num121 = Projectile.NewProjectile(vector2.X + value9.X, vector2.Y + value9.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[num121].Calamity().forceRanged = true;
                    Main.projectile[num121].noDropItem = true;
                }
                else
                {
                    int num121 = Projectile.NewProjectile(vector2.X + value9.X, vector2.Y + value9.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[num121].noDropItem = true;
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Monsoon>());
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<DepthCells>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 20);
            recipe.AddIngredient(ModContent.ItemType<Tenebris>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
