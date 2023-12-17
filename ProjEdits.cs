using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;

namespace SkyblockBrutalism
{
	public class ProjEdits : GlobalProjectile
	{
        //Frost Legion Stats for projectiles.  See NPCEdits.
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (projectile.type == ProjectileID.RockGolemRock || projectile.type == ProjectileID.BulletSnowman || projectile.type == ProjectileID.SnowBallHostile)
            {
                modifiers.SourceDamage /= 4;
            }
        }
        //kill shimmer balloon when wet.  Kill isn't calling our OnKill here, and small projectile size means we need to place above the liquid surface anyway, so just repeat the liquid placement here.
        public override void AI(Projectile projectile)
        {
            if (projectile.type is ProjectileID.GelBalloon && projectile.wet)
            {
                Point ij = projectile.Center.ToTileCoordinates();
                projectile.Kill();
                WorldGen.PlaceLiquid(ij.X, ij.Y - 1, (byte)LiquidID.Shimmer, 255);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendTileSquare(-1, ij.X, ij.Y);
                }
            }
        }
        //Fertilizer is a bringer of life, the opposite of a weed whacker.
        public override bool? CanCutTiles(Projectile projectile)
        {
            if (projectile.type is ProjectileID.Fertilizer)
            {
                return false;
            }
            return null;
        }
        public override void OnKill(Projectile projectile, int timeLeft)
        {
            switch (projectile.type)
            {
                //Hive Drop to get resources for Queen Bee
                case ProjectileID.BeeHive:
                    if (projectile.wet)
                    {
                        projectile.netUpdate = true;
                        Point ij = projectile.Center.ToTileCoordinates();
                        WorldGen.PlaceTile(ij.X, ij.Y, TileID.Hive, false, true);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, ij.X, ij.Y);
                        }
                    }
                    break;
                //Stupid Sparkle Balloons need a real purpose, see language file.
                case ProjectileID.GelBalloon:
                    {
                        projectile.netUpdate = true;
                        Point ij = projectile.Center.ToTileCoordinates();
                        WorldGen.PlaceLiquid(ij.X, ij.Y, (byte)LiquidID.Shimmer, 255);
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, ij.X, ij.Y);
                        }
                    }
                    break;

                //Attempting to jam most of the functionality of the UpdateWorld_xxxTile functions into fertilizer.  Doesn't include crystals, water plants, most rubble.
                //private static void UpdateWorld_OvergroundTile(int i, int j, bool checkNPCSpawns, int wallDist)
                //private static void UpdateWorld_UndergroundTile(int i, int j, bool checkNPCSpawns, int wallDist)
                case ProjectileID.Fertilizer:
                    {
                        //The for loop might be entirely unecessary?
                        Point pos = projectile.position.ToTileCoordinates();
                        for (int i = pos.X - 1; i < pos.X + (projectile.width / 16) + 2; i++)
                        {
                            for (int j = pos.Y - 1; j < pos.Y + (projectile.height / 16) + 2; j++)
                            {
                                Tile tile = Framing.GetTileSafely(i, j);
                                Tile tileAbove = Framing.GetTileSafely(i, j - 1);
                                int tileType = tile.TileType;
                                bool plantsHaveGrown = false;

                                //skip if air
                                if (!tile.HasTile)
                                {
                                    continue;
                                }
                                //Potion Plants.  Grows existing or Places youngest version
                                if (tileType is TileID.ImmatureHerbs)
                                {
                                    Main.tile[i, j].TileType = TileID.MatureHerbs;
                                    if (Main.netMode == NetmodeID.Server)
                                    {
                                        NetMessage.SendTileSquare(-1, i, j);
                                    }
                                    WorldGen.SquareTileFrame(i, j);
                                }
                                else if (!tileAbove.HasTile)
                                {
                                    if (tileType is TileID.ClayPot or TileID.PlanterBox or TileID.RockGolemHead)
                                    {
                                        WorldGen.PlaceTile(i, j - 1, TileID.Plants, mute: true);
                                        if (Main.netMode == NetmodeID.Server)
                                        {
                                            NetMessage.SendTileSquare(-1, i, j - 1);
                                        }
                                    }
                                    //Also dye plants
                                    else if (Main.rand.NextBool(20))
                                    {
                                        WorldGen.plantDye(i, j);
                                    }
                                    else if (Main.hardMode && Main.rand.NextBool(40))
                                    {
                                        WorldGen.plantDye(i, j, exoticPlant: true);
                                    }
                                    else if (WorldGen.genRand.NextBool(20))
                                    {
                                        if (tileType is TileID.Grass or TileID.HallowedGrass)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 0); //Daybloom
                                        }
                                        else if (tileType is TileID.JungleGrass)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 1); //Moonglow
                                        }
                                        else if (tileType is TileID.Dirt or TileID.Mud)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 2); //Blinkroot
                                        }
                                        else if (tileType is TileID.CorruptGrass or TileID.CorruptJungleGrass or TileID.Ebonstone or TileID.CrimsonGrass or TileID.CrimsonJungleGrass or TileID.Crimstone)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 3); //Deathweed
                                        }
                                        else if ((tileType is TileID.Sand or TileID.Pearlsand) && i >= WorldGen.beachDistance && i <= Main.maxTilesX - WorldGen.beachDistance)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 4); //Waterleaf
                                        }
                                        else if (tileType is TileID.Ash or TileID.AshGrass)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 5); //Fireblossom
                                        }
                                        else if (tileType is TileID.SnowBlock or TileID.IceBlock or TileID.CorruptIce or TileID.FleshIce or TileID.HallowedIce)
                                        {
                                            WorldGen.PlaceAlch(i, j - 1, 6); //Shiverthorn
                                        }
                                        if (tileAbove.HasTile && Main.netMode == NetmodeID.Server)
                                        {
                                            NetMessage.SendTileSquare(-1, i, j - 1);
                                        }
                                    }
                                }
                                if (tileType == TileID.Pumpkins)
                                {
                                    WorldGen.GrowPumpkin(i, j, TileID.Pumpkins);
                                } 
                                else if (!tile.CheckingLiquid)
                                {
                                    //cactus, pumpkin, and generic flowers in pots
                                    if (!tileAbove.HasTile && (tileType == TileID.Cactus || (TileID.Sets.Conversion.Sand[tileType] && i > WorldGen.beachDistance + 20 && i < Main.maxTilesX - WorldGen.beachDistance - 20 && WorldGen.genRand.NextBool(20))))
                                    {
                                        WorldGen.GrowCactus(i, j);
                                    }
                                    //spider cave wall web, why not
                                    else if (tile.WallType is WallID.SpiderUnsafe && Main.rand.NextBool(3))
                                    {
                                        int rand24 = WorldGen.genRand.Next(2, 4);
                                        bool webAllowed = false;
                                        for (int k = i - rand24; k <= i + rand24; k++)
                                        {
                                            for (int l = j - rand24; l <= j + rand24; l++)
                                            {
                                                if (WorldGen.SolidTile(k, l))
                                                {
                                                    webAllowed = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (webAllowed)
                                        {
                                            WorldGen.PlaceTile(i, j, TileID.Cobweb, mute: true);
                                            WorldGen.TileFrame(i, j, resetFrame: true);
                                            if (Main.netMode == NetmodeID.Server)
                                            {
                                                NetMessage.SendTileSquare(-1, i, j, 1);
                                            }
                                            continue;
                                        }
                                    }
                                    //Grass spread, plants on grass, moss
                                    else if (TileID.Sets.SpreadOverground[tileType] || Main.tileMoss[tileType] || TileID.Sets.tileMossBrick[tileType] || tileType is TileID.Chlorophyte or TileID.ChlorophyteBrick)
                                    {
                                        int dirt = -1;
                                        int dirt2 = -1;
                                        int grass2 = -1;
                                        int plants = -1;
                                        int plantChance = 1;
                                        switch (tileType)
                                        {
                                            case TileID.Grass:
                                                if (j <= Main.worldSurface || Main.remixWorld)
                                                {
                                                    dirt = TileID.Dirt;
                                                    plants = TileID.Plants;
                                                    plantChance = 3;
                                                }
                                                break;
                                            case TileID.HallowedGrass:
                                                if (j <= Main.worldSurface || Main.remixWorld)
                                                {
                                                    dirt = TileID.Dirt;
                                                    plants = TileID.HallowedPlants;
                                                    plantChance = 2;
                                                }
                                                if (!WorldGen.AllowedToSpreadInfections)
                                                {
                                                    continue;
                                                }
                                                break;
                                            case TileID.CorruptGrass:
                                                dirt = TileID.Dirt;
                                                dirt2 = TileID.Mud;
                                                grass2 = TileID.CorruptJungleGrass;
                                                plants = TileID.CorruptPlants;
                                                plantChance = 2;
                                                if (!WorldGen.AllowedToSpreadInfections)
                                                {
                                                    continue;
                                                }
                                                break;
                                            case TileID.CrimsonGrass:
                                                dirt = TileID.Dirt;
                                                dirt2 = TileID.Mud;
                                                grass2 = TileID.CrimsonJungleGrass;
                                                plants = TileID.CrimsonPlants;
                                                plantChance = 2;
                                                if (!WorldGen.AllowedToSpreadInfections)
                                                {
                                                    continue;
                                                }
                                                break;
                                            case TileID.CorruptJungleGrass:
                                                dirt = TileID.Mud;
                                                dirt2 = TileID.Dirt;
                                                grass2 = TileID.CorruptGrass;
                                                plants = TileID.CorruptPlants;
                                                plantChance = 2;
                                                if (!WorldGen.AllowedToSpreadInfections)
                                                {
                                                    continue;
                                                }
                                                break;
                                            case TileID.CrimsonJungleGrass:
                                                dirt = TileID.Mud;
                                                dirt2 = TileID.Dirt;
                                                grass2 = TileID.CrimsonGrass;
                                                plants = TileID.CrimsonPlants;
                                                plantChance = 2;
                                                if (!WorldGen.AllowedToSpreadInfections)
                                                {
                                                    continue;
                                                }
                                                break;
                                            case TileID.JungleGrass:
                                                dirt = TileID.Mud;
                                                plants = TileID.JunglePlants;
                                                plantChance = 4;
                                                break;
                                            case TileID.MushroomGrass:
                                                dirt = TileID.Mud;
                                                plants = TileID.MushroomPlants;
                                                plantChance = 4;
                                                break;
                                            case TileID.AshGrass:
                                                dirt = TileID.Ash;
                                                plants = TileID.AshPlants;
                                                plantChance = 2;
                                                break;
                                            default:
                                                //Moss is somewhat shoehorned in here.  The grass functions are correct except for sideways moss plants.
                                                if (Main.tileMoss[tileType] || TileID.Sets.tileMossBrick[tileType])
                                                {
                                                    if (Main.tileMoss[tileType])
                                                    {
                                                        dirt = TileID.Stone;
                                                        dirt2 = TileID.GrayBrick;
                                                    }
                                                    else
                                                    {
                                                        dirt = TileID.GrayBrick;
                                                        dirt2 = TileID.Stone;
                                                    }
                                                    plants = TileID.LongMoss;
                                                    plantChance = 4;
                                                    //Moss translation between stone variants
                                                    grass2 = tileType switch
                                                    {
                                                        182 => 515,
                                                        515 => 182,
                                                        180 => 513,
                                                        513 => 180,
                                                        179 => 512,
                                                        512 => 179,
                                                        381 => 517,
                                                        517 => 381,
                                                        534 => 535,
                                                        535 => 534,
                                                        536 => 537,
                                                        537 => 536,
                                                        539 => 540,
                                                        540 => 539,
                                                        625 => 626,
                                                        626 => 625,
                                                        627 => 628,
                                                        628 => 627,
                                                        183 => 516,
                                                        516 => 183,
                                                        181 => 514,
                                                        514 => 181,
                                                        _ => -1
                                                    };
                                                }
                                                break;
                                        }
                                        if (!tileAbove.HasTile && plants != -1 && WorldGen.genRand.NextBool(plantChance))
                                        {
                                            if (WorldGen.PlaceTile(i, j - 1, plants, mute: true))
                                            {
                                                plantsHaveGrown = true;
                                                Main.tile[i, j - 1].CopyPaintAndCoating(Main.tile[i, j]);
                                            }
                                            if (Main.netMode == NetmodeID.Server && plantsHaveGrown)
                                            {
                                                NetMessage.SendTileSquare(-1, i, j - 1);
                                            }
                                        }
                                        if (dirt != -1)
                                        {
                                            bool grassHasSpread = false;
                                            TileColorCache color = Main.tile[i, j].BlockColorAndCoating();
                                            //normally it'd be width /16 for world to tile conversion, but I wanted more
                                            for (int k = i - (projectile.width / 10) - 1; k < i + (projectile.width / 10) + 2; k++)
                                            {
                                                for (int l = j - (projectile.height / 10) - 1; l < j + (projectile.height / 10) + 2; l++)
                                                {
                                                    Tile tileAdj = Framing.GetTileSafely(k, l);
                                                    if ((i == k && j == l) || !tileAdj.HasTile)
                                                    {
                                                        continue;
                                                    }
                                                    if (tileAdj.TileType == dirt)
                                                    {
                                                        WorldGen.SpreadGrass(k, l, dirt, tileType, repeat: false, color);
                                                        if (tileAdj.TileType == tileType)
                                                        {
                                                            WorldGen.SquareTileFrame(k, l);
                                                            grassHasSpread = true;
                                                        }
                                                    }
                                                    else if (dirt2 > -1 && grass2 > -1 && tileAdj.TileType == dirt2)
                                                    {
                                                        WorldGen.SpreadGrass(k, l, dirt2, grass2, repeat: false, color);
                                                        if (tileAdj.TileType == grass2)
                                                        {
                                                            WorldGen.SquareTileFrame(k, l);
                                                            grassHasSpread = true;
                                                        }
                                                    }
                                                }
                                            }
                                            if (Main.netMode == NetmodeID.Server && grassHasSpread)
                                            {
                                                NetMessage.SendTileSquare(-1, i, j, 3);
                                            }
                                            if (plantsHaveGrown || grassHasSpread)
                                            {
                                                continue;
                                            }
                                        }
                                        switch (tileType)
                                        {
                                            //pumpkin
                                            case TileID.Grass:
                                            case TileID.HallowedGrass:
                                                if (Main.halloween && !tileAbove.HasTile && WorldGen.genRand.NextBool(20))
                                                {
                                                    int pumpSpacing = 100;
                                                    int pumpCount = 0;
                                                    for (int pumpCheckX = i - pumpSpacing; pumpCheckX < i + pumpSpacing; pumpCheckX += 2)
                                                    {
                                                        for (int pumpCheckY = j - pumpSpacing; pumpCheckY < j + pumpSpacing; pumpCheckY += 2)
                                                        {
                                                            if (pumpCheckX > 1 && pumpCheckX < Main.maxTilesX - 2 && pumpCheckY > 1 && pumpCheckY < Main.maxTilesY - 2 && Framing.GetTileSafely(pumpCheckX, pumpCheckY).HasTile && Framing.GetTileSafely(pumpCheckX, pumpCheckY).TileType == TileID.Pumpkins)
                                                            {
                                                                pumpCount++;
                                                            }
                                                        }
                                                    }
                                                    if (pumpCount < 3)
                                                    {
                                                        WorldGen.PlacePumpkin(i, j - 1);
                                                        if (Main.netMode == NetmodeID.Server && tileAbove.TileType == TileID.Pumpkins)
                                                        {
                                                            NetMessage.SendTileSquare(-1, i - 1, j - 1 - 1, 2, 2);
                                                        }
                                                    }
                                                }
                                                break;
                                            //mushroom tree
                                            case TileID.MushroomGrass:
                                                if (WorldGen.genRand.NextBool(10))
                                                {
                                                    if (WorldGen.GrowTree(i, j) && WorldGen.PlayerLOS(i, j))
                                                    {
                                                        WorldGen.TreeGrowFXCheck(i, j - 1);
                                                    }
                                                }
                                                break;
                                            case TileID.Chlorophyte:
                                            case TileID.ChlorophyteBrick:
                                                {
                                                    //Spread Chlorophyte
                                                    if (Main.hardMode && ((double)j > (Main.worldSurface + Main.rockLayer) / 2.0 || Main.remixWorld))
                                                    {
                                                        int chloroSpreadX = i;
                                                        int chloroSpreadY = j;
                                                        if (WorldGen.genRand.NextBool(2))
                                                        {
                                                            int chloroSpreadSide = WorldGen.genRand.Next(4);
                                                            if (chloroSpreadSide == 0)
                                                            {
                                                                chloroSpreadX++;
                                                            }
                                                            if (chloroSpreadSide == 1)
                                                            {
                                                                chloroSpreadX--;
                                                            }
                                                            if (chloroSpreadSide == 2)
                                                            {
                                                                chloroSpreadY++;
                                                            }
                                                            if (chloroSpreadSide == 3)
                                                            {
                                                                chloroSpreadY--;
                                                            }
                                                            Tile chloroSpreadTile = Framing.GetTileSafely(chloroSpreadX, chloroSpreadY);
                                                            if (WorldGen.InWorld(chloroSpreadX, chloroSpreadY, 2) && chloroSpreadTile.HasTile && (chloroSpreadTile.TileType is TileID.Mud or TileID.JungleGrass) && WorldGen.Chlorophyte(chloroSpreadX, chloroSpreadY))
                                                            {
                                                                chloroSpreadTile.TileType = TileID.Chlorophyte;
                                                                WorldGen.SquareTileFrame(chloroSpreadX, chloroSpreadY);
                                                                if (Main.netMode == NetmodeID.Server)
                                                                {
                                                                    NetMessage.SendTileSquare(-1, chloroSpreadX, chloroSpreadY);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            case TileID.JungleGrass:
                                                {
                                                    //Generate Chlorophyte
                                                    if (Main.hardMode && ((double)j > (Main.worldSurface + Main.rockLayer) / 2.0 || Main.remixWorld) && WorldGen.genRand.NextBool(10))
                                                    {
                                                        int chloroX = i + WorldGen.genRand.Next(-10, 11);
                                                        int chloroY = j + WorldGen.genRand.Next(-10, 11);
                                                        Tile chloroTile = Framing.GetTileSafely(chloroX, chloroY);
                                                        if (WorldGen.InWorld(chloroX, chloroY, 2) && chloroTile.HasTile && chloroTile.TileType == TileID.Mud && (!Framing.GetTileSafely(chloroX, chloroY - 1).HasTile || !(Framing.GetTileSafely(chloroX, chloroY - 1).TileType is TileID.Trees or TileID.LifeFruit or TileID.PlanteraBulb)) && WorldGen.Chlorophyte(chloroX, chloroY))
                                                        {
                                                            chloroTile.TileType = TileID.Chlorophyte;
                                                            WorldGen.SquareTileFrame(chloroX, chloroY);
                                                            if (Main.netMode == NetmodeID.Server)
                                                            {
                                                                NetMessage.SendTileSquare(-1, chloroX, chloroY);
                                                            }
                                                        }
                                                    }
                                                    if (tileAbove.HasTile)
                                                    {
                                                        break;
                                                    }
                                                    //Plantera Bulb
                                                    if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && WorldGen.genRand.NextBool(30))
                                                    {
                                                        bool bulbAllowed = true;
                                                        int bulbSpacing = 150;
                                                        for (int bulbCheckX = i - bulbSpacing; bulbCheckX < i + bulbSpacing; bulbCheckX += 2)
                                                        {
                                                            for (int bulbCheckY = j - bulbSpacing; bulbCheckY < j + bulbSpacing; bulbCheckY += 2)
                                                            {
                                                                if (bulbCheckX > 1 && bulbCheckX < Main.maxTilesX - 2 && bulbCheckY > 1 && bulbCheckY < Main.maxTilesY - 2 && Framing.GetTileSafely(bulbCheckX, bulbCheckY).HasTile && Framing.GetTileSafely(bulbCheckX, bulbCheckY).TileType == TileID.PlanteraBulb)
                                                                {
                                                                    bulbAllowed = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (bulbAllowed)
                                                        {
                                                            WorldGen.PlaceJunglePlant(i, j - 1, TileID.PlanteraBulb, 0, 0);
                                                            WorldGen.SquareTileFrame(i, j - 1);
                                                            WorldGen.SquareTileFrame(i + 2, j - 1);
                                                            WorldGen.SquareTileFrame(i - 1, j - 1);
                                                            if (tileAbove.TileType == TileID.PlanteraBulb && Main.netMode == NetmodeID.Server)
                                                            {
                                                                NetMessage.SendTileSquare(-1, i, j - 1, 5);
                                                            }
                                                            break;
                                                        }
                                                    }
                                                    //Life Fruit.  Has better chances and spacing in Expert+
                                                    if (Main.hardMode && NPC.downedMechBossAny && WorldGen.genRand.NextBool(Main.expertMode ? 26 : 30))
                                                    {
                                                        bool lifeFruitAllowed = true;
                                                        int lifeFruitSpacing = 60;
                                                        if (Main.expertMode)
                                                        {
                                                            lifeFruitSpacing -= 10;
                                                        }
                                                        for (int fruitCheckX = i - lifeFruitSpacing; fruitCheckX < i + lifeFruitSpacing; fruitCheckX += 2)
                                                        {
                                                            for (int fruitCheckY = j - lifeFruitSpacing; fruitCheckY < j + lifeFruitSpacing; fruitCheckY += 2)
                                                            {
                                                                if (fruitCheckX > 1 && fruitCheckX < Main.maxTilesX - 2 && fruitCheckY > 1 && fruitCheckY < Main.maxTilesY - 2 && Framing.GetTileSafely(fruitCheckX, fruitCheckY).HasTile && Framing.GetTileSafely(fruitCheckX, fruitCheckY).TileType == TileID.LifeFruit)
                                                                {
                                                                    lifeFruitAllowed = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (lifeFruitAllowed)
                                                        {
                                                            WorldGen.PlaceJunglePlant(i, j - 1, TileID.LifeFruit, WorldGen.genRand.Next(3), 0);//style, not chance
                                                            WorldGen.SquareTileFrame(i, j - 1);
                                                            WorldGen.SquareTileFrame(i + 1, j - 1 + 1);
                                                            if (tileAbove.TileType == TileID.LifeFruit && Main.netMode == NetmodeID.Server)
                                                            {
                                                                NetMessage.SendTileSquare(-1, i, j - 1, 4);
                                                            }
                                                            break;
                                                        }
                                                    }
                                                    //jungle tree
                                                    if ((j <= Main.worldSurface || Main.remixWorld) && WorldGen.genRand.NextBool(30))
                                                    {
                                                        if (WorldGen.GrowTree(i, j) && WorldGen.PlayerLOS(i, j))
                                                        {
                                                            WorldGen.TreeGrowFXCheck(i, j - 1);
                                                        }
                                                        break;
                                                    }
                                                    //jungle ferns
                                                    else if (WorldGen.genRand.NextBool(4))
                                                    {
                                                        WorldGen.PlaceJunglePlant(i, j - 1, TileID.PlantDetritus, WorldGen.genRand.Next(8), 0);//style, not chance
                                                        if (tileAbove.TileType is TileID.PlantDetritus)
                                                        {
                                                            if (Main.netMode == NetmodeID.Server)
                                                            {
                                                                NetMessage.SendTileSquare(-1, i, j - 1, 4);
                                                            }
                                                            //Small jungle plants won't render if this isn't run in this 'if'.
                                                            else
                                                            {
                                                                //little jungle ferns
                                                                WorldGen.PlaceJunglePlant(i, j - 1, TileID.PlantDetritus, WorldGen.genRand.Next(12), 1);//style, not chance
                                                                if (tileAbove.TileType == TileID.PlantDetritus && Main.netMode == NetmodeID.Server)
                                                                {
                                                                    NetMessage.SendTileSquare(-1, i, j - 1, 3);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                        }
                                        continue;
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }
}