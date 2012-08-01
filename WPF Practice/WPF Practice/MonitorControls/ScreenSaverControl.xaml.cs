﻿using System;
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
using Winforms = System.Windows.Forms;

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
       private int currentActiveGroup;

       public delegate void ChangedGroupSettings(object sender, EventArgs e);
       public event ChangedGroupSettings changed;

       public GroupSetting getGroupSettings(int place)
       {
           return groupsettings[place];
       }

       protected virtual void OnChanged(EventArgs e)
       {
           if (changed != null)
               changed(this, e);
       }

       public List<List<string>> getOwnedMonitors()
       {
           return ownedmonitors;
       }

        public ScreenSaverControl()
        {
            InitializeComponent();
        }

        public ScreenSaverControl(List<GroupSetting> settings)
        {
            InitializeComponent();
            groupsettings = settings;
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public int createnewGroup()
        {
            
            List<string> tmpstring = new List<string>();
            groupsettings.Add(new GroupSetting());
            ownedmonitors.Add(new List<string>());
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
            gcontrol.isActive.Click += new RoutedEventHandler(isActive_Click);
            List<string> unassignedMonitors = new List<string>();
            int monitorcount = 1;
            foreach (System.Windows.Forms.Screen Screen in System.Windows.Forms.Screen.AllScreens)
            {
                string tmpMonitor ="Monitor " + monitorcount;
                if (!ownedmonitors[selectedScreen].Contains(tmpMonitor))
                {
                    unassignedMonitors.Add(tmpMonitor);
                }
                else
                {
                    gcontrol.OwnedMonitors.Add(tmpMonitor);
                }
                monitorcount++;

            }
            gcontrol.AvailableMonitors = unassignedMonitors;
           // gcontrol.OwnedMonitors = ownedmonitors[selectedScreen];
            mainPanel.Children.Add(gcontrol);
            currentActiveGroup = selectedScreen;
        }

        private void isActive_Click(object sender, RoutedEventArgs e)
        {
            List<string> tmpstrings = new List<string>();
            tmpstrings = gcontrol.OwnedMonitors;

            for (int count = 0; count <= groupsettings.Count - 1; count++)
            {
                if(currentActiveGroup != count && groupsettings[count].isActive)
                foreach(string str in tmpstrings)
                    foreach (string str2 in ownedmonitors[count])
                    {
                        if (String.Equals(str, str2, StringComparison.CurrentCulture))
                        {
                            gcontrol.isActive.Content = "Not Active";
                            groupsettings[currentActiveGroup].isActive = false;
                            string message = String.Format("{1} is active in {0}", groupsettings[count].groupName, str2);
                            Winforms.MessageBox.Show(message,"Active Error",Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Error);
                        }
                    }
            }
                    
        }

        public void saveGroupSettings()
        {
            groupsettings[currentActiveGroup] = gcontrol.groupSetting;
            ownedmonitors[currentActiveGroup] = gcontrol.OwnedMonitors;
            OnChanged(EventArgs.Empty);
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
