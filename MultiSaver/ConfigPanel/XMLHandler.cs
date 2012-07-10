using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace ConfigPanel
{
    class XMLHandler
    {
        public static void save(List<GroupSetting> groups)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            XmlWriter writer = XmlWriter.Create("config.xml", settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Groups");
            foreach(GroupSetting gs in groups)
            {
                writer.WriteStartElement("Group");
                writer.WriteElementString("Name", gs.groupName);
                writer.WriteElementString("Type", gs.ssType);
                writer.WriteElementString("isActive", gs.isActive.ToString());
                writer.WriteElementString("Album", gs.albumLocation);
                writer.WriteElementString("PictureOrder", gs.order);
                writer.WriteElementString("MazeSize", gs.mazeSize.ToString());
                writer.WriteElementString("Pallet", gs.mazePalletName);
                foreach(MonitorSetting ms in gs.monitors)
                {
                    writer.WriteStartElement("Monitor");
                    writer.WriteElementString("ID", ms.monitorId.ToString());
                    writer.WriteElementString("FadeTime", ms.fadeTime.ToString());
                    writer.WriteElementString("DisplayTime", ms.displayTime.ToString());
                    writer.WriteElementString("PanTime", ms.panTime.ToString());
                    writer.WriteElementString("PanDirIn", ms.dirIn.ToString());
                    writer.WriteElementString("PanDirOut", ms.dirOut.ToString());
                    writer.WriteElementString("isClockwise", ms.clockwise.ToString());
                    writer.WriteElementString("NumRotations", ms.numRotations.ToString());
                    writer.WriteElementString("AIView", ms.aiView);
                    writer.WriteElementString("AIMethod", ms.aiMethod);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public static List<GroupSetting> load(string pathname)
        {
            return null;
        }
    }
}
