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
            if (ModContent.GetInstance<Config>().RestrictedMode)
            {
                CreateRecipe(30)
                    .AddIngredient(ItemID.Dirt3Echo, 15)
                    .AddIngredient(ItemID.AshBlock, 15)
                    .AddTile(TileID.WorkBenches)
                    .AddCondition(Condition.DownedSkeletron)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient(ItemID.Dirt3Echo)
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }
        }
    }
}