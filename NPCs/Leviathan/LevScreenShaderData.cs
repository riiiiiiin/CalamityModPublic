using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.Leviathan
{
    public class LevScreenShaderData : ScreenShaderData
    {
        private int LevIndex;

        public LevScreenShaderData(string passName)
            : base(passName)
        {
        }

        private void UpdateLIndex()
        {
            int LevType = ModContent.NPCType<Leviathan>();
            if (LevIndex >= 0 && Main.npc[LevIndex].active && Main.npc[LevIndex].type == LevType)
            {
                return;
            }
            LevIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == LevType)
                {
                    LevIndex = i;
                    break;
                }
            }
        }

        public override void Apply()
        {
            UpdateLIndex();
            if (LevIndex != -1)
            {
                UseTargetPosition(Main.npc[LevIndex].Center);
            }
            base.Apply();
        }
    }
}
