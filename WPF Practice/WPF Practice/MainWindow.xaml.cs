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
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();
            monitor.setMonitorInfo("Test");

            MonitorTab monitor2 = new MonitorTab();
            monitor.setMonitorInfo("Test");

            addnewMonitor(monitor);
            addnewMonitor(monitor2);
        }

        public void addnewMonitor(MonitorTab monitor)
        {
            MonitorMenu.Children.Add(monitor);
         
        }


    }
}
