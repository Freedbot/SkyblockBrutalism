using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using System.Collections.Generic;
using System.IO;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria.Chat;
using Terraria.Localization;

namespace SkyblockBrutalism
{
	public class SkyPlayer : ModPlayer
	{
        //Random little fishing code for hive.
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (attempt.inHoney && Main.rand.NextBool(3))
            {
                itemDrop = ItemID.Hive;
                return;
            }
        }

        //The following stores player first boss kill data and applies health when valid.  Mod packets are sent here.  Data is saved to player file.  Player health cannot be changed outside of here it seems.

        //List of valid bosses killed by this player.
        private readonly HashSet<int> _defeatedBosses = new();
        //I only care about these bosses.  Tons of Mod/mini bosses would make it easy to stack health quickly.
        //Only these will be stored until more are added here.  Decently future proof.  Order doesn't really matter.
        private readonly HashSet<int> validBosses = new()
        {
            NPCID.KingSlime,
            NPCID.EyeofCthulhu,

            NPCID.BrainofCthulhu,
            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsTail,

            NPCID.QueenBee,
            NPCID.SkeletronHead,
            NPCID.Deerclops,
            NPCID.WallofFlesh,

            NPCID.QueenSlimeBoss,
            NPCID.TheDestroyer,

            NPCID.Retinazer,
            NPCID.Spazmatism,

            NPCID.SkeletronPrime,
            NPCID.Plantera,
            NPCID.Golem,
            NPCID.DukeFishron,
            NPCID.HallowBoss,
            NPCID.CultistBoss,
            NPCID.MoonLordCore,
            //minibosses... mostly
            NPCID.DD2Betsy,
            NPCID.Pumpking,
            NPCID.IceQueen,
            NPCID.MartianSaucerCore
        };

        //Called from NPCEdits from singleplayer or server
        //Called from SkyblockBrutalism.HandlePacket via reciept of the below DefeatedBoss packet in client side
        //Crystal consumption happens on all sides.  So it doesn't need syncing.
        public void HandleBossDefeated(int type)
        {
            string heart = "";
            if (_defeatedBosses.Contains(type) || !validBosses.Contains(type))
            {
                //Already defeated or not valid
                return;
            }
            //Mark as defeated.  Certain bosses count for one check, so just write them all off as killed.
            //Theoretically any npc.boss is already the last surviving member of multipart bosses.
            if (type is NPCID.BrainofCthulhu or NPCID.EaterofWorldsHead or NPCID.EaterofWorldsBody or NPCID.EaterofWorldsTail)
            {
                _defeatedBosses.Add(NPCID.BrainofCthulhu);
                _defeatedBosses.Add(NPCID.EaterofWorldsHead);
                _defeatedBosses.Add(NPCID.EaterofWorldsBody);
                _defeatedBosses.Add(NPCID.EaterofWorldsTail);
            }
            else if (type is NPCID.Retinazer or NPCID.Spazmatism)
            {
                _defeatedBosses.Add(NPCID.Retinazer);
                _defeatedBosses.Add(NPCID.Spazmatism);
            }
            else _defeatedBosses.Add(type);
            //Force feed life crystals/fruit to players as appropriate
            if (Player.ConsumedLifeCrystals < Player.LifeCrystalMax)
            {
                heart = " [i:29]";
                Player.ConsumedLifeCrystals++;
            }
            else if (NPC.downedMechBossAny && Player.ConsumedLifeFruit < Player.LifeFruitMax)
            {
                heart = " [i:1291]";
                Player.ConsumedLifeFruit++;
            }
            //Play heart sound for client when given
            if ((Main.netMode is NetmodeID.SinglePlayer or NetmodeID.MultiplayerClient) && heart != "")
            {
                SoundEngine.PlaySound(SoundID.Item4, Player.Center);
            }
            else //Forward boss ID to everyone from server (ignored by server in SkyblockBrutalism.HandlePacket)
            {
                var packet = Mod.GetPacket();
                packet.Write((byte)SkyblockBrutalism.ModMessageID.DefeatedBoss);
                packet.Write(Player.whoAmI);
                packet.Write(type);
                packet.Send();
            }
            if (Main.netMode is NetmodeID.SinglePlayer or NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"{Player.name} [c/FFFFFF:{Language.GetTextValue("RandomWorldName_Adjective.Defeated")} {Lang.GetNPCName(type)}!]{heart}"), Main.teamColor[Player.team]);
            }
        }
        //Called in SkyblockBrutalism.HandlePacket upon packet reciept
        //Flushes and repopulates, mostly on server?
        public void ReceivePlayerSync(BinaryReader reader)
        {
            _defeatedBosses.Clear();
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                _defeatedBosses.Add(reader.ReadInt32());
            }
        }
        //Used on connect and normal online handshake activities, mostly to the server?
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            var packet = Mod.GetPacket();
            packet.Write((byte)SkyblockBrutalism.ModMessageID.SyncPlayerDefeatedBosses);
            packet.Write(Player.whoAmI);
            packet.Write(_defeatedBosses.Count);
            foreach (var type in _defeatedBosses)
            {
                packet.Write(type);
            }
            packet.Send(toWho, fromWho);
        }
        //Propegate player boss info
        public override ModPlayer Clone(Player newEntity)
        {
            var clone = (SkyPlayer)base.Clone(newEntity);
            clone._defeatedBosses.Clear();
            foreach (var type in _defeatedBosses)
            {
                clone._defeatedBosses.Add(type);
            }

            return clone;
        }
        //tMod Player save boss kills on file
        public override void SaveData(TagCompound tag)
        {
            tag.Add("defeatedBosses", _defeatedBosses.ToList());
        }
        public override void LoadData(TagCompound tag)
        {
            _defeatedBosses.Clear();
            foreach (var type in tag.GetList<int>("defeatedBosses"))
            {
                _defeatedBosses.Add(type);
            }
        }
    }
}