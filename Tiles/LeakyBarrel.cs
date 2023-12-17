using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SkyblockBrutalism.Tiles
{
    public class LeakyBarrel : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);

            AdjTiles = new int[] { Type, TileID.Sinks };

			AddMapEntry(Color.Brown, CreateMapEntryName());
		}
	}
}
