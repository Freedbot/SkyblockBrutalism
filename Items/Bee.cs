using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SkyblockBrutalism.Items
{
    //Bees are now just violent catchable critters.  See NPCEdits.
    //I changed progression so I don't need this...
    //but the memes...
    public class Bee : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Frog);
            Item.makeNPC = NPCID.BeeSmall;
            Item.value = 10; //1/20th the stinger value
        }
    }
}