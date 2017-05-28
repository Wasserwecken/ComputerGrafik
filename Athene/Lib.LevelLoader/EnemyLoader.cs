using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.LevelLoader.Xml;
using Lib.LevelLoader.Xml.Root;

namespace Lib.LevelLoader
{
    public class EnemyLoader
    {
        public static XmlEnemyList GetEnemies()
        {
            string path = @"Enemies\Enemies.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(XmlEnemyList));
            StreamReader reader = new StreamReader(path);
            XmlEnemyList xmlEnemyList = (XmlEnemyList)serializer.Deserialize(reader);
            return xmlEnemyList;
        }
    }
}
