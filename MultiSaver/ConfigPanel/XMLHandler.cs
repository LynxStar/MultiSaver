using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml
namespace ConfigPanel
{
    class XMLHandler
    {
        public static void save(List<GroupSetting> groups)
        {
            XmlWriter writer = XmlWriter.Create("config.xml");
            writer.WriteStartDocument();
            writer.WriteStartElement("Groups");
            foreach(GroupSetting gs in groups)
            {
                writer.WriteStartElement("Group");
                writer.WriteElementString("Name", gs.groupName);
                writer.WriteElementString("Type", gs.ssType);
                writer.WriteElementString("Active?", gs.isActive.ToString());
                writer.WriteElementString("Album", gs.albumLocation);
                writer.WriteElementString("PictureOrder", gs.order);
                writer.WriteElementString("MazeSize", gs.mazeSize.ToString());
                writer.WriteElementString("Pallet", gs.mazePalletName);
                foreach(MonitorSetting ms in gs.monitors)
                {
                    writer.WriteStartElement("Monitor");
                    writer.WriteElementString("ID", ms.monitorId.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        public static List<GroupSetting> load(string pathname)
        {

        }
    }
}
