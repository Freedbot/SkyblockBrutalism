using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using static tModPorter.ProgressUpdate;

namespace SkyblockBrutalism
{
    //The wall at the Dungeon Point is unbreakable to preserve the landmark
    public class DungeonPoint : GlobalWall
    {
        public override void KillWall(int i, int j, int type, ref bool fail)
        {
            if (i == Main.dungeonX && j == Main.dungeonY)
            {
                fail = true;
            }
        }
    }
}
