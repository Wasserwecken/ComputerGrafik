using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.LevelLoader.LevelItems
{
    public class PlayerBlockEnvironmentSet
    {
        public Block BlockLeft { get; set; }
        public Block BlockRight { get; set; }
        public Block BlockBottom { get; set; }
        public Block BlockTop { get; set; }
        public Block BlockBehind { get; set; }
    }
}
