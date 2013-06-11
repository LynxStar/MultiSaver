using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlHelper = MultiSaver.ConfigData.XMLHelper;

namespace MultiSaver.ConfigData
{

    public class Settings
    {

        public List<Monitor> Unassigned = new List<Monitor>();
        public List<Monitor> Maze = new List<Monitor>();
        public List<Monitor> Slideshow = new List<Monitor>();

        public void SetMonitorMode(String DisplayName, String Mode)
        {
             List<Monitor> PutIn;

            bool Found = false;

            if (Mode == "Unassigned")
                PutIn = Unassigned;
            else if (Mode == "Maze")
                PutIn = Maze;
            else
                PutIn = Slideshow;

            foreach (Monitor M in Unassigned)
            {

                if (M.ID == DisplayName)
                {

                    Found = true;
                    Unassigned.Remove(M);
                    PutIn.Add(M);
                    break;

                }

            }

            if (!Found)
            {

                foreach (Monitor M in Maze)
                {

                    if (M.ID == DisplayName)
                    {

                        Found = true;
                        Maze.Remove(M);
                        PutIn.Add(M);
                        break;

                    }

                }

            }

            if (!Found)
            {

                foreach (Monitor M in Slideshow)
                {

                    if (M.ID == DisplayName)
                    {

                        Found = true;
                        Slideshow.Remove(M);
                        PutIn.Add(M);
                        break;

                    }

                }

            }

        }

        public Monitor FindMonitor(int ID)
        {

            foreach (Monitor M in Slideshow)
            {

                if (M.IDNumber == ID)
                    return M;

            }

            return null;

        }

        public void Load()
        {

            XmlHelper.Node Configuration = XMLHelper.LoadXML("MultiSaverConfiguration.xml");

            if (Configuration != null)
            {

                Unassigned.Clear();
                Maze.Clear();
                Slideshow.Clear();

                XmlHelper.Node UnassignedMonitors = Configuration.Children[1].Children[0];

                foreach (XmlHelper.Node N in UnassignedMonitors.Children)
                {

                    Monitor M = new Monitor();
                    M.ID = N.Attributes[0].Value;
                    M.Bounds = new System.Drawing.Rectangle(Convert.ToInt32(N.Attributes[1].Value),
                        Convert.ToInt32(N.Attributes[2].Value),
                        Convert.ToInt32(N.Attributes[3].Value),
                        Convert.ToInt32(N.Attributes[4].Value));

                    Unassigned.Add(M);

                }

                XmlHelper.Node MazeMonitors = Configuration.Children[1].Children[1];

                foreach (XmlHelper.Node N in MazeMonitors.Children)
                {

                    Monitor M = new Monitor();
                    M.ID = N.Attributes[0].Value;
                    M.Bounds = new System.Drawing.Rectangle(Convert.ToInt32(N.Attributes[1].Value),
                        Convert.ToInt32(N.Attributes[2].Value),
                        Convert.ToInt32(N.Attributes[3].Value),
                        Convert.ToInt32(N.Attributes[4].Value));

                    Maze.Add(M);

                }

                XmlHelper.Node SlideshowMonitors = Configuration.Children[1].Children[2];

                foreach (XmlHelper.Node N in SlideshowMonitors.Children)
                {

                    Monitor M = new Monitor();
                    M.ID = N.Attributes[0].Value;
                    M.Bounds = new System.Drawing.Rectangle(Convert.ToInt32(N.Attributes[1].Value),
                        Convert.ToInt32(N.Attributes[2].Value),
                        Convert.ToInt32(N.Attributes[3].Value),
                        Convert.ToInt32(N.Attributes[4].Value));

                    M.TransitionMode = N.Attributes[5].Value;

                    M.Source = N.Attributes[6].Value;
                    M.Order = N.Attributes[7].Value;

                    M.TileType = N.Attributes[8].Value;
                    M.FixedTiles = Convert.ToInt32(N.Attributes[9].Value);
                    M.MinTiles = Convert.ToInt32(N.Attributes[10].Value);
                    M.MaxTiles = Convert.ToInt32(N.Attributes[11].Value);

                    M.TransitionTime = N.Attributes[12].Value;
                    M.FixedTime = Convert.ToInt32(N.Attributes[13].Value);
                    M.MinTime = Convert.ToInt32(N.Attributes[14].Value);
                    M.MaxTime = Convert.ToInt32(N.Attributes[15].Value);

                    Slideshow.Add(M);

                }

            }

        }

        public void Save()
        {

            XmlWriterSettings WriterSettings = new XmlWriterSettings();
            WriterSettings.Indent = true;

            XmlWriter Writer = XmlWriter.Create("MultiSaverConfiguration.xml", WriterSettings);

            Writer.WriteStartDocument();

            Writer.WriteStartElement("Configuration");

            #region Unassigned

            Writer.WriteStartElement("Unassigned");

            foreach (Monitor M in Unassigned)
            {

                Writer.WriteStartElement("Monitor");

                Writer.WriteAttributeString("Name", M.ID);

                Writer.WriteAttributeString("X", M.Bounds.X.ToString());
                Writer.WriteAttributeString("Y", M.Bounds.Y.ToString());
                Writer.WriteAttributeString("Width", M.Bounds.Width.ToString());
                Writer.WriteAttributeString("Height", M.Bounds.Height.ToString());

                Writer.WriteEndElement();

            }

            Writer.WriteEndElement();

            #endregion

            #region Maze

            Writer.WriteStartElement("Maze");

            foreach (Monitor M in Maze)
            {

                Writer.WriteStartElement("Monitor");

                Writer.WriteAttributeString("Name", M.ID);

                Writer.WriteAttributeString("X", M.Bounds.X.ToString());
                Writer.WriteAttributeString("Y", M.Bounds.Y.ToString());
                Writer.WriteAttributeString("Width", M.Bounds.Width.ToString());
                Writer.WriteAttributeString("Height", M.Bounds.Height.ToString());

                Writer.WriteEndElement();

            }

            Writer.WriteEndElement();

            #endregion

            #region Slideshow

            Writer.WriteStartElement("Slideshow");

            foreach (Monitor M in Slideshow)
            {

                Writer.WriteStartElement("Monitor");

                Writer.WriteAttributeString("Name", M.ID);

                Writer.WriteAttributeString("X", M.Bounds.X.ToString());
                Writer.WriteAttributeString("Y", M.Bounds.Y.ToString());
                Writer.WriteAttributeString("Width", M.Bounds.Width.ToString());
                Writer.WriteAttributeString("Height", M.Bounds.Height.ToString());

                Writer.WriteAttributeString("TransitionMode", M.TransitionMode);

                Writer.WriteAttributeString("Source", M.Source);
                Writer.WriteAttributeString("Order", M.Order);

                Writer.WriteAttributeString("TileType", M.TileType);
                Writer.WriteAttributeString("FixedTiles", Convert.ToString(M.FixedTiles));
                Writer.WriteAttributeString("MinTiles", Convert.ToString(M.MinTiles));
                Writer.WriteAttributeString("MaxTiles", Convert.ToString(M.MaxTiles));

                Writer.WriteAttributeString("TransitionTime", M.TransitionTime);
                Writer.WriteAttributeString("FixedTime", Convert.ToString(M.FixedTime));
                Writer.WriteAttributeString("MinTime", Convert.ToString(M.MinTime));
                Writer.WriteAttributeString("MaxTime", Convert.ToString(M.MaxTime));

                Writer.WriteEndElement();

            }

            Writer.WriteEndElement();

            #endregion

            Writer.WriteEndDocument();

            Writer.Flush();
            Writer.Close();

        }

    }

}
