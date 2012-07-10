using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigPanel
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


    }

    class MonitorSetting
    {
        public int monitorId;
        public int fadeTime;
        public int displayTime;
        int panTime;
        public int dirIn;
        public int dirOut;
        public bool clockwise;
        public int numRotations;

        public string aiView;
        public string aiMethod;


    }
}
