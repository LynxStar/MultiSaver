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
        struct SlideShowInfo
        {
            int id;
            int FadeTime;
            int DisplayTime;
            int PanTime;
            int Rotation;
            bool Clockwise;
            bool Alphabetical;
            bool RevAlphabetical;
            bool Random;
        }

        struct Group
        {
            string name;
            int groupid;
            List<string> ownedMonitors;
        }

        private int currentscreen = -1;

        
        List<SlideShowInfo> slideShowList = new List<SlideShowInfo>();
        List<Group> listofGroups = new List<Group>();
        String[] unassignedMonitors = new String[3];

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
            unassignedMonitors[0] = "Dynex 19\" Monitor";
            unassignedMonitors[1] = "Acer 23\" Monitor";
            unassignedMonitors[2] = "Projector";

        }

        private void Create_Button_Clicked(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();
            monitor.Width = 383;
            monitor.Height = 30;
            monitor.MouseDoubleClick += clicked;
            addnewMonitor(monitor);
            monitor.order = listofGroups.Count-1;
        }

        public void addnewMonitor(MonitorTab monitor)
        {
            createNewData();
            monitor.MinWidth = MonitorMenu.MinWidth;
            monitor.MaxWidth = MonitorMenu.MaxWidth;
            MonitorMenu.Children.Add(monitor);
        }

        public void clicked(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            if (tab.Background == Brushes.Blue)
                tab.Background = null;
            else
                tab.Background = Brushes.Blue;

            if (!isgroupAdded)
            {
                ConfigPage.Children.Remove(gControl);
                ConfigPage.Children.Add(gControl);
                isgroupAdded = true;
                ScreenSaverButton.Content = "Screen Saver";
            }
        }

        private void ScreenSaver_Click(object sender, RoutedEventArgs e)
        {
            if (isgroupAdded)
            {
                ConfigPage.Children.RemoveAt(0);
                ConfigPage.Children.Add(slideConfig);
                ScreenSaverButton.Content = "Monitor Groups";
                isgroupAdded = false;
            }
            else
            {
                ConfigPage.Children.RemoveAt(0);
                ConfigPage.Children.Add(gControl);
                isgroupAdded = true;
                ScreenSaverButton.Content = "ScreenSaver";
                
            }
        }

        private void createNewData()
        {
            listofGroups.Add(new Group());
            slideShowList.Add(new SlideShowInfo());
            currentscreen = listofGroups.Count-1;
        }

        private void fillInGroupForm(MonitorTab tab)
        {

        }

    }
}
