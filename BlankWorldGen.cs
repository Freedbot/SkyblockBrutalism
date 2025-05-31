using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent;

namespace SkyblockBrutalism
{
    public class BlankWorldGen : ModSystem
    {
        public static LocalizedText MakeSkyblock { get; private set; }
        public override void SetStaticDefaults()
        {
            MakeSkyblock = Language.GetOrRegister(Mod.GetLocalizationKey($"BlankWorldGen.{nameof(MakeSkyblock)}"));
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            if (ModContent.GetInstance<Config>().SkyblockWorldGen)
            {
                GenPass resetTask = tasks.Find((GenPass genpass) => genpass.Name.Equals("Reset"));
                tasks.Clear();
                tasks.Add(resetTask);
                int resetIndex = tasks.FindIndex((GenPass genpass) => genpass.Name.Equals("Reset"));
                tasks.Insert(resetIndex + 1, new PassLegacy("Adding Skyblocks", new WorldGenLegacyMethod(this.AddSkyblocksPass), 1.0));
            }
        }

        private void AddSkyblocksPass(GenerationProgress progress, GameConfiguration config)
        {
            progress.Message = MakeSkyblock.Value;
            progress.Set(0.0);

            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 4 - 40;
            Main.dungeonX = Main.maxTilesX / 2;
            Main.dungeonY = Main.maxTilesY / 4 - 80;
            Main.worldSurface = Main.maxTilesY / 4;

            if (Main.remixWorld)
            {
                Main.rockLayer = Main.maxTilesY * 0.6;
            }
            else
            {
                Main.rockLayer = Main.maxTilesY / 2.5;
            }

            int isleSpread = Main.maxTilesX / 6;
            for (int i = Main.spawnTileX; i < Main.maxTilesX-10; i += isleSpread)
            {
                WorldGen.PlaceTile(i, Main.spawnTileY, ModContent.TileType<Tiles.Skyblock>());
            }
            for (int i = Main.spawnTileX - isleSpread; i > 10; i -= isleSpread)
            {
                WorldGen.PlaceTile(i, Main.spawnTileY, ModContent.TileType<Tiles.Skyblock>());
            }
            progress.Set(0.75);

            WorldGen.PlaceTile(Main.dungeonX, Main.dungeonY, TileID.CrackedBlueDungeonBrick);
            WorldGen.PlaceWall(Main.dungeonX, Main.dungeonY, WallID.BlueDungeonUnsafe);
            Thread.Sleep(500);
            //Below actions finish off the world file and are taken from WorldGen.Final Cleanup.  I'm not certain which steps are vital, or what they do, however...
            //I can confirm that without "WorldGen.gen = false", loading the world in multiplayer will delete all items dropped by the player until a file is loaded in singleplayer.
            WorldGen.gen = false;
            WorldGen.noTileActions = false;
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
            Main.NotifyOfEvent(GameNotificationType.WorldGen);

            progress.Set(1.0);
            Thread.Sleep(500);
        }
    }
}
