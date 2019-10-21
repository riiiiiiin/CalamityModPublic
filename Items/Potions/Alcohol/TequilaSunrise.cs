using CalamityMod.Buffs.Alcohol;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Items.Potions.Alcohol
{
    public class TequilaSunrise : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tequila Sunrise");
            Tooltip.SetDefault(@"Boosts damage, damage reduction, and knockback by 7%, crit chance by 3%, and defense by 15 during daytime
Reduces life regen by 1
The greatest daytime drink I've ever had");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 18;
            item.useTurn = true;
            item.maxStack = 30;
            item.rare = 4;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            item.buffType = ModContent.BuffType<TequilaSunriseBuff>();
            item.buffTime = 18000; //5 minutes
            item.value = Item.buyPrice(0, 20, 0, 0);
        }
    }
}
