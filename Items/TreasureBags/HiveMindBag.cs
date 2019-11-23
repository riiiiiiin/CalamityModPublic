using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.NPCs.HiveMind;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.TreasureBags
{
    public class HiveMindBag : ModItem
    {
        public override int BossBagNPC => ModContent.NPCType<HiveMindP2>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.rare = 9;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void OpenBossBag(Player player)
        {
            // Materials
            DropHelper.DropItem(player, ItemID.RottenChunk, 10, 20);
            DropHelper.DropItem(player, ItemID.DemoniteBar, 9, 14);
            DropHelper.DropItem(player, ModContent.ItemType<TrueShadowScale>(), 30, 40);
            DropHelper.DropItemCondition(player, ItemID.CursedFlame, Main.hardMode, 15, 30);

            // Weapons
            DropHelper.DropItemChance(player, ModContent.ItemType<PerfectDark>(), 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<LeechingDagger>(), 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<Shadethrower>(), 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<ShadowdropStaff>(), 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<ShaderainStaff>(), 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<DankStaff>(), 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<RotBall>(), 3, 50, 75);

            // Equipment
            DropHelper.DropItem(player, ModContent.ItemType<RottenBrain>());
            DropHelper.DropItemChance(player, ModContent.ItemType<FilthyGlove>(), 3);

            // Vanity
            DropHelper.DropItemChance(player, ModContent.ItemType<HiveMindMask>(), 7);
        }
    }
}
