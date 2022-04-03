
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityMod.Tiles.Ores
{
    public class AerialiteOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileOreFinderPriority[Type] = 450;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            ItemDrop = ModContent.ItemType<Items.Placeables.Ores.AerialiteOre>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Aerialite Ore");
            AddMapEntry(new Color(0, 255, 255), name);
            MineResist = 2f;
            MinPick = 65;
            SoundType = SoundID.Tink;
            Main.tileSpelunker[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
