using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SkyblockBrutalism;
using System.IO;

namespace SkyblockBrutalism.Items
{
	public class RuinedGnome : ModItem
	{
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12; // This helps sort inventory know this is a boss summoning item.
            NPCID.Sets.MPAllowedEnemies[NPCID.RockGolem] = true;
        }
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SuspiciousLookingEye);
            Item.rare = ItemRarityID.Gray;
		}
        public override bool? UseItem(Player player)
        {

            if (player.whoAmI == Main.myPlayer)
            {
                // If the player using the item is the client
                // (explicitly excluded serverside here)
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                int type = NPCID.RockGolem;

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // If the player is not in multiplayer, spawn directly
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    // If the player is in multiplayer, request a spawn
                    // This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in this class above
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }
            return true;
        }
    }

    //Several days combined of work just because I was trolled by a stupid gnome bug
    public class HammerGnome : GlobalItem
    {
        //Only hammerable when the player is holding a hammer.  Otherwise it wouldn't be possible to mine with a pick normally.
        //This works flawlessly with smart cursor as a "secondary" tool, as the pick is still auto-selected for mining.
        //The trick is that this code would normally be on all sides for all players and all gnomes.
        //Because of that, and KillTile's issue, I use a custom packet.
        public override void HoldItem(Item item, Player player)
        {
            if (Main.netMode is not NetmodeID.Server)
            {
                Main.tileHammer[TileID.GardenGnome] = item.type is ItemID.TheBreaker or ItemID.FleshGrinder;
            }
        }
    }
    //My world is on fire.  How 'bout yours?
    public class SmashGnome : GlobalTile
    {
        public static int gnomeX = 0;
        public static int gnomeY = 0;

        //KillTile would be great for handling all the gnome issues and even avoid sending packets, except noItem does not work for Multitiles.
        //I get that this is a vanilla oversight simply because they don't need it in the base game, and most mod situations won't need this...
        //but I still think the tModloader team should address it.
        //So.  I can only use it to send a packet on break, and hope it arrives safely before the the vanilla break packet sent after.
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type is TileID.GardenGnome && Main.tileHammer[TileID.GardenGnome] && Main.netMode is NetmodeID.MultiplayerClient && !fail)
            {
                var packet = Mod.GetPacket();
                packet.Write((byte)SkyblockBrutalism.ModMessageID.RuinGnome);
                packet.Write(Main.LocalPlayer.whoAmI);
                packet.Write(i);
                packet.Write(j);
                packet.Send();
            }
        }
        //See SkyblockBrutalism for packet handling
        public static void ReceiveGnome(BinaryReader reader)
        {
            gnomeX = reader.ReadInt32();
            gnomeY = reader.ReadInt32();
        }
        //Singleplayer bypasses all these packet methods entirely and just breaks when hammerable.
        //I use the coordinates rather than a bool just to be extra paranoid about disconnects.
        //Otherwise it would ruin the next gnome mined with a pick.
        public override bool CanDrop(int i, int j, int type)
        {
            if (type is TileID.GardenGnome && (Main.tileHammer[TileID.GardenGnome] || (i == gnomeX && j == gnomeY)))
            {
                gnomeX = 0;
                gnomeY = 0;
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 8, 8, ModContent.ItemType<RuinedGnome>());
                return false;
            }
            return true;
        }
    }
}

