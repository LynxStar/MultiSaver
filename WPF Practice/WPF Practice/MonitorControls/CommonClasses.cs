using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Practice.MonitorControls
{
        public class SlideShowInfo
        {
            public int groupId;
            public int FadeTime;
            public int DisplayTime;
            public int PanTime;
            public int Rotation;
            public bool Clockwise;
            public bool Alphabetical;
            public bool RevAlphabetical;
            public bool Random;
            public char dIn;
            public char dout;

            public SlideShowInfo(int Id)
            {
                groupId = Id;
                FadeTime = 0;
                DisplayTime = 0;
                PanTime = 0;
                Rotation = 0;
                Clockwise = false;
                Alphabetical = true;
                RevAlphabetical = false;
                Random = false;
                dIn = ' ';
                dout = ' ';
            }
        }

        public class Group
        {
            public string name;
            public int groupid;
            public int screensavertype;
            public int screensaverID;
            public List<string> ownedMonitors;

            public Group( int groupID)
            {
                name = ""; groupid = groupID;
                ownedMonitors = new List<string>();
            }
        }


        public class MazeInfo
        {

        }
}
