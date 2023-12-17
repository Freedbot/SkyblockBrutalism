using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
    public class LeakyBarrel : ModItem
    {
        public override void SetDefaults()
        {

            Item.CloneDefaults(ItemID.Barrel);
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LeakyBarrel>());
            Item.value = 0;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(RecipeGroupID.Wood, 9)
                .AddIngredient(ItemID.StoneBlock, 4)
                .AddIngredient(ItemID.RainCloud, 4)
                .AddTile(TileID.WorkBenches)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.AshWood, 15)
                .AddIngredient(ItemID.RainCloud, 4)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}