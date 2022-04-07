using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Critters;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Tiles.Astral;
using CalamityMod.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Fishing.AstralCatches
{
    public class AstralCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Crate");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 1);
            Item.createTile = ModContent.TileType<AstralCrateTile>();
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            //Modded materials
            DropHelper.DropItem(player, ModContent.ItemType<Stardust>(), 5, 10);
            DropHelper.DropItem(player, ItemID.FallenStar, 5, 10);
            DropHelper.DropItemChance(player, ItemID.Meteorite, 0.2f, 10, 20);
            DropHelper.DropItemChance(player, ItemID.MeteoriteBar, 0.1f, 1, 3);
            DropHelper.DropItemCondition(player, ModContent.ItemType<AstralJelly>(), DownedBossSystem.downedAstrumAureus, 0.2f, 2, 5);
            DropHelper.DropItemCondition(player, ModContent.ItemType<AstralOre>(), DownedBossSystem.downedAstrumDeus, 0.2f, 10, 20);
            DropHelper.DropItemCondition(player, ModContent.ItemType<AstralBar>(), DownedBossSystem.downedAstrumDeus, 0.1f, 1, 3);
            DropHelper.DropItemCondition(player, ModContent.ItemType<MeldBlob>(), DownedBossSystem.downedAstrumDeus, 0.25f, 5, 10);

            // Weapons
            DropHelper.DropItemFromSetCondition(player, DownedBossSystem.downedAstrumAureus, 0.1f,
                ModContent.ItemType<StellarKnife>(),
                ModContent.ItemType<AstralachneaStaff>(),
                ModContent.ItemType<TitanArm>(),
                ModContent.ItemType<HivePod>(),
                ModContent.ItemType<AstralScythe>(),
                ModContent.ItemType<StellarCannon>(),
                ModContent.ItemType<StarbusterCore>());

            //Pet
            DropHelper.DropItemChance(player, ModContent.ItemType<AstrophageItem>(), 10);

            //Bait
            DropHelper.DropItemChance(player, ModContent.ItemType<TwinklerItem>(), 5, 1, 3);
            DropHelper.DropItemChance(player, ItemID.EnchantedNightcrawler, 5, 1, 3);
            DropHelper.DropItemChance(player, ModContent.ItemType<ArcturusAstroidean>(), 5, 1, 3);
            DropHelper.DropItemChance(player, ItemID.Firefly, 3, 1, 3);

            //Potions
            DropHelper.DropItemChance(player, ItemID.ObsidianSkinPotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.SwiftnessPotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.IronskinPotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.NightOwlPotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.ShinePotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.MiningPotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.HeartreachPotion, 10, 1, 3);
            DropHelper.DropItemChance(player, ItemID.TrapsightPotion, 10, 1, 3); //Dangersense Potion
            DropHelper.DropItemCondition(player, ModContent.ItemType<AstralInjection>(), DownedBossSystem.downedAstrumAureus, 0.1f, 1, 3);
            DropHelper.DropItemCondition(player, ModContent.ItemType<GravityNormalizerPotion>(), DownedBossSystem.downedAstrumAureus, 0.1f, 1, 3);
            int healingPotID = ItemID.HealingPotion;
            int manaPotID = ItemID.ManaPotion;
            if (DownedBossSystem.downedDoG)
            {
                healingPotID = ModContent.ItemType<SupremeHealingPotion>();
                manaPotID = ModContent.ItemType<SupremeManaPotion>();
            }
            else if (DownedBossSystem.downedProvidence)
            {
                healingPotID = ItemID.SuperHealingPotion;
                manaPotID = ItemID.SuperManaPotion;
            }
            else if (NPC.downedMechBossAny)
            {
                healingPotID = ItemID.GreaterHealingPotion;
                manaPotID = ItemID.GreaterManaPotion;
            }
            DropHelper.DropItemChance(player, Main.rand.NextBool(2) ? healingPotID : manaPotID, 0.25f, 2, 5);

            //Money
            DropHelper.DropItem(player, ItemID.SilverCoin, 10, 90);
            DropHelper.DropItemChance(player, ItemID.GoldCoin, 0.5f, 1, 5);
        }
    }
}
