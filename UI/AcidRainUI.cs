using CalamityMod.Events;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityMod.UI
{
    public class AcidRainUI : InvasionProgressUI
    {
        public override bool IsActive => (CalamityWorld.rainingAcid || CalamityWorld.acidRainExtraDrawTime > 0) && Main.LocalPlayer.Calamity().ZoneSulphur;
        public override float CompletionRatio => 1f - CalamityWorld.AcidRainCompletionRatio;
        public override string InvasionName => "Acid Rain";
        public override Color InvasionBarColor => AcidRainEvent.TextColor;
        public override Texture2D IconTexture => ModContent.GetTexture("CalamityMod/ExtraTextures/UI/AcidRainIcon");
    }
}
