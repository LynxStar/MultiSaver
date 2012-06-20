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

        public string GroupName
        {
            get { return grouptxt.Text; }
            set { grouptxt.Text = value; }
        }

        public GroupControl()
        {
            InitializeComponent();
        }

        public void AssignAvailableString(ref List<string> AvailableString)
        {
            this.availabeString = AvailableString;
        }

        public void AssignOwnedStrings(List<string> targetStrings)
        {
            this.availabeString = targetStrings;
        }

        public void Load_Page(object sender, EventArgs e)
        {
            foreach (String str in availabeString)
            {
                MonitorTab tab = new MonitorTab();
                tab.setMonitorInfo(str);
                tab.MouseDoubleClick += clicked_AvailableGroupBox;
                PendingScreens.Children.Add(tab);
            }
            foreach (String str in OwnedScreens)
            {
                MonitorTab tab = new MonitorTab();
                tab.setMonitorInfo(str);
                tab.MouseDoubleClick += clicked_OwnedGroupBox;
                PendingScreens.Children.Add(tab);
            }
        }

        public void clicked_AvailableGroupBox(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            PendingScreens.Children.Remove(tab);
            abductedScreens.Children.Add(tab);
        }

        public void clicked_OwnedGroupBox(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            abductedScreens.Children.Remove(tab);
            PendingScreens.Children.Add(tab);
        }


    }
}
