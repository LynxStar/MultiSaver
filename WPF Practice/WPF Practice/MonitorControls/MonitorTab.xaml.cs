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
    /// Interaction logic for MonitorTab.xaml
    /// </summary>
    public partial class MonitorTab : UserControl
    {
        private int place;
        public int order
        {
            get { return place; }
            set { place = value; }
        }
        public MonitorTab()
        {
            InitializeComponent();
        }

        public void setMonitorInfo(string str)
        {
            MonitorInfo.Content = str;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
        if(Background == null)
           Background = Brushes.YellowGreen;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            if(Background == Brushes.YellowGreen)
                Background = null;
        }

        public void resetBackground()
        {
            Background = null;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            border.Height = this.Height;
            border.Width = this.Width;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void setPic(string str)
        {
            //controlImage.Source = str;
        }
    }
}
