using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader.Creation
{
    public class AnimatedBlockInformation
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int AnimationLength { get; set; }

        public AnimatedBlockInformation()
        {
            
        }

        public AnimatedBlockInformation(string name, string path, int animationLength)
        {
            Name = name;
            Path = path;
            AnimationLength = animationLength;
        }

        public FileInfo GetFirstImage()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory() + @"\" + Path).GetFiles()[0];
        }
    }
}
