﻿using CalamityMod.Items.Materials;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.NPCs.NormalNPCs
{
    public class IceMass : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aurora Spirit");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = 86;
            NPC.damage = 40;
            NPC.width = 40;
            NPC.height = 24;
            NPC.defense = 8;
            NPC.alpha = 100;
            NPC.lifeMax = 50;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath15;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = false;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void FindFrame(int frameHeight)
        {
            int num1 = 1;
            if (!Main.dedServ)
            {
                if (!Main.NPCLoaded[NPC.type] || TextureAssets.Npc[NPC.type].Value == null)
                    return;
                num1 = TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type];
            }
            if (NPC.velocity.X < 0f)
                NPC.direction = -1;
            else
                NPC.direction = 1;
            if (NPC.direction == 1)
                NPC.spriteDirection = 1;
            if (NPC.direction == -1)
                NPC.spriteDirection = -1;
            NPC.rotation = (float)Math.Atan2((double)NPC.velocity.Y * (double)NPC.direction, (double)NPC.velocity.X * (double)NPC.direction);
            NPC.frameCounter++;
            if (NPC.frameCounter > 4)
            {
              NPC.frame.Y += num1;
              NPC.frameCounter = 0;
            }
            if (NPC.frame.Y / num1 >= Main.npcFrameCount[NPC.type])
              NPC.frame.Y = 0;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneSnow &&
                spawnInfo.Player.ZoneOverworldHeight &&
                !spawnInfo.Player.PillarZone() &&
                !spawnInfo.Player.ZoneDungeon &&
                !spawnInfo.Player.InSunkenSea() &&
                Main.hardMode && !spawnInfo.playerInTown && !spawnInfo.Player.ZoneOldOneArmy && !Main.snowMoon && !Main.pumpkinMoon ? 0.01f : 0f;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Frostburn, 90, true);
            player.AddBuff(BuffID.Chilled, 60, true);
        }

        public override void AI()
        {
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.02f, 0.7f, 0.7f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 67, hitDirection, -1f, 0, default, 1f);
                }
                Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/CryoSpirit"), 1f);
            }
        }

        public override void NPCLoot()
        {
            DropHelper.DropItemChance(NPC, ModContent.ItemType<EssenceofEleum>(), 0.25f);
        }
    }
}
