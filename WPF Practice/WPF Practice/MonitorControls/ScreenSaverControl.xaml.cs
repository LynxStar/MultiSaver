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
    /// Interaction logic for ScreenSaverControl.xaml
    /// </summary>
    public partial class ScreenSaverControl : UserControl
    {
        SlideShowConfig slideshowConfig = new SlideShowConfig();
        MazeConfig mazeConfig = new MazeConfig();
        GroupControl gcontrol = new GroupControl();

        public ScreenSaverControl()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            TabItem screenSaver = new TabItem();
            screenSaver.Content = gcontrol;
            GroupConfigOptions.Items.Add(screenSaver);
            

        }

        public void createnewPanels()
        {
            TabItem screenSaver = new TabItem();
            screenSaver.Content = gcontrol;
            GroupConfigOptions.Items.Add(screenSaver);
        }
    }
}
