using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MultiSaver.ConfigData
{
    public static class XMLHelper
    {

        public struct Attribute
        {

            public string Name;
            public string Value;

            public Attribute(string Name, string Value)
            {

                this.Name = Name;
                this.Value = Value;

            }



        }

        public class Node
        {

            public XmlNodeType NodeType;
            public String Name = string.Empty;
            public String Value = string.Empty;
            public Int32 Depth = 0;

            public List<Attribute> Attributes = new List<Attribute>();
            public List<Node> Children = new List<Node>();

            public Node Parent;

            public Node(int Depth)
            {

                this.Depth = Depth;

            }

        }

        public static Node LoadXML(String Location)
        {

            if (!File.Exists(Location))
                return null;

            XmlReader Reader;

            Stream S = new FileStream(Location, FileMode.Open);
            Reader = XmlReader.Create(S);

            Node RootNode = new Node(0);
            RootNode.Name = "Root Node";
            Node WorkingNode = new Node(0);
            WorkingNode.Parent = RootNode;

            while (Reader.Read())
            {

                //Ignore whitespace
                if (Reader.NodeType != XmlNodeType.Whitespace)
                {

                    //Moved down a tier
                    if (WorkingNode.Depth > Reader.Depth)
                    {

                        if (Reader.NodeType != XmlNodeType.EndElement)
                        {

                            RootNode = null;
                            break;

                        }

                        else
                        {

                            if (WorkingNode.Parent.Children.Count == 1 && Reader.NodeType == XmlNodeType.EndElement)
                            {

                                if (WorkingNode.Parent.Children[0].NodeType != XmlNodeType.Element)
                                {

                                    WorkingNode.Parent.Value = WorkingNode.Parent.Children[0].Value;
                                    WorkingNode.Parent.Children.Clear();

                                }

                            }

                            WorkingNode.Depth = Reader.Depth;
                            WorkingNode.Parent = WorkingNode.Parent.Parent;
                            continue;

                        }

                    }

                    //We moved back up a tier
                    else if (WorkingNode.Depth < Reader.Depth)
                    {

                        WorkingNode.Parent = WorkingNode.Parent.Children[WorkingNode.Parent.Children.Count - 1];
                        WorkingNode.Depth = Reader.Depth;

                    }

                    Node BuiltNode = new Node(WorkingNode.Depth);
                    BuiltNode.Parent = WorkingNode.Parent;

                    BuiltNode.NodeType = Reader.NodeType;
                    BuiltNode.Name = Reader.Name;
                    BuiltNode.Value = Reader.Value;

                    for (int i = 0; i < Reader.AttributeCount; i++)
                    {

                        Reader.MoveToAttribute(i);
                        BuiltNode.Attributes.Add(new Attribute() { Name = Reader.Name, Value = Reader.Value });

                    }

                    WorkingNode.Parent.Children.Add(BuiltNode);

                }

            }

            S.Close();

            return RootNode;

        }

    }

}
