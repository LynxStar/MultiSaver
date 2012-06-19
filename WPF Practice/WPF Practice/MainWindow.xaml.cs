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

        public int selectedelement = 0;
        ScreenSaverControl screenPage = new ScreenSaverControl();

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();
            monitor.Width = 383;
            monitor.Height = 30;
            monitor.MouseDoubleClick += clicked;
            addnewMonitor(monitor);

            ConfigPage.Children.Add(screenPage);
        }

        public void addnewMonitor(MonitorTab monitor)
        {
            MonitorMenu.Children.Add(monitor);
         
        }

        public void clicked(object sender, EventArgs args)
        {
            MonitorTab tab = (MonitorTab)sender;
            if (Background == Brushes.Blue)
                tab.Background = null;
            else
                tab.Background = Brushes.Blue;

           
        }



    }
}
