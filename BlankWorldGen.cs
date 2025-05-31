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
using Microsoft.Build.Construction;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;

namespace SkyblockBrutalism
{
    public class BlankWorldGen : ModSystem
    {
        public static LocalizedText MakeSkyblock { get; private set; }
        public override void SetStaticDefaults()
        {
            MakeSkyblock = Language.GetOrRegister(Mod.GetLocalizationKey($"BlankWorldGen.{nameof(MakeSkyblock)}"));
        }
        //"Stolen" and heavily adapted with Moomoo's permission from Moomoo's Ultimate Skyblock mod.
        //GeneratedWithSkyblock is false at load and after all world closes, only true during worldgen.
        public static bool GeneratedWithSkyblock = false;
        public override void ClearWorld()
        {
            GeneratedWithSkyblock = false;
        }
        //Save Header is readable from world select.  It reapplies data every save.  This only applies the tag after worldgen or if it already exists.
        public override void SaveWorldHeader(TagCompound tag)
        {
            if (GeneratedWithSkyblock || (Main.ActiveWorldFileData.TryGetHeaderData(this, out var data) && data.GetBool("GeneratedWithSkyblock")))
            {
                tag["GeneratedWithSkyblock"] = true;
            }
        }
        //Load overlay.
        public override void Load()
        {
            On_UIWorldListItem.DrawSelf += (orig, self, spriteBatch) =>
            {
                orig(self, spriteBatch);
                DrawWorldSelectItemOverlay(self, spriteBatch);
            };
        }
        //Apply label to skyblock worlds with custom header data.
        private void DrawWorldSelectItemOverlay(UIWorldListItem uiItem, SpriteBatch spriteBatch)
        {
            if ((!uiItem.Data.TryGetHeaderData(this, out var data) || !data.GetBool("GeneratedWithSkyblock")))
                return;

            var dims = uiItem.GetInnerDimensions();
            var pos = new Vector2(dims.X + 500, dims.Y);
            Color color = Color.Lerp(Color.Bisque, Color.BurlyWood, (MathF.Sin(Main.GlobalTimeWrappedHourly * 1.3f) + 1) / 2f);
            Terraria.Utils.DrawBorderString(spriteBatch, "Skyblock", pos, color);
        }
        //The actual worldgen code.  I "save" the reset task that starts the process, because why not, then clear all tasks, reinstert Reset, and then insert my custom worldgen right after reset.  Other mod worldgen will not be happy with this.
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
            //Progress bar, timed waits are just to give time to read that it's working.
            progress.Message = MakeSkyblock.Value;
            progress.Set(0.0);
            //Assign depths and points.  World size is handled fine without me doing anything.
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
            //Spread skyblocks
            int isleSpread = Main.maxTilesX / 6;
            for (int i = Main.spawnTileX; i < Main.maxTilesX - 10; i += isleSpread)
            {
                WorldGen.PlaceTile(i, Main.spawnTileY, ModContent.TileType<Tiles.Skyblock>());
            }
            for (int i = Main.spawnTileX - isleSpread; i > 10; i -= isleSpread)
            {
                WorldGen.PlaceTile(i, Main.spawnTileY, ModContent.TileType<Tiles.Skyblock>());
            }
            progress.Set(0.75);
            //Place Dungeon, No NPC's to start
            WorldGen.PlaceTile(Main.dungeonX, Main.dungeonY, TileID.CrackedBlueDungeonBrick);
            WorldGen.PlaceWall(Main.dungeonX, Main.dungeonY, WallID.BlueDungeonUnsafe);
            Thread.Sleep(500);
            //Below actions finish off the world file and are taken from WorldGen.Final Cleanup.  I'm not certain which steps are vital, or what they do, however...
            //I can confirm that without "WorldGen.gen = false", loading the world in multiplayer will delete all items dropped by the player until a file is loaded in singleplayer.
            WorldGen.gen = false;
            WorldGen.noTileActions = false;
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
            Main.NotifyOfEvent(GameNotificationType.WorldGen);

            GeneratedWithSkyblock = true;  //Used to label worlds as skyblock above.
            progress.Set(1.0);
            Thread.Sleep(500);
        }
    }
}
