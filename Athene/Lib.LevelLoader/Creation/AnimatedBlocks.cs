using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Visuals.Graphics;

namespace Lib.LevelLoader.Creation
{
    public static class AnimatedBlocks
    {
        public static Dictionary<string, AnimatedBlockInformation> Get()
        {
            Init();
            return animatedBlockList;
        }

        public static void Init()
        {
            animatedBlockList = new Dictionary<string, AnimatedBlockInformation>();
            animatedBlockList.Add("water_top", new AnimatedBlockInformation("water_top", "Animations/water_top", 2500));

        }

        private static Dictionary<string, AnimatedBlockInformation> animatedBlockList;
    }
}
