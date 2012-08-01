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
    /// Interaction logic for GroupControl.xaml
    /// </summary>
    public partial class GroupControl : UserControl
    {
        private MazeConfig mazeConfig;
        private GroupSetting gSetting = new GroupSetting();
        private SlideShowConfig slideshowConfig;
        private int previoussetting = 0; 

        
        List<string> availabeString = new List<string>();
        List<string> OwnedScreens = new List<string>();

        public bool donothing = false;

        public GroupSetting groupSetting
        {
            get { return gSetting; }
            set { gSetting = value; }
        }

        public List<string> OwnedMonitors
        {
            get { return OwnedScreens; }
            set { OwnedScreens = value; }
        }

        public List<string> AvailableMonitors
        {
            get { return availabeString; }
            set { availabeString = value; }
        }

        public GroupControl()
        {
            slideshowConfig = new SlideShowConfig(gSetting);
            mazeConfig = new MazeConfig(gSetting);
            InitializeComponent();
        }

        private void Load_Page(object sender, RoutedEventArgs e)
        {
            nametxtbox.Text = gSetting.groupName;
            if (String.Equals(gSetting.ssType,"SlideShow", StringComparison.InvariantCultureIgnoreCase))
                comboScreenSaver.SelectedIndex = 0;
            else if (String.Equals(gSetting.ssType, "Maze", StringComparison.InvariantCultureIgnoreCase))
                comboScreenSaver.SelectedIndex = 1;

            if (gSetting.isActive)
                isActive.Content = "Active";
            else
                isActive.Content = "Not Active";
           
            int tmpCount = 0;
            foreach (String str in availabeString)
            {
                MonitorTab tab = new MonitorTab();
                tab.title = str;
                tab.order = tmpCount;
                tab.Height = 20;
                tab.MouseDown += clicked_AvailableGroupBox;
                PendingScreens.Children.Add(tab);
                tmpCount++;
            }
            tmpCount = 1;
            foreach (String str in OwnedScreens)
            {
                MonitorTab tab = new MonitorTab();
                tab.title = str;
                tab.order = tmpCount;
                tab.Height = 20;
                tab.MouseDown += clicked_AvailableGroupBox;
                abductedScreens.Children.Add(tab);

                ComboBoxItem tmpItem = new ComboBoxItem();
                tmpItem.Content = tab.title;
                tmpItem.Name = "N" + tmpCount;
                combomonitorSelection.Items.Add(tmpItem);

                tmpCount++;
            }
        }

        private void clicked_AvailableGroupBox(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            StackPanel parent = (StackPanel)tab.Parent;
            string tmpName = parent.Name;
            Debug.WriteLine(tmpName);
            if (tmpName.Equals("PendingScreens", StringComparison.OrdinalIgnoreCase))
            {
                //Display Changes
                PendingScreens.Children.Remove(tab);
                availabeString.Remove(tab.title);
                OwnedScreens.Add(tab.title);
                abductedScreens.Children.Add(tab);

                //Adding the monitor settings
                gSetting.monitors.Add(new MonitorSetting(System.Windows.Forms.Screen.AllScreens[int.Parse(tab.title.Substring(8,1))-1].DeviceName));

                ComboBoxItem tmpItem = new ComboBoxItem();
                tmpItem.Content = tab.title;
                tmpItem.Name = "N" + gSetting.monitors.Count.ToString();
                combomonitorSelection.Items.Add(tmpItem);
            }
            else if (tmpName.Equals("abductedScreens", StringComparison.OrdinalIgnoreCase))
            {
                //Diplay Changes
                ComboBoxItem tmpitem = new ComboBoxItem();

                abductedScreens.Children.Remove(tab);
                OwnedScreens.Remove(tab.title);
                availabeString.Add(tab.title);
                PendingScreens.Children.Add(tab);

                foreach (ComboBoxItem item in combomonitorSelection.Items)
                {
                    if (item.Content == tab.title)
                    {
                        tmpitem = item;
                        break;
                    }
                }
                combomonitorSelection.Items.Remove(tmpitem);
                
                if (combomonitorSelection.SelectedValue != null )
                {
                    int tmpStr = Int32.Parse((combomonitorSelection.SelectedValue as ComboBoxItem).Name[1].ToString()) - 1;
                    gSetting.monitors.RemoveAt(tmpStr);
                }

            }

        }


        private void combomonitorSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (comboScreenSaver.SelectedValue!= null && combomonitorSelection.SelectedValue != null)
            {
               int tmpStr = Int32.Parse((combomonitorSelection.SelectedValue as ComboBoxItem).Name[1].ToString()) - 1;

                   if (comboScreenSaver.SelectedIndex == 0 && donothing)
                   {
                       //Is ScreenSaver
                       slideshowConfig.getFormInfo(gSetting, previoussetting);
                   }
                   else if (comboScreenSaver.SelectedIndex == 1 && donothing)
                   {
                       mazeConfig.getPage(gSetting, previoussetting);
                   }

                   panelMonitorSettings.Children.Clear();
                   if (comboScreenSaver.SelectedIndex == 0)
                       panelMonitorSettings.Children.Add(slideshowConfig);
                   else if (comboScreenSaver.SelectedIndex == 1)
                       panelMonitorSettings.Children.Add(mazeConfig);
                  previoussetting = tmpStr;

                  if (comboScreenSaver.SelectedIndex == 0)
                  {
                      slideshowConfig.fillForm(gSetting, tmpStr);
                  }
                  else if (comboScreenSaver.SelectedIndex == 1)
                  {
                      mazeConfig.setGroupSettings(gSetting, tmpStr);
                  }
                  donothing = true;
            }

        }

        public void nametxtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            groupSetting.groupName = nametxtbox.Text;
        }

        private void comboScreenSaver_DropDownClosed(object sender, EventArgs e)
        {
            groupSetting.ssType = (sender as ComboBox).Text;
            if (combomonitorSelection.SelectedValue != null)
            {
                panelMonitorSettings.Children.Clear();
                if (comboScreenSaver.SelectedIndex == 0)
                    panelMonitorSettings.Children.Add(slideshowConfig);
                else if (comboScreenSaver.SelectedIndex == 1)
                    panelMonitorSettings.Children.Add(mazeConfig);
            }

        }

        public void isActive_Click(object sender, RoutedEventArgs e)
        {
            if (abductedScreens.Children.Capacity == 0)
                return;

            if (String.Equals(isActive.Content.ToString(), "Not Active", StringComparison.InvariantCultureIgnoreCase))
            {
                isActive.Content = "Active";
                gSetting.isActive = true;
            }
            else
            {
                isActive.Content = "Not Active";
                gSetting.isActive = false;
            } 
        }
    }
}
