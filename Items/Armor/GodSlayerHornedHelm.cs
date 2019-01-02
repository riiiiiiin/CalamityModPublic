﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace CalamityMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHornedHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("God Slayer Horned Helm");
            Tooltip.SetDefault("+3 max minions");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 5000000;
            item.defense = 12; //96
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

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("GodSlayerChestplate") && legs.type == mod.ItemType("GodSlayerLeggings");
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.GetModPlayer<CalamityPlayer>(mod);
            modPlayer.godSlayer = true;
            modPlayer.godSlayerSummon = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(mod.BuffType("Mechworm")) == -1)
                {
                    player.AddBuff(mod.BuffType("Mechworm"), 3600, true);
                }
                if (player.ownedProjectileCounts[mod.ProjectileType("MechwormHead")] < 1)
                {
                    int owner = player.whoAmI;
                    int typeHead = mod.ProjectileType("MechwormHead");
                    int typeBody = mod.ProjectileType("MechwormBody");
                    int typeBody2 = mod.ProjectileType("MechwormBody2");
                    int typeTail = mod.ProjectileType("MechwormTail");
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == owner)
                        {
                            if (Main.projectile[i].type == typeHead || Main.projectile[i].type == typeTail || Main.projectile[i].type == typeBody ||
                                Main.projectile[i].type == typeBody2)
                            {
                                Main.projectile[i].Kill();
                            }
                        }
                    }
                    int maxMinionScale = player.maxMinions;
                    if (maxMinionScale > 10)
                    {
                        maxMinionScale = 10;
                    }
                    int damage = (int)(35 * ((player.minionDamage * 5 / 3) + ((player.minionDamage * 0.46f) * (maxMinionScale - 1))));
                    Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                    Vector2 value = Vector2.UnitX.RotatedBy((double)player.fullRotation, default(Vector2));
                    Vector2 vector3 = Main.MouseWorld - vector2;
                    float velX = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                    float velY = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                    if (player.gravDir == -1f)
                    {
                        velY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                    }
                    float dist = (float)Math.Sqrt((double)(velX * velX + velY * velY));
                    if ((float.IsNaN(velX) && float.IsNaN(velY)) || (velX == 0f && velY == 0f))
                    {
                        velX = (float)player.direction;
                        velY = 0f;
                        dist = 10f;
                    }
                    else
                    {
                        dist = 10f / dist;
                    }
                    velX *= dist;
                    velY *= dist;
                    int head = -1;
                    int tail = -1;
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == owner)
                        {
                            if (head == -1 && Main.projectile[i].type == typeHead)
                            {
                                head = i;
                            }
                            else if (tail == -1 && Main.projectile[i].type == typeTail)
                            {
                                tail = i;
                            }
                            if (head != -1 && tail != -1)
                            {
                                break;
                            }
                        }
                    }
                    if (head == -1 && tail == -1)
                    {
                        float num77 = Vector2.Dot(value, vector3);
                        if (num77 > 0f)
                        {
                            player.ChangeDir(1);
                        }
                        else
                        {
                            player.ChangeDir(-1);
                        }
                        velX = 0f;
                        velY = 0f;
                        vector2.X = (float)Main.mouseX + Main.screenPosition.X;
                        vector2.Y = (float)Main.mouseY + Main.screenPosition.Y;
                        int curr = Projectile.NewProjectile(vector2.X, vector2.Y, velX, velY, mod.ProjectileType("MechwormHead"), damage, 1f, owner);

                        int prev = curr;
                        curr = Projectile.NewProjectile(vector2.X, vector2.Y, velX, velY, mod.ProjectileType("MechwormBody"), damage, 1f, owner, (float)prev);

                        prev = curr;
                        curr = Projectile.NewProjectile(vector2.X, vector2.Y, velX, velY, mod.ProjectileType("MechwormBody2"), damage, 1f, owner, (float)prev);
                        Main.projectile[prev].localAI[1] = (float)curr;
                        Main.projectile[prev].netUpdate = true;

                        prev = curr;
                        curr = Projectile.NewProjectile(vector2.X, vector2.Y, velX, velY, mod.ProjectileType("MechwormTail"), damage, 1f, owner, (float)prev);
                        Main.projectile[prev].localAI[1] = (float)curr;
                        Main.projectile[prev].netUpdate = true;
                    }
                    else if (head != -1 && tail != -1)
                    {
                        int body = Projectile.NewProjectile(vector2.X, vector2.Y, velX, velY, mod.ProjectileType("MechwormBody"), damage, 1f, owner, Main.projectile[tail].ai[0]);
                        int back = Projectile.NewProjectile(vector2.X, vector2.Y, velX, velY, mod.ProjectileType("MechwormBody2"), damage, 1f, owner, (float)body);

                        Main.projectile[body].localAI[1] = (float)back;
                        Main.projectile[body].ai[1] = 1f;
                        Main.projectile[body].minionSlots = 0f;
                        Main.projectile[body].netUpdate = true;

                        Main.projectile[back].localAI[1] = (float)tail;
                        Main.projectile[back].netUpdate = true;
                        Main.projectile[back].minionSlots = 0f;
                        Main.projectile[back].ai[1] = 1f;

                        Main.projectile[tail].ai[0] = (float)back;
                        Main.projectile[tail].netUpdate = true;
                        Main.projectile[tail].ai[1] = 1f;
                    }
                }
            }
            player.setBonus = "65% increased minion damage\n" +
                "You will survive fatal damage and will be healed 150 HP if an attack would have killed you\n" +
                "This effect can only occur once every 45 seconds\n" +
                "While the cooldown for this effect is active you gain a 10% increase to all damage\n" +
                "Hitting enemies will summon god slayer phantoms\n" +
                "Summons a god-eating mechworm to fight for you";
            player.minionDamage += 0.65f;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CosmiliteBar", 14);
            recipe.AddIngredient(null, "NightmareFuel", 8);
            recipe.AddIngredient(null, "EndothermicEnergy", 8);
            recipe.AddTile(null, "DraedonsForge");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}