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
        
        //Common Classes holds the classes in which we transfer things from the form to the listsz

       private List<string> unassignedMonitors = new List<string>();
       private int currentActiveGroup;

        public ScreenSaverControl()
        {
            InitializeComponent();
            //unassignedMonitors.Add("Dynex 19\" Monitor");
            //unassignedMonitors.Add("Acer 23\" Monitor");
            //unassignedMonitors.Add("Projector");

            //groupsettings = XMLHandler.load("./config.xml");
            foreach (System.Windows.Forms.Screen Screen in System.Windows.Forms.Screen.AllScreens)
            {

                unassignedMonitors.Add((Screen.Primary ? "Primary" : "Secondary") + "Monitor");

            }

        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public int createnewGroup()
        {
            int groupid = groupsettings.Count + 1;
            groupsettings.Add(new GroupSetting());
            ownedmonitors.Add(new List<string>());
            groupsettings[groupsettings.Count - 1].groupName = "Unnamed";
            return groupsettings.Count - 1;
        }

        public void createNewSlideShow(SlideShowInfo info)
        {
            
        }

        public string getCurrentGroupName()
        {
            return gcontrol.Name;
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
            reset();
            gcontrol = new GroupControl();
            gcontrol.groupSetting = groupsettings[selectedScreen];
            gcontrol.AssignAvailableString(unassignedMonitors);
            gcontrol.AssignOwnedStrings(ownedmonitors[selectedScreen]);
            mainPanel.Children.Add(gcontrol);
            currentActiveGroup = selectedScreen;
        }

        public void saveGroupSettings()
        {

            groupsettings[currentActiveGroup] = gcontrol.groupSetting;
            ownedmonitors[currentActiveGroup] = gcontrol.getOwnedScreens();

        }
        public void deleteGroup(int selectedScreen)
        {
            groupsettings.RemoveAt(selectedScreen);
        }

    }
}
