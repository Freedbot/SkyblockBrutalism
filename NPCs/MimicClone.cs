using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.Utilities;

namespace SkyblockBrutalism.NPCs
{
    public class MimicClone : ModNPC
    {
        //A simple mimic clone designed to be KeyOfMagic summonable and drop chest loot like Remix mimics, but with no money and not limited to pre-hardmode
        //See also KeyOfMagic and SkyPlayer (spawn mechanics).
        public override string Texture => $"Terraria/Images/NPC_{NPCID.Mimic}";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Mimic];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Mimic);
            NPC.value = 0f;
            if (!Main.hardMode)
            {
                NPC.damage = 30;
                NPC.lifeMax = 300;
                NPC.defense = 12;
            }
            else
            {
                NPC.damage = 80;
                NPC.lifeMax = 500;
                NPC.defense = 30;
            }
            AIType = NPCID.Mimic; 
            AnimationType = NPCID.Mimic; // Use vanilla mimic's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
            Banner = Item.NPCtoBanner(NPCID.Mimic);
            BannerItem = Item.BannerToItem(Banner); // Makes kills of this NPC go towards dropping the banner it's associated with.
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptions(1, 
                ItemID.BandofRegeneration,
                ItemID.MagicMirror,
                ItemID.CloudinaBottle,
                ItemID.HermesBoots,
                ItemID.Mace,
                ItemID.ShoeSpikes
                ));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.SkyblockBrutalism.NPCs.MimicClone.BestiaryInfo")),
            });
        }
    }
}
