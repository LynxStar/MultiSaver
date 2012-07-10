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
        List<string> availabeString = new List<string>();
        List<string> OwnedScreens = new List<string>();

        public string Name
        {
            get { return nametxtbox.Text; }
            set { nametxtbox.Text = value; }
        }

        public GroupControl()
        {
            InitializeComponent();
        }

        public void AssignAvailableString(ref  List<string> AvailableString)
        {
            this.availabeString = AvailableString;
        }
        public void AssignOwnedStrings(ref List<string> targetStrings)
        {
            this.OwnedScreens = targetStrings;
        }

        private void Load_Page(object sender, RoutedEventArgs e)
        {
            int tmpCount = 0;
            foreach (String str in availabeString)
            {
                MonitorTab tab = new MonitorTab();
                tab.setMonitorInfo(str);
                tab.order = tmpCount;
                tab.Height = 20;
                tab.MouseDoubleClick += clicked_AvailableGroupBox;
                PendingScreens.Children.Add(tab);
                tmpCount++;
            }
            tmpCount = 0;
            foreach (String str in OwnedScreens)
            {
                MonitorTab tab = new MonitorTab();
                tab.setMonitorInfo(str);
                tab.order = tmpCount;
                tab.Height = 20;
                tab.MouseDoubleClick += clicked_AvailableGroupBox;
                abductedScreens.Children.Add(tab);
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

                PendingScreens.Children.Remove(tab);
                availabeString.Remove(tab.getMonitorInfo());
                OwnedScreens.Add(tab.getMonitorInfo());
                abductedScreens.Children.Add(tab);
            }
            else if (tmpName.Equals("abductedScreens", StringComparison.OrdinalIgnoreCase))
            {
                abductedScreens.Children.Remove(tab);
                OwnedScreens.Remove(tab.getMonitorInfo());
                availabeString.Add(tab.getMonitorInfo());
                PendingScreens.Children.Add(tab);
            }

        }

        private void clicked_OwnedGroupBox(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;


        }

        public void fillBox(Group targetGroup)
        {
            nametxtbox.Text = targetGroup.name;
        }

    }
}
