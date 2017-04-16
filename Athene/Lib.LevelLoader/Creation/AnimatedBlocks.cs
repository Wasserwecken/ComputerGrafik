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
            animatedBlockList.Add("water", new AnimatedBlockInformation("water", "Animations/water", 2500));
            animatedBlockList.Add("water2", new AnimatedBlockInformation("water2", "Animations/water2", 2500));
        }

        private static Dictionary<string, AnimatedBlockInformation> animatedBlockList;
    }
}
