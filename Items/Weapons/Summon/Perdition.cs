using Terraria.DataStructures;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace CalamityMod.Items.Weapons.Summon
{
    public class Perdition : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Perdition");
            Tooltip.SetDefault("Summons a beacon at the position of the mouse\n" +
                "When a target is manually selected via right click it releases torrents of souls from below onto the target\n" +
                "Only one beacon may exist at a time");
        }

        public override void SetDefaults()
        {
            Item.damage = 444;
            Item.mana = 10;
            Item.width = Item.height = 56;
            Item.useTime = Item.useAnimation = 10; // 9 because of useStyle 1
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.DD2_EtherianPortalOpen;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PerditionBeacon>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;

            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ItemRarityID.Purple;
            Item.Calamity().customRarity = CalamityRarity.Violet;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                CalamityUtils.OnlyOneSentry(player, type);
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
                player.UpdateMaxTurrets();
            }
            else
            {
                // Play some demonic noises prior to a target being selected.
                SoundEngine.PlaySound(SoundID.Zombie, player.Center, 93);
                SoundEngine.PlaySound(SoundID.Item119, player.Center);
            }
            return false;
        }
    }
}
