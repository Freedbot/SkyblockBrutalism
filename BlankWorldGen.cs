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
                tasks.Clear();
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
            Main.worldSurface = (double)(Main.maxTilesY / 4);
            Main.rockLayer = Main.maxTilesY / 2.5;

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
            Thread.Sleep(1000);
            progress.Set(1.0);
        }
    }
}
