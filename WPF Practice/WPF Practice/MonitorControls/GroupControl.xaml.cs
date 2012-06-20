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
                tab.MouseDoubleClick += clicked_OwnedGroupBox;
                abductedScreens.Children.Add(tab);
                tmpCount++;
            }
        }

        private void clicked_AvailableGroupBox(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            MonitorTab tab2 = tab;
            PendingScreens.Children.RemoveAt(tab.order);
            availabeString.RemoveAt(tab.order);
            OwnedScreens.Add(tab.getMonitorInfo());
            abductedScreens.Children.Add(tab2);
        }

        private void clicked_OwnedGroupBox(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            MonitorTab tab2 = tab;
            abductedScreens.Children.RemoveAt(tab.order);
            OwnedScreens.RemoveAt(tab.order);
            availabeString.Add(tab.getMonitorInfo());
            PendingScreens.Children.Add(tab2);
        }

        public void fillBox(Group targetGroup)
        {
            nametxtbox.Text = targetGroup.name;
        }

    }
}
