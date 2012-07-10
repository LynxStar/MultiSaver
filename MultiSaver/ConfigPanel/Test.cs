using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigPanel
{
    class Test
    {
        public static void main(String[] args)
        {
            List<GroupSetting> groups = new List<GroupSetting>();
            GroupSetting group = new GroupSetting();
            group.addMonitor(1);
            groups.Add(group);
            XMLHandler.save(groups);
        }
    }
}
