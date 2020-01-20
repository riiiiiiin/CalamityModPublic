using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.Summon;
using CalamityMod.World;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.Buffs
{
    public class CalamityGlobalBuff : GlobalBuff
    {
        public override void Update(int type, Player player, ref int buffIndex)
        {
            if (type == BuffID.Shine)
            {
                player.Calamity().shine = true;
            }
            else if (type == BuffID.IceBarrier)
            {
                player.endurance -= 0.1f;
            }
            else if (type == BuffID.ObsidianSkin)
            {
                player.lavaMax += 420;
            }
            else if (type == BuffID.Rage)
            {
                player.Calamity().throwingCrit += 10;
            }
            else if (type == BuffID.WellFed)
            {
                player.Calamity().throwingCrit += 2;
            }
            else if (type >= BuffID.NebulaUpDmg1 && type <= BuffID.NebulaUpDmg3)
            {
                float nebulaDamage = 0.075f * (float)player.nebulaLevelDamage; //7.5% to 22.5%
                player.allDamage -= nebulaDamage;
            }
            else if (type >= BuffID.NebulaUpLife1 && type <= BuffID.NebulaUpLife3)
            {
                player.lifeRegen -= 5 * player.nebulaLevelLife; //10 to 30 changed to 5 to 15
            }
            else if (type == BuffID.Warmth)
            {
                player.buffImmune[ModContent.BuffType<GlacialState>()] = true;
                player.buffImmune[BuffID.Frozen] = true;
                player.buffImmune[BuffID.Chilled] = true;
            }
        }

        public override void Update(int type, NPC npc, ref int buffIndex)
        {
			if (type == BuffID.Webbed)
			{
				if (npc.Calamity().webbed < npc.buffTime[buffIndex])
					npc.Calamity().webbed = npc.buffTime[buffIndex];
				npc.DelBuff(buffIndex);
				buffIndex--;
			}
            else if (type == BuffID.Slow)
            {
                if (npc.Calamity().slowed < npc.buffTime[buffIndex])
                    npc.Calamity().slowed = npc.buffTime[buffIndex];
                npc.DelBuff(buffIndex);
                buffIndex--;
            }
            if (type == BuffID.Electrified)
            {
                if (npc.Calamity().electrified < npc.buffTime[buffIndex])
                    npc.Calamity().electrified = npc.buffTime[buffIndex];
                npc.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {
            if (type == BuffID.NebulaUpDmg1)
                tip = "7.5% increased damage";
            else if (type == BuffID.NebulaUpDmg2)
                tip = "15% increased damage";
            else if (type == BuffID.NebulaUpDmg3)
                tip = "22.5% increased damage";
            else if (type == BuffID.WeaponImbueVenom || type == BuffID.WeaponImbueCursedFlames || type == BuffID.WeaponImbueFire || type == BuffID.WeaponImbueGold ||
                type == BuffID.WeaponImbueIchor || type == BuffID.WeaponImbueNanites || type == BuffID.WeaponImbueConfetti || type == BuffID.WeaponImbuePoison)
                tip = "Rogue and " + tip;
            else if (type == BuffID.IceBarrier)
                tip = "Damage taken is reduced by 15%";
            else if (type == BuffID.ChaosState && CalamityWorld.revenge)
                tip += ". All damage taken increased by 25%";
            else if (type == BuffID.Ichor)
            {
                tip = "Defense reduced by 20";
                if (CalamityWorld.revenge)
                    tip += ". All damage taken increased by 25%";
            }
            else if (type == BuffID.CursedInferno && CalamityWorld.revenge)
                tip += ". All damage taken increased by 20%";
            else if (type == BuffID.Warmth)
                tip += ". Immunity to the Chilled, Frozen, and Glacial State debuffs";
            else if (type == ModContent.BuffType<ProfanedBabs>())
            {
                Player player = Main.player[Main.myPlayer];
                bool offense = player.Calamity().gOffense;
                bool defense = player.Calamity().gDefense;
                if (offense && defense)
                {
                    tip = "The Profaned Babs will fight for and defend you!";
                }
                else if (offense || defense)
                {
                    tip = "The " + (offense ? "Offensive" : "Defensive") + " Duo will " + (offense ? "fight for and heal for you!" : "protect and heal you!");
                }
            }
        }
    }
}
