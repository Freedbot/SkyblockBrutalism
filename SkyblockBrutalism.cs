using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SkyblockBrutalism.Items;

namespace SkyblockBrutalism
{
	public class SkyblockBrutalism : Mod
	{
        public override void Load()
        {
        }
        public override void Unload()
        {
        }
        //Networking.  First is the packet list.
        internal enum ModMessageID : byte
        {
            SyncPlayerDefeatedBosses,
            DefeatedBoss,
            RuinGnome
        }
        //This mod's "inbox" for packets.  Routes them to SkyPlayer which also does the sending upon request from NPCEdits.
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModMessageID msgType = (ModMessageID)reader.ReadByte();
            SkyPlayer player = Main.player[reader.ReadInt32()].GetModPlayer<SkyPlayer>();
            switch (msgType)
            {
                case ModMessageID.SyncPlayerDefeatedBosses:
                    player.ReceivePlayerSync(reader);
                    break;
                case ModMessageID.DefeatedBoss:
                    if (Main.netMode is NetmodeID.MultiplayerClient)
                    {
                        player.HandleBossDefeated(reader.ReadInt32());
                    }
                    break;
                case ModMessageID.RuinGnome:
                    SmashGnome.ReceiveGnome(reader);
                    break;
                default:
                    Logger.WarnFormat("SkyblockBrutalism: Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}