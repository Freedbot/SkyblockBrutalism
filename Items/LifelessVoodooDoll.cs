using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
    public class LifelessVoodooDoll : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.GuideVoodooDoll);
            Item.consumable = false;
        }

        public override void AddRecipes()
        {
            if (ModContent.GetInstance<Config>().RestrictedMode)
            {
                Recipe.Create(ItemID.GuideVoodooDoll)
                    .AddIngredient(this)
                    .AddIngredient(ItemID.PartyHat)
                    .AddTile(TileID.Loom)
                    .Register();
            }
            else
            {
                Recipe.Create(ItemID.GuideVoodooDoll)
                    .AddIngredient(this)
                    .AddIngredient(ItemID.DD2EnergyCrystal, 40)
                    .AddTile(TileID.DemonAltar)
                    .Register();

                Recipe.Create(ItemID.GuideVoodooDoll)
                    .AddIngredient(this)
                    .AddIngredient(ItemID.Book)
                    .AddTile(TileID.DemonAltar)
                    .Register();
            }
        }
    }
}