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

            if (ScreenSaver.SelectedIndex == 0)
                monitor.setMonitorInfo("3D Maze");
            else if (ScreenSaver.SelectedIndex == 1)
                monitor.setMonitorInfo("Slideshow");
            monitor.MouseDoubleClick += clicked;

            addnewMonitor(monitor);
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

            if(
        }



    }
}
