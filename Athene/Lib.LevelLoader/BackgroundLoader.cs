using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml.LevelItems;
using Lib.LevelLoader.Xml.Root;

namespace Lib.LevelLoader
{
    public class BackgroundLoader
    {
        public static List<XmlBackgroundItem> GetBackgrounds()
        {
            List<XmlBackgroundItem> items = new List<XmlBackgroundItem>();
            var files = Directory.GetFiles("Images/Backgrounds/");
            foreach (var file in files)
            {
                XmlBackgroundItem item = new XmlBackgroundItem()
                {
                    Path = file
                };
                items.Add(item);

            }

            return items;
        }
    }
}
