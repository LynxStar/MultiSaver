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
        struct SlideShowConfig
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
            int Groupid;
            List<string> ownedMonitors;
        }

        public int selectedelement = 0;
        private bool isScreenSaver = false;

        
        List<SlideShowConfig> slideShowList = new List<SlideShowConfig>();
        List<Group> listofGroups = new List<Group>();
        
        ScreenSaverControl screenPage = new ScreenSaverControl();
        GroupControl gControl = new GroupControl();
        MazeConfig mazeConfig = new MazeConfig();
        SlideShowConfig slideShowConfig = new SlideShowConfig();

        bool isgroupAdded = false;

        public MainWindow()
        {
            InitializeComponent();
            
        }

        public void form_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();
            monitor.Width = 383;
            monitor.Height = 30;
            monitor.MouseDoubleClick += clicked;
            addnewMonitor(monitor);
        }

        public void addnewMonitor(MonitorTab monitor)
        {
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
            }
        }

        public void screensaverClicked(object sender, EventArgs e)
        {
            if (isgroupAdded)
            {
                ConfigPage.Children.Remove(gControl);
                ConfigPage.Children.Add(slideShowConfig);
            }
            
        }

        private void ScreenSaver_Click(object sender, RoutedEventArgs e)
        {

        }



    }
}
