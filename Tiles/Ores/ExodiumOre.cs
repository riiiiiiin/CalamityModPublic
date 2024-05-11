﻿
using CalamityMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityMod.Tiles.Ores
{
    public class ExodiumOre : ModTile
    {
        public TileFraming.MergeFrameData tileAdjacency;
        public TileFraming.MergeFrameData secondTileAdjacency;
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            CalamityUtils.MergeWithGeneral(Type);
            CalamityUtils.MergeWithSet(Type, TileID.LunarOre);

            TileID.Sets.Ore[Type] = true;
            TileID.Sets.OreMergesWithMud[Type] = true;

            AddMapEntry(new Color(51, 48, 68), CreateMapEntryName());
            MineResist = 3f;
            MinPick = 225;
            HitSound = SoundID.Tink;
            Main.tileOreFinderPriority[Type] = 760;
            Main.tileSpelunker[Type] = true;
            base.SetStaticDefaults();

            TileID.Sets.ChecksForMerge[Type] = true;


            TileFraming.SetUpUniversalMerge(Type, TileID.Dirt, "CalamityMod/Tiles/Merges/DirtMerge", out tileAdjacency);
            TileFraming.SetUpUniversalMerge(Type, TileID.LunarOre, "CalamityMod/Tiles/Merges/LuminiteMerge", out secondTileAdjacency);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            TileFraming.DrawUniversalMergeFrames(i, j, secondTileAdjacency, tileAdjacency);
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            TileFraming.GetAdjacencyData(i, j, TileID.Dirt, tileAdjacency);
            TileFraming.GetAdjacencyData(i, j, TileID.LunarOre, secondTileAdjacency);
            return true;
        }

        public override bool CanExplode(int i, int j) => false;

        public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 2 : 4;
    }
}
