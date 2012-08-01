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
using System.Diagnostics;
using Winforms = System.Windows.Forms;


namespace WPF_Practice.MonitorControls
{
    class ScreenSaverListener
    {
        private ScreenSaverControl saver;
        private MainWindow main;

        public ScreenSaverListener(MainWindow host,ScreenSaverControl control)
        {
            saver = control;
            main = host;
            saver.changed += new ScreenSaverControl.ChangedGroupSettings(controlChanged);
        }

        public void controlChanged(object sender, EventArgs e)
        {
            
            for (int i = 0; i < main.MonitorMenu.Children.Capacity - 1; i++)
            {
                MonitorTab tab = (MonitorTab)main.MonitorMenu.Children[i];
                tab.title = String.Format("{0}   {1}\nOwned:  {2} Active:  {3}",saver.getGroupSettings(i).groupName, saver.getGroupSettings(i).ssType, 
                    saver.getGroupSettings(i).monitors.Capacity, saver.getGroupSettings(i).isActive);
                main.MonitorMenu.Children[i] = tab;
            }
        }
    }
}
