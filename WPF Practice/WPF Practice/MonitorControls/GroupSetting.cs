using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_Practice.MonitorControls
{
    public class GroupSetting
    {
        public string groupName;
        public List<MonitorSetting> monitors;

        public string ssType;
        public bool isActive;
        public string albumLocation;
        public string order;
        public int mazeSize;
        public string mazePalletName;

        public GroupSetting()
        {
            groupName = "Group";
            
            isActive = false;
            albumLocation = "C:\\Users\\Public\\Pictures";
            order = "Alphabetical";
            mazeSize = 10;
            mazePalletName = "Basic.png";
            monitors = new List<MonitorSetting>();
        }

        public void addMonitor(string id)
        {
            monitors.Add(new MonitorSetting(id));
        }

        public void addMonitor(MonitorSetting ms)
        {
            monitors.Add(ms);
        }

    }

    public class MonitorSetting
    {
        public  string monitorId; //missing
        public string transitionType;
        public int fadeTime;
        public int displayTime;
        public int panTime;
        public int dirIn;
        public int dirOut;
        public bool clockwise;
        public int numRotations;

        public string aiView;
        public string aiMethod;

        public MonitorSetting(string id)
        {
            monitorId = id;
            fadeTime = 1;
            displayTime = 1;
            panTime = 3;
            dirIn = 90;
            dirOut = 270;
            clockwise = true;
            numRotations = 1;
            transitionType = "Fade";

            aiMethod = "Depth First";
            aiView = "First Person";
        }
    }
}
