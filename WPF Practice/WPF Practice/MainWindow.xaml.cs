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
using WPF_Practice.MonitorControls;

namespace WPF_Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MonitorTab activeGroup;
        private int currentscreen = 0;

        
        List<SlideShowInfo> slideShowList = new List<SlideShowInfo>();
        List<Group> listofGroups = new List<Group>();
        List<string> unassignedMonitors = new List<string>();

        ScreenSaverControl screenPage = new ScreenSaverControl();
        GroupControl gControl = new GroupControl();
        MazeConfig mazeConfig = new MazeConfig();
        SlideShowConfig slideConfig = new SlideShowConfig();

        bool isgroupAdded = false;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        public void form_Loaded(object sender, RoutedEventArgs e)
        {
            unassignedMonitors.Add("Dynex 19\" Monitor");
            unassignedMonitors.Add("Acer 23\" Monitor");
            unassignedMonitors.Add("Projector");
        }

        private void Create_Button_Clicked(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();
            monitor.Width = 383;
            monitor.Height = 30;
            monitor.MouseDoubleClick += Monitor_clicked;
            monitor.order = listofGroups.Count;
            addnewMonitor(monitor);
        }

        public void addnewMonitor(MonitorTab monitor)
        {
            listofGroups.Add(new Group(listofGroups.Count + 1, slideShowList.Count + 1));
            slideShowList.Add(new SlideShowInfo(slideShowList.Count + 1));
            listofGroups[listofGroups.Count - 1].name = "Unnamed";
            monitor.setMonitorInfo(ref listofGroups[listofGroups.Count-1].name);
            monitor.MinWidth = MonitorMenu.MinWidth;
            monitor.MaxWidth = MonitorMenu.MaxWidth;
            MonitorMenu.Children.Add(monitor);
        }

        public void Monitor_clicked(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            activeGroup = tab;
            foreach(MonitorTab mt in MonitorMenu.Children)
            {
                mt.Background = null;
            }
                tab.Background = Brushes.Blue;
            
            if (isgroupAdded)
            {
                listofGroups[currentscreen].name = gControl.Name;
                ConfigPage.Children.RemoveAt(0);
            }
            tab.Name = listofGroups[currentscreen].name;
            gControl = new GroupControl();
            ConfigPage.Children.Remove(gControl);
            ConfigPage.Children.Remove(slideConfig);
            gControl.Name = listofGroups[tab.order].name;
            gControl.AssignOwnedStrings(ref listofGroups[tab.order].ownedMonitors);
            gControl.AssignAvailableString(ref unassignedMonitors);
            ConfigPage.Children.Add(gControl);
            isgroupAdded = true;
            ScreenSaverButton.Content = "Screen Saver";
            currentscreen = tab.order;
        }

        private void ScreenSaver_Click(object sender, RoutedEventArgs e)
        {

            if (isgroupAdded)
            {
                listofGroups[currentscreen].name = gControl.Name;
                ConfigPage.Children.RemoveAt(0);
                ConfigPage.Children.Add(slideConfig);
                ScreenSaverButton.Content = "Monitor Groups";
                isgroupAdded = false;
            }
            else
            {
               // MonitorTab tab =  MonitorMenu.Children.
                gControl = new GroupControl();
                gControl.Name = listofGroups[currentscreen].name;
                ConfigPage.Children.RemoveAt(0);
                gControl.AssignOwnedStrings(ref listofGroups[currentscreen].ownedMonitors);
                gControl.AssignAvailableString(ref unassignedMonitors);
                ConfigPage.Children.Add(gControl);
                isgroupAdded = true;
                ScreenSaverButton.Content = "Screen Saver";
                
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MonitorMenu.Children.Remove(activeGroup);
        }

    }
}
