using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using Terraria.Localization;
using SkyblockBrutalism.Items;

namespace SkyblockBrutalism
{
	public class RecipeEdits : ModSystem
	{
        //See Items/ItemEdits for custom shimmer crafting.
        public override void AddRecipeGroups()
        {
            RecipeGroup group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.CorruptSeeds)}", ItemID.CorruptSeeds, ItemID.CrimsonSeeds);
            RecipeGroup.RegisterGroup(nameof(ItemID.CorruptSeeds), group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.ShadowScale)}", ItemID.ShadowScale, ItemID.TissueSample);
            RecipeGroup.RegisterGroup(nameof(ItemID.ShadowScale), group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldCrown)}", ItemID.GoldCrown, ItemID.PlatinumCrown);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldCrown), group);

            group = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.AncientBlueDungeonBrick)}", ItemID.AncientBlueDungeonBrick, ItemID.AncientGreenDungeonBrick, ItemID.AncientPinkDungeonBrick); ;
            RecipeGroup.RegisterGroup(nameof(ItemID.AncientBlueDungeonBrick), group);
        }
        public override void AddRecipes()
        {
            //Gravedigger Shovel Tierdown.  See ItemEdits and NPCEdits.
            Recipe recipe = Recipe.Create(ItemID.GravediggerShovel)
                .AddRecipeGroup(RecipeGroupID.Wood, 15)
                .AddTile(TileID.Anvils)
                .AddCondition(Condition.InGraveyard)
                .Register();

            //Early Furnature config option
            if (ModContent.GetInstance<Config>().EarlyFurniture)
            {
                recipe = Recipe.Create(ItemID.SkywareWorkbench)
                    .AddIngredient(ItemID.Cloud, 10)
                    .AddIngredient(ItemID.FallenStar, 6)
                    .Register();

                recipe = Recipe.Create(ItemID.SkywareTable)
                    .AddIngredient(ItemID.Cloud, 8)
                    .AddIngredient(ItemID.FallenStar, 2)
                    .AddTile(TileID.WorkBenches)
                    .Register();

                recipe = Recipe.Create(ItemID.SkywareChair)
                    .AddIngredient(ItemID.Cloud, 4)
                    .AddIngredient(ItemID.FallenStar, 2)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }

            recipe = Recipe.Create(ItemID.Fertilizer)
                .AddIngredient(ItemID.PoopBlock, 18)
                .AddTile(TileID.Bottles)
                .Register();

            recipe = Recipe.Create(ItemID.Fertilizer)
                .AddIngredient(ItemID.PoopBlock, 9)
                .AddIngredient(ItemID.AshBlock, 9)
                .AddTile(TileID.Bottles)
                .Register();

            recipe = Recipe.Create(ItemID.SnowGlobe)
                .AddIngredient(ItemID.SnowCloudBlock, 10)
                .Register();

            recipe = Recipe.Create(ItemID.SuspiciousLookingEye)
                .AddIngredient(ItemID.Lens, 20)
                .AddTile(TileID.Solidifier)
                .Register();

            recipe = Recipe.Create(ItemID.ClothierVoodooDoll)
                .AddIngredient(ModContent.ItemType<LifelessVoodooDoll>())
                .AddIngredient(ItemID.RedHat)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.ClayBlock)
                .AddIngredient(ItemID.MudBlock)
                .AddIngredient(ItemID.SandBlock)
                .AddTile(TileID.WorkBenches)
                .Register();

            recipe = Recipe.Create(ItemID.DirtiestBlock)
                .AddIngredient(ItemID.DirtBlock, 350)
                .AddTile(TileID.WorkBenches)
                .Register();

            recipe = Recipe.Create(ItemID.Granite, 5)
                .AddIngredient(ItemID.StoneBlock, 5)
                .AddIngredient(ItemID.BlueDye)
                .AddTile(TileID.WorkBenches)
                .Register();

            recipe = Recipe.Create(ItemID.Marble, 5)
                .AddIngredient(ItemID.StoneBlock, 5)
                .AddIngredient(ItemID.BrightSilverDye)
                .AddTile(TileID.WorkBenches)
                .Register();

            recipe = Recipe.Create(ItemID.IceBlock)
                .AddIngredient(ItemID.SnowBlock, 2)
                .AddTile(TileID.IceMachine)
                .Register();

            recipe = Recipe.Create(ItemID.AshBlock)
                .AddRecipeGroup(RecipeGroupID.Wood, 4)
                .AddTile(TileID.Furnaces)
                .Register();

            recipe = Recipe.Create(ItemID.Cobweb, 3)
                .AddIngredient(ItemID.TatteredCloth)
                .AddTile(TileID.WorkBenches)
                .Register();

            recipe = Recipe.Create(ItemID.JungleGrassSeeds)
                .AddRecipeGroup(nameof(ItemID.CorruptSeeds))
                .AddIngredient(ItemID.PurificationPowder)
                .Register();
            //Statues
            recipe = Recipe.Create(ItemID.KingStatue)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddRecipeGroup(nameof(ItemID.GoldCrown))
                .AddTile(TileID.HeavyWorkBench)
                .Register();

            recipe = Recipe.Create(ItemID.QueenStatue)
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddRecipeGroup(nameof(ItemID.GoldCrown))
                .AddTile(TileID.HeavyWorkBench)
                .Register();
            //Trap Conversions
            recipe = Recipe.Create(ItemID.GeyserTrap)
                .AddIngredient(ItemID.DartTrap)
                .AddIngredient(ItemID.VolcanoLarge)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            recipe = Recipe.Create(ItemID.SuperDartTrap)
                .AddIngredient(ItemID.DartTrap)
                .AddIngredient(ItemID.LihzahrdBrick)
                .AddTile(TileID.ImbuingStation)
                .Register();

            recipe = Recipe.Create(ItemID.FlameTrap)
                .AddIngredient(ItemID.GeyserTrap)
                .AddIngredient(ItemID.LihzahrdBrick)
                .AddTile(TileID.ImbuingStation)
                .Register();

            recipe = Recipe.Create(ItemID.SpikyBallTrap)
                .AddIngredient(ItemID.DartTrap)
                .AddIngredient(ItemID.LihzahrdBrick)
                .AddIngredient(ItemID.SpikyBall, 20)
                .AddTile(TileID.ImbuingStation)
                .Register();

            recipe = Recipe.Create(ItemID.SpearTrap)
                .AddIngredient(ItemID.DartTrap)
                .AddIngredient(ItemID.LihzahrdBrick)
                .AddIngredient(ItemID.Javelin)
                .AddTile(TileID.ImbuingStation)
                .Register();
            //Dungeon Bricks
            recipe = Recipe.Create(ItemID.BlueBrick, 4)
                .AddIngredient(ItemID.IceBrick, 4)
                .AddRecipeGroup(nameof(ItemID.ShadowScale), 2)
                .AddIngredient(ItemID.BeeWax)
                .AddIngredient(ItemID.GolfCupFlagBlue)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.GreenBrick, 4)
                .AddIngredient(ItemID.IceBrick, 4)
                .AddRecipeGroup(nameof(ItemID.ShadowScale), 2)
                .AddIngredient(ItemID.BeeWax)
                .AddIngredient(ItemID.GolfCupFlagGreen)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.PinkBrick, 4)
                .AddIngredient(ItemID.IceBrick, 4)
                .AddRecipeGroup(nameof(ItemID.ShadowScale), 2)
                .AddIngredient(ItemID.BeeWax)
                .AddIngredient(ItemID.GolfCupFlagPurple)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.LihzahrdBrick)
                .AddRecipeGroup(nameof(ItemID.AncientBlueDungeonBrick))
                .AddIngredient(ItemID.TempleKey)
                .AddIngredient(ItemID.JungleKey)
                .AddIngredient(ItemID.CorruptionKey)
                .AddIngredient(ItemID.CrimsonKey)
                .AddIngredient(ItemID.HallowedKey)
                .AddIngredient(ItemID.FrozenKey)
                .AddIngredient(ItemID.DungeonDesertKey)
                .AddConsumeItemCallback((Recipe recipe, int type, ref int amount) => {
                    if (type == ItemID.TempleKey || type == ItemID.JungleKey || type == ItemID.CorruptionKey || type == ItemID.CrimsonKey || type == ItemID.HallowedKey || type == ItemID.FrozenKey || type == ItemID.DungeonDesertKey)
                    { amount = 0; } })
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.LihzahrdAltar)
                .AddIngredient(ItemID.LihzahrdBrick, 15)
                .AddIngredient(ItemID.LizardEgg)
                .AddTile(TileID.SkyMill)
                .Register();
            //Unsafe Walls-----------------------------------
            recipe = Recipe.Create(ItemID.SpiderWallUnsafe)
                .AddIngredient(ItemID.SpiderEcho) //watch your wall ID's.
                .AddTile(TileID.DemonAltar)
                .Register();
            //blue dungeon wall
            recipe = Recipe.Create(ItemID.BlueBrickWallUnsafe)
                .AddIngredient(ItemID.BlueBrickWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.BlueSlabWallUnsafe)
                .AddIngredient(ItemID.BlueSlabWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.BlueTiledWallUnsafe)
                .AddIngredient(ItemID.BlueTiledWall)
                .AddTile(TileID.DemonAltar)
                .Register();
            //green dungeon wall
            recipe = Recipe.Create(ItemID.GreenBrickWallUnsafe)
                .AddIngredient(ItemID.GreenBrickWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.GreenSlabWallUnsafe)
                .AddIngredient(ItemID.GreenSlabWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.GreenTiledWallUnsafe)
                .AddIngredient(ItemID.GreenTiledWall)
                .AddTile(TileID.DemonAltar)
                .Register();
            //pink dungeon wall
            recipe = Recipe.Create(ItemID.PinkBrickWallUnsafe)
                .AddIngredient(ItemID.PinkBrickWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.PinkSlabWallUnsafe)
                .AddIngredient(ItemID.PinkSlabWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.PinkTiledWallUnsafe)
                .AddIngredient(ItemID.PinkTiledWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.HardenedSandWallUnsafe)
                .AddIngredient(ItemID.HardenedSandWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.SandstoneWallUnsafe)
                .AddIngredient(ItemID.SandstoneWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            recipe = Recipe.Create(ItemID.LihzahrdWallUnsafe)
                .AddIngredient(ItemID.LihzahrdBrickWall)
                .AddTile(TileID.DemonAltar)
                .Register();

            //adds same recipes for same Altars as added by this mod, but for MagicStorage instead.
            if (ModLoader.TryGetMod("MagicStorage", out Mod MagicStorage) && MagicStorage.TryFind<ModItem>("DemonAltar", out ModItem DemonAltar) && MagicStorage.TryFind<ModItem>("CrimsonAltar", out ModItem CrimsonAltar))
            {
                recipe = Recipe.Create(DemonAltar.Type)
                    .AddIngredient(ItemID.DemonTorch, 5)
                    .AddIngredient(ItemID.RottenChunk, 10)
                    .AddIngredient(ItemID.Ebonwood, 20)
                    .AddTile(TileID.WorkBenches)
                    .Register();

                recipe = Recipe.Create(CrimsonAltar.Type)
                    .AddIngredient(ItemID.DemonTorch, 5)
                    .AddIngredient(ItemID.Vertebrae, 10)
                    .AddIngredient(ItemID.Shadewood, 20)
                    .AddTile(TileID.WorkBenches)
                    .Register();
            }
        }
        public override void PostAddRecipes()
        {
            for (int i = 0; i < Recipe.numRecipes; i++)
            {
                Recipe recipe = Main.recipe[i];

                if (recipe.HasResult(ItemID.GravediggerShovel) && recipe.HasTile(TileID.Anvils) && recipe.HasRecipeGroup(RecipeGroupID.IronBar) && recipe.HasRecipeGroup(RecipeGroupID.Wood))
                {
                    recipe.DisableRecipe();
                }
            }
        }
    }
}