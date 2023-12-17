using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SkyblockBrutalism.Tiles
{
    //This is Magic Storage's Altar tile.  As such, it shouldn't exist when Magic Storage does.
    public class EvilAltarTile : ModTile
	{
        public override bool IsLoadingEnabled(Mod mod)
        {
            return !ModLoader.TryGetMod("MagicStorage", out var MagicStorage);
        }
        public override string Texture => $"Terraria/Images/Tiles_{TileID.DemonAltar}";

		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.addTile(Type);

			AdjTiles = new int[] { Type, TileID.DemonAltar };

			AddMapEntry(Color.MediumPurple, CreateMapEntryName());
		}
	}
}
