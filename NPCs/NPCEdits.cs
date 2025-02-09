using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using SkyblockBrutalism.Items;
using Terraria.ModLoader.Utilities;

namespace SkyblockBrutalism.NPCs
{
    public class NPCEdits : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            //Adds pinky to the general mob spawn pools in 3 layers at roughly the same rate as vanilla, effectively doubling the rate, theoretically more in cavern.
            //This shouldn't affect slime rain or windy day spawns.
            if (spawnInfo.Player.InZonePurity())
            {
                if ((spawnInfo.Player.ZoneDirtLayerHeight && !Main.remixWorld) || (spawnInfo.Player.ZoneRockLayerHeight && Main.remixWorld))
                {
                    pool[NPCID.Pinky] = SpawnCondition.Underground.Chance * .005f;
                }
                else if ((spawnInfo.Player.ZoneRockLayerHeight && !Main.remixWorld) || (spawnInfo.Player.ZoneDirtLayerHeight && Main.remixWorld))
                {
                    pool[NPCID.Pinky] = SpawnCondition.Cavern.Chance * .0025f;
                }
                else pool[NPCID.Pinky] = SpawnCondition.OverworldDaySlime.Chance * .005f;
            }
        }
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Dryad)
            {
                shop.Add(ItemID.PurificationPowder, Condition.RemixWorld, Condition.NotBloodMoon);
            }
            if (shop.NpcType == NPCID.Steampunker)
            {
                shop.Add(ItemID.Clentaminator, Condition.RemixWorld);
                shop.GetEntry(ItemID.PurpleSolution).AddCondition(Condition.NotInGraveyard);
                shop.GetEntry(ItemID.RedSolution).AddCondition(Condition.NotInGraveyard);
                shop.Add(ItemID.PurpleSolution, Condition.RemixWorld, Condition.EclipseOrBloodMoon, Condition.CorruptWorld);
                shop.Add(ItemID.RedSolution, Condition.RemixWorld, Condition.EclipseOrBloodMoon, Condition.CrimsonWorld);
                shop.Add(ItemID.PurpleSolution, Condition.EclipseOrBloodMoon, Condition.CrimsonWorld, Condition.InGraveyard);
                shop.Add(ItemID.RedSolution, Condition.EclipseOrBloodMoon, Condition.CorruptWorld, Condition.InGraveyard);
                shop.Add(ItemID.BlueSolution, Condition.RemixWorld, Condition.InHallow, Condition.NotEclipseAndNotBloodMoon);
                shop.Add(ItemID.GreenSolution, Condition.RemixWorld, Condition.NotInHallow, Condition.NotEclipseAndNotBloodMoon);
                shop.Add(ItemID.SandSolution, Condition.RemixWorld, Condition.DownedMartians);
                shop.Add(ItemID.SnowSolution, Condition.RemixWorld, Condition.DownedMartians);
                shop.Add(ItemID.DirtSolution, Condition.RemixWorld, Condition.DownedMartians);
                shop.Add(ItemID.SandSolution, Condition.NotDownedMoonLord, Condition.DownedMartians);
                shop.Add(ItemID.SnowSolution, Condition.NotDownedMoonLord, Condition.DownedMartians);
                shop.Add(ItemID.DirtSolution, Condition.NotDownedMoonLord, Condition.DownedMartians);
            }
            if (shop.NpcType == NPCID.Truffle)
            {
                shop.Add(ItemID.DarkBlueSolution, Condition.RemixWorld);
            }
            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                shop.Add(new Item(ItemID.GoldChest) { shopCustomPrice = Item.buyPrice(gold: 1) });
                shop.Add(ItemID.Spear, Condition.MoonPhaseWaningCrescent);
                shop.Add(ItemID.Blowpipe, Condition.MoonPhaseFirstQuarter);
            }
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add(ItemID.FlareGun, Condition.NpcIsPresent(NPCID.ArmsDealer));
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {

            //Gravedigger Shovel gets the dirt.  Retier was required, See RecipeEdits and ItemEdits.
            //Too damn many zombies!
            if (npc.type == NPCID.Zombie || npc.type == NPCID.ZombieDoctor || npc.type == NPCID.ZombieEskimo || npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat || npc.type == NPCID.ZombiePixie || npc.type == NPCID.ZombieRaincoat || npc.type == NPCID.ZombieSuperman || npc.type == NPCID.ZombieSweater || npc.type == NPCID.ZombieXmas || npc.type == NPCID.ArmedTorchZombie || npc.type == NPCID.ArmedZombie || npc.type == NPCID.ArmedZombieCenx || npc.type == NPCID.ArmedZombieEskimo || npc.type == NPCID.ArmedZombiePincussion || npc.type == NPCID.ArmedZombieSlimed || npc.type == NPCID.ArmedZombieSwamp || npc.type == NPCID.ArmedZombieTwiggy || npc.type == NPCID.BaldZombie || npc.type == NPCID.BigBaldZombie || npc.type == NPCID.BigFemaleZombie || npc.type == NPCID.BigPincushionZombie || npc.type == NPCID.BigRainZombie || npc.type == NPCID.BigSlimedZombie || npc.type == NPCID.BigSwampZombie || npc.type == NPCID.BigTwiggyZombie || npc.type == NPCID.BigZombie || npc.type == NPCID.FemaleZombie || npc.type == NPCID.MaggotZombie || npc.type == NPCID.PincushionZombie || npc.type == NPCID.SlimedZombie || npc.type == NPCID.SmallBaldZombie || npc.type == NPCID.SmallFemaleZombie || npc.type == NPCID.SmallPincushionZombie || npc.type == NPCID.SmallRainZombie || npc.type == NPCID.SmallSlimedZombie || npc.type == NPCID.SmallSwampZombie || npc.type == NPCID.SmallTwiggyZombie || npc.type == NPCID.SmallZombie || npc.type == NPCID.SwampZombie || npc.type == NPCID.TorchZombie || npc.type == NPCID.TwiggyZombie)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new DirtDropCondition(), ItemID.DirtBlock));
            }
            if (npc.type == NPCID.Deerclops)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.IceMachine));
            }
            if (npc.type == NPCID.VoodooDemon && ModContent.GetInstance<Config>().LifelessVoodooDoll)
            {
                npcLoot.RemoveWhere(
                    rule => rule is CommonDrop drop
                    && drop.itemId == ItemID.GuideVoodooDoll);
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LifelessVoodooDoll>()));
            }
            if (npc.type is NPCID.WallCreeper or NPCID.BlackRecluse)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.WebSlinger, 200));
            }
        }
        public override void OnKill(NPC npc)
        {
            //On kill, sends boss NPCID to SkyPlayer for saving to list and to check before force feeding heart crystals/fruit.  See SkyPlayer, SkyblockBrutalism(networking).
            if (npc.boss)
            {
                // Handle boss defeated for all players that interacted with the boss
                for (var i = 0; i < Main.maxPlayers; i++)
                {
                    var player = Main.player[i];
                    //skips to the next player[i], the negative check avoids errors as well
                    if (!player.active || !npc.playerInteraction[i])
                    {
                        continue;
                    }
                    // Called by single-player or server
                    player.GetModPlayer<SkyPlayer>().HandleBossDefeated(npc.type);
                }
            }
        }
        //Finally, a fix for the void bug that leaves town NPC's trapped in the void without dying or respawning.  Turns out there's a not Christmas check for no reason in the kill code.
        public override void PostAI(NPC npc)
        {
            if (Main.xMas)
            {
                if (npc.aiStyle == NPCAIStyleID.Passive && npc.position.Y > Main.bottomWorld - 640f + (float)npc.height && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.StrikeInstantKill();
                }
            }
        }
        //Retier Frost Legion and Rock Golem to early game.  See Items.ItemEdits.cs
        //See ProjEdits for their ranged attack nerfs.
        public override void SetDefaults(NPC entity)
        {
            if (!Main.hardMode)
            {
                if (entity.type == NPCID.RockGolem || entity.type == NPCID.MisterStabby || entity.type == NPCID.SnowmanGangsta || entity.type == NPCID.SnowBalla)
                {
                    entity.damage /= 4;
                    entity.lifeMax /= 4;
                    if (entity.type == NPCID.RockGolem)
                    {
                        entity.defense = 10;
                        entity.value /= 2;
                    }
                    else
                    {
                        entity.defense = 0;
                    }
                }
            }
            //Crude prevention of direct WoF summoning without voodoo doll
            if (entity.type == NPCID.Guide && ModContent.GetInstance<Config>().LifelessVoodooDoll)
            {
                entity.lavaImmune = true;
            }
            //Bees are critters. I changed progression so I don't need this...
            //but the memes...  See Items/Bee.cs
            if (entity.type == NPCID.Bee || entity.type == NPCID.BeeSmall)
            {
                Main.npcCatchable[entity.type] = true;
                entity.catchItem = ModContent.ItemType<Items.Bee>();
            }
        }
    }
}
