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
            XmlReader reader = XmlReader.Create(pathname);
            List<GroupSetting> groups = new List<GroupSetting>();
            GroupSetting tempGroup = new GroupSetting();
            MonitorSetting tempMonitor = new MonitorSetting(0);
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "Group":
                            tempGroup = new GroupSetting();
                            break;
                        case "Name":
                            reader.Read();
                            tempGroup.groupName = reader.Value.Trim();
                            break;
                        case "isActive":
                            reader.Read();
                            tempGroup.isActive = bool.Parse(reader.Value.Trim());
                            break;
                        case "Album":
                            reader.Read();
                            tempGroup.albumLocation = reader.Value.Trim();
                            break;
                        case "PictureOrder":
                            reader.Read();
                            tempGroup.order = reader.Value.Trim();
                            break;
                        case "MazeSize":
                            reader.Read();
                            tempGroup.mazeSize = int.Parse(reader.Value);
                            break;
                        case "Pallet":
                            reader.Read();
                            tempGroup.mazePalletName = reader.Value.Trim();
                            break;
                        case "Monitor":
                            reader.Read();
                            if (reader.Name == "ID")
                            { 
                                reader.Read();
                                tempMonitor = new MonitorSetting(int.Parse(reader.Value.Trim()));
                            }
                            else
                            {
                                throw new Exception("No Monitor ID");
                            }
                            break;
                        case "FadeTime":
                            reader.Read();
                            tempMonitor.fadeTime = int.Parse(reader.Value.Trim());
                            break;
                        case "DisplayTime":
                            reader.Read();
                            tempMonitor.displayTime = int.Parse(reader.Value.Trim());
                            break;
                        case "PanTime":
                            reader.Read();
                            tempMonitor.panTime = int.Parse(reader.Value.Trim());
                            break;
                        case "PanDirIn":
                            reader.Read();
                            tempMonitor.dirIn = int.Parse(reader.Value.Trim());
                            break;
                        case "PanDirOut":
                            reader.Read();
                            tempMonitor.dirOut = int.Parse(reader.Value.Trim());
                            break;
                        case "IsClockwise":
                            reader.Read();
                            tempMonitor.clockwise = bool.Parse(reader.Value.Trim());
                            break;
                        case "NumRotations":
                            reader.Read();
                            tempMonitor.numRotations = int.Parse(reader.Value.Trim());
                            break;
                        case "AIView":
                            reader.Read();
                            tempMonitor.aiView = reader.Value.Trim();
                            break;
                        case "AIMethod":
                            reader.Read();
                            tempMonitor.aiMethod = reader.Value.Trim();
                            break;

                    }

                }
                else if (reader.MoveToContent().Equals(XmlNodeType.EndElement) && reader.Name == "Monitor")
                {
                    tempGroup.addMonitor(tempMonitor);
                }
                else if (reader.MoveToContent() == XmlNodeType.EndElement && reader.Name == "Group")
                {
                    groups.Add(tempGroup);
                }

            }
            return groups;
        }
    }
}
