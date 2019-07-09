﻿using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.NPCs;

namespace CalamityMod.Buffs
{
	public class IceShieldBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ice Shield");
			Description.SetDefault("Absorbs 15% damage from the next hit you take, then shatters");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<CalamityPlayer>(mod).sirenIce = true;
		}
	}
}
