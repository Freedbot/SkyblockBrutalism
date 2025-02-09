using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
        //This is Magic Storage's Crimson Altar.  As such, it shouldn't exist when Magic Storage does.
        //See Recipe edits for the same crafting recipe tweak for their item.
    public class CrimsonAltar : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return (!ModLoader.TryGetMod("MagicStorage", out var MagicStorage));
        }
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 48;
            Item.height = 34;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<Tiles.EvilAltarTile>();
            Item.placeStyle = 1;
            Item.maxStack = 99;
            Item.value = 1875;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.useTurn = true;
        }

        public override void AddRecipes()
        {
            if (ModContent.GetInstance<Config>().RestrictedMode)
            {
                CreateRecipe()
                    .AddIngredient(ItemID.LunarOre, 10)
                    .AddIngredient(ItemID.DemonTorch, 5)
                    .AddIngredient(ItemID.Vertebrae, 10)
                    .AddIngredient(ItemID.Shadewood, 20)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient(ItemID.DemonTorch, 5)
                    .AddIngredient(ItemID.Vertebrae, 10)
                    .AddIngredient(ItemID.Shadewood, 20)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }
        }
    }
}