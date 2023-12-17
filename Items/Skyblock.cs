using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
	//This exists as an item only for the sake of tooltips and "fixing" maps using Cheat Sheet.
	//It cannot be aquired otherwise, and will only be infinite when placed at coords where worldgen would put it.
	public class Skyblock : ModItem
	{
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Cloud);
			Item.rare = ItemRarityID.Cyan;
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Skyblock>());
		}
	}
}