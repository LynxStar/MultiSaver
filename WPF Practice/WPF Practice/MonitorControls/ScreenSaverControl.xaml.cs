using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WPF_Practice.MonitorControls
{
    /// <summary>
    /// Interaction logic for ScreenSaverControl.xaml
    /// </summary>
    public partial class ScreenSaverControl : UserControl
    {
       private GroupControl gcontrol = new GroupControl();
       private List<GroupSetting> groupsettings = new List<GroupSetting>();
       private List<List<string>> ownedmonitors = new List<List<string>>();
       //private List<List<string>> unassignedMonitors = new List<List<string>>();
        //Common Classes holds the classes in which we transfer things from the form to the listsz
       private int currentActiveGroup;

       public GroupSetting getGroupSettings(int place)
       {
           return groupsettings[place];
       }

        public ScreenSaverControl()
        {
            InitializeComponent();
            List<string> tmpMonitors = new List<string>();

            //groupsettings = XMLHandler.load("./config.xml");
           /* foreach (System.Windows.Forms.Screen Screen in System.Windows.Forms.Screen.AllScreens)
            {

                tmpMonitors.Add((Screen.Primary ? "Primary" : "Secondary") + "Monitor");

            }
            unassignedMonitors.Add(tmpMonitors);*/

        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public int createnewGroup()
        {
            int groupid = groupsettings.Count + 1;
            List<string> tmpstring = new List<string>();
            groupsettings.Add(new GroupSetting());
            ownedmonitors.Add(new List<string>());
           /* foreach (string str in unassignedMonitors[0])
                tmpstring.Add(str);
            unassignedMonitors.Add(tmpstring); */
            groupsettings[groupsettings.Count - 1].groupName = "New Group";

            Debug.WriteLine(String.Format("GroupSettings: {0} Ownded MOnitors: {1}", groupsettings.Count, ownedmonitors.Count));

            return groupsettings.Count - 1;
        }
           

        public List<GroupSetting> getGroupSettings()
        {
            return groupsettings;
        }

        public void reset()
        {

            if (mainPanel.Children.Count != 0)
                mainPanel.Children.RemoveAt(0);
        }

        public int getTotalNumberofGroups()
        {
            return groupsettings.Count;
        }

        public void displayGroupControl(int selectedScreen)
        {
            Debug.WriteLine("Selected Screen: {0}", selectedScreen);
            reset();
            gcontrol = new GroupControl();
            gcontrol.groupSetting = groupsettings[selectedScreen];
            //gcontrol.AvailableMonitors = unassignedMonitors[selectedScreen + 1];
            List<string> unassignedMonitors = new List<string>();
            int monitorcount = 1;
            foreach (System.Windows.Forms.Screen Screen in System.Windows.Forms.Screen.AllScreens)
            {
                string tmpMonitor ="Monitor " + monitorcount++;
                if (!ownedmonitors[selectedScreen].Contains(Screen.DeviceName))
                {
                    unassignedMonitors.Add(tmpMonitor);
                }
                else
                {
                    gcontrol.OwnedMonitors.Add(tmpMonitor);
                }

            }
            gcontrol.AvailableMonitors = unassignedMonitors;
           // gcontrol.OwnedMonitors = ownedmonitors[selectedScreen];
            mainPanel.Children.Add(gcontrol);
            currentActiveGroup = selectedScreen;
        }

        public void saveGroupSettings()
        {

            groupsettings[currentActiveGroup] = gcontrol.groupSetting;
            ownedmonitors[currentActiveGroup] = gcontrol.OwnedMonitors;
            //unassignedMonitors[currentActiveGroup] = gcontrol.AvailableMonitors;
        }
        public void deleteGroup(int selectedScreen)
        {
            Debug.WriteLine("Selected Screen: {0}", selectedScreen);
            groupsettings.RemoveAt(selectedScreen);
            ownedmonitors.RemoveAt(selectedScreen);
            //unassignedMonitors.RemoveAt(selectedScreen + 1);
            reset();
        }
    }
}
