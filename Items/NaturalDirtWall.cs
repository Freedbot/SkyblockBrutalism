using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
	public class NaturalDirtWall : ModItem
	{
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Dirt3Echo);
			Item.DefaultToPlaceableWall(WallID.DirtUnsafe3);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Dirt3Echo)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}