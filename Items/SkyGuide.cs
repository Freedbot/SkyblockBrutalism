using Microsoft.Xna.Framework;
using SkyblockBrutalism.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace SkyblockBrutalism.Items
{
	public class SkyGuide : ModItem
	{

        public int SkyChoice;
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.ManaCrystal);
            Item.consumable = false;
            Item.ResearchUnlockCount = 3;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Cyan;
            Item.UseSound = null;
        }
        public override void SaveData(TagCompound tag)
        {
            tag["SkyChoices"] = SkyChoice;
        }
        public override void LoadData(TagCompound tag)
        {
            SkyChoice = tag.Get<int>("SkyChoices");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(SkyChoice);
        }
        public override void NetReceive(BinaryReader reader)
        {
            SkyChoice = reader.ReadInt32();
        }
        // If the player using the item is the client
        // (explicitly excluded serverside here)
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (ModContent.GetInstance<SkyUISystem>().IsUIOpen())
                {
                    SoundEngine.PlaySound(SoundID.MenuClose);
                    ModContent.GetInstance<SkyUISystem>().HideMyUI();
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.MenuOpen);
                    ModContent.GetInstance<SkyUISystem>().ShowMyUI(SkyChoice);
                }

            }

            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (SkyChoice == 0)
            {
                tooltips.RemoveAt(2);
            }
        }
    }
    //The book is given at the start of the game and otherwise unaquireable.
    public class PlayerStartingItems : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            if (!mediumCoreDeath && ModContent.GetInstance<Config>().SkyGuideStart)
            {
                return new[] {
                new Item(ModContent.ItemType<SkyGuide>()),
                };
            }
            else
            {
                return Enumerable.Empty<Item>();
            }
        }
    }
}