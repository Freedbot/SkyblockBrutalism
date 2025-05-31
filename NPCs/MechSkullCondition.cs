using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SkyblockBrutalism.NPCs
{
    internal class MechSkullCondition : IItemDropRuleCondition, IProvideItemConditionDescription
    {
        private static LocalizedText Description;

        public MechSkullCondition()
        {
            Description ??= Language.GetOrRegister("Mods.SkyblockBrutalism.DropConditions.MechSkull");
        }
        public bool CanDrop(DropAttemptInfo info)
        {
            return ModContent.GetInstance<Config>().RestrictedMode && Main.getGoodWorld && Main.remixWorld && NPC.downedMechBoss3 && (!NPC.downedMechBoss1 || !NPC.downedMechBoss2);
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return Description.Value;
        }
    }
}
