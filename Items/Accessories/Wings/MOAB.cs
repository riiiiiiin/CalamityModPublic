﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class MOAB : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MOAB");
            Tooltip.SetDefault("The mother of all balloons\n" +
                "Counts as wings\n" +
                "Horizontal speed: 6.5\n" +
                "Acceleration multiplier: 1\n" +
                "Good vertical speed\n" +
                "Flight time: 75");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.value = Item.buyPrice(0, 39, 99, 99);
            item.rare = 8;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.controlJump && player.wingTime > 0f && !player.jumpAgainCloud && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                player.rocketDelay2--;
                if (player.rocketDelay2 <= 0)
                {
                    Main.PlaySound(SoundID.Item13, player.position);
                    player.rocketDelay2 = 60;
                }
                int num66 = 2;
                if (player.controlUp)
                {
                    num66 = 4;
                }
                for (int num67 = 0; num67 < num66; num67++)
                {
                    int type = 6;
                    if (player.head == 41)
                    {
                        int arg_58FD_0 = player.body;
                    }
                    float scale = 1.75f;
                    int alpha = 100;
                    float x = player.position.X + (float)(player.width / 2) + 16f;
                    if (player.direction > 0)
                    {
                        x = player.position.X + (float)(player.width / 2) - 26f;
                    }
                    float num68 = player.position.Y + (float)player.height - 18f;
                    if (num67 == 1 || num67 == 3)
                    {
                        x = player.position.X + (float)(player.width / 2) + 8f;
                        if (player.direction > 0)
                        {
                            x = player.position.X + (float)(player.width / 2) - 20f;
                        }
                        num68 += 6f;
                    }
                    if (num67 > 1)
                    {
                        num68 += player.velocity.Y;
                    }
                    int num69 = Dust.NewDust(new Vector2(x, num68), 8, 8, type, 0f, 0f, alpha, default, scale);
                    Dust expr_5A11_cp_0_cp_0 = Main.dust[num69];
                    expr_5A11_cp_0_cp_0.velocity.X *= 0.1f;
                    Main.dust[num69].velocity.Y = Main.dust[num69].velocity.Y * 1f + 2f * player.gravDir - player.velocity.Y * 0.3f;
                    Main.dust[num69].noGravity = true;
                    Main.dust[num69].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                    if (num66 == 4)
                    {
                        Dust expr_5AA6_cp_0_cp_0 = Main.dust[num69];
                        expr_5AA6_cp_0_cp_0.velocity.Y += 6f;
                    }
                }
            }
            player.doubleJumpCloud = true;
            player.doubleJumpSandstorm = true;
            player.doubleJumpBlizzard = true;
            player.jumpBoost = true;
            player.autoJump = true;
            player.noFallDmg = true;
            player.jumpSpeedBoost += 0.8f;
            player.wingTimeMax = 75;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.75f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 2.5f;
            constantAscend = 0.125f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 6.5f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FrogLeg);
            recipe.AddIngredient(ItemID.BundleofBalloons);
            recipe.AddIngredient(ItemID.LuckyHorseshoe);
            recipe.AddIngredient(ItemID.Jetpack);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
