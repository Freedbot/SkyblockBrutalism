using Microsoft.Xna.Framework;
using SkyblockBrutalism.Items;
using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.WorldBuilding;

namespace SkyblockBrutalism.Tiles
{
    public class Skyblock : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileShine[Type] = 666; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;
            TileID.Sets.Clouds[Type] = true;
            TileID.Sets.IsValidSpawnPoint[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            TileObjectData.newTile.FullCopyFrom(TileID.Cloud);
            VanillaFallbackOnModDeletion = TileID.Cloud;
            

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(185, 235, 255), name);

            DustType = 16;
            HitSound = null;
            MineResist = 4f;
            //MinPick = ;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
                r = 2.0f;
                g = 1.8f;
                b = 0.8f;
        }

        //Set Spawn code
        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.FindSpawn();

            if (player.SpawnX == i && player.SpawnY == j)
            {
                player.RemoveSpawn();
                Main.NewText(Language.GetTextValue("Game.SpawnPointRemoved"), byte.MaxValue, 240, 20);
            }
            else
            {
                player.ChangeSpawn(i, j);
                Main.NewText(Language.GetTextValue("Game.SpawnPointSet"), byte.MaxValue, 240, 20);
            }
            return true;
        }
        //Mirsario magic creeping right by hooks to bypass all spawn check restrictions like solid blocks or valid housing
        public override void Load()
        {
            On_Player.CheckSpawn += (orig, x, y) =>
            {
                if (Framing.GetTileSafely(x, y).TileType == this.Type)
                {
                    return true;
                }
                return orig(x, y);
            };
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemID.Bed;
        }
        //locations where skyblocks are spawned
        private bool IsWorldGenSkyblock(int i, int j)
        {
            if (j == Main.spawnTileY)
            {
                int isleSpread = Main.maxTilesX / 6;
                for (int lc = Main.spawnTileX; lc < Main.maxTilesX-10; lc += isleSpread)
                {
                    if (i == lc)
                    {
                        return true;
                    }
                }
                for (int lc = Main.spawnTileX - isleSpread; lc > 10; lc -= isleSpread)
                {
                    if (i == lc)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //Disable Block Swap
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)
        {
            return !IsWorldGenSkyblock(i, j);
        }
        //Make unbreakable, and drop items from a loot pool
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (IsWorldGenSkyblock(i, j) && !fail)
            {
                effectOnly = true;
                int lootOdds = Main.rand.Next(99);
                int lootDrop = ItemID.Cloud;
                if (0 <= lootOdds && lootOdds <= 19)
                {
                    lootDrop = 0;
                }
                else if (20 <= lootOdds && lootOdds <= 39)
                {
                    lootDrop = ItemID.CopperCoin;
                }
                else if (40 <= lootOdds && lootOdds <= 44)
                {
                    lootDrop = ItemID.RainCloud;
                }
                else if (45 <= lootOdds && lootOdds <= 47)
                {
                    lootDrop = ItemID.SnowCloudBlock;
                }
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 8, 8, lootDrop);
            }
        }
    }
}
