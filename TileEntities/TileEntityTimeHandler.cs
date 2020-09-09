﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityMod.TileEntities
{
	public static class TileEntityTimeHandler
	{
		public static void Update()
		{
			MultiplayerClientUpdateVisuals();
		}
		private static void MultiplayerClientUpdateVisuals()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
				return;

			byte factoryType = ModContent.GetInstance<TEPowerCellFactory>().type;
			byte chargerType = ModContent.GetInstance<TEChargingStation>().type;

			var enumerator = TileEntity.ByID.Values.GetEnumerator();
			do
			{
				TileEntity te = enumerator.Current;
				if (te != null && te.type == factoryType)
				{
					// Specifically on multiplayer clients, manually update the time variables of Power Cell Factories every frame.
					// This makes sure they animate. It will NOT produce cells; that code can only run server side.
					// Time is manually synced from the server every time a cell is created, so even under heavy lag they cannot stay desynced indefinitely.
					TEPowerCellFactory factory = (TEPowerCellFactory)te;
					++factory.Time;
				}
				if (te != null && te.type == chargerType)
				{
					// Specifically on multiplayer clients, produce charging dust when the "should dust" flag is set by the most recent sync packet.
					// This makes sure they produce charging dust for all clients. It will NOT actually charge items; that code can only run server side.
					TEChargingStation charger = (TEChargingStation)te;

					if (charger.ClientChargingDust && charger.CanDoWork)
					{
						charger.ClientChargingDust = false;
						charger.SpawnChargingDust();
					}
				}
			} while (enumerator.MoveNext());
		}
	}
}
