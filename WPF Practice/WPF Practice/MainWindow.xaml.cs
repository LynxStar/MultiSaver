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
using System.Diagnostics;
using System.Windows.Shapes;
using WPF_Practice.MonitorControls;

namespace WPF_Practice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ScreenSaverControl configScreensSaverControl = new ScreenSaverControl();
        private int currentScreen = -1;

        bool firstAppear = true;

        public MainWindow()
        {
            InitializeComponent();

                load("");
        }

        public void form_Loaded(object sender, RoutedEventArgs e)
        {
            mainControl.Children.Add(configScreensSaverControl);
        }

        private void Create_Button_Clicked(object sender, RoutedEventArgs e)
        {
            MonitorTab monitor = new MonitorTab();

            monitor.Width = 383;
            monitor.Height = 30;
            monitor.MinWidth = MonitorMenu.MinWidth;
            monitor.MaxWidth = MonitorMenu.MaxWidth;
            monitor.MouseDown += Monitor_clicked;
            monitor.order = configScreensSaverControl.createnewGroup();
            currentScreen = configScreensSaverControl.getTotalNumberofGroups() - 1;
            monitor.passtitleRef(ref configScreensSaverControl.getGroupSettings(monitor.order).groupName);
            MonitorMenu.Children.Add(monitor);
        }

        private void Monitor_clicked(object sender, EventArgs e)
        {
            MonitorTab tab = (MonitorTab)sender;
            MonitorTab prevtab = (MonitorTab)MonitorMenu.Children[currentScreen];
            prevtab.title = configScreensSaverControl.getGroupSettings()[currentScreen].groupName;
            //resetting the color
            foreach(MonitorTab mt in MonitorMenu.Children)
                mt.Background = null;
            currentScreen = tab.order;
            tab.Background = Brushes.DarkTurquoise;
            if (!firstAppear)
                configScreensSaverControl.saveGroupSettings();
            else
                firstAppear = false;
            configScreensSaverControl.displayGroupControl(currentScreen);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            configScreensSaverControl.deleteGroup(currentScreen);

            for (int i = currentScreen +1;  i <= MonitorMenu.Children.Count-1 ; i++)
            {
                    MonitorTab tab = (MonitorTab)MonitorMenu.Children[i];
                    tab.order = i-1;
            }

            MonitorMenu.Children.RemoveAt(currentScreen);
            firstAppear = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Save save = new Save(configScreensSaverControl.getGroupSettings(), this);
            save.setSave();
            save.ShowDialog();
            //XMLHandler.save(configScreensSaverControl.getGroupSettings());
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            XMLHandler.save(configScreensSaverControl.getGroupSettings());
        }

        public void load(string pathname)
        {
            List<GroupSetting> loaded = new List<GroupSetting>();
            loaded = XMLHandler.load(pathname + "config.xml");
            foreach (GroupSetting gs in loaded)
            {
                MonitorTab monitor = new MonitorTab();
                configScreensSaverControl.getGroupSettings().Add(gs);
                monitor.Width = 383;
                monitor.Height = 30;
                monitor.MinWidth = MonitorMenu.MinWidth;
                monitor.MaxWidth = MonitorMenu.MaxWidth;
                monitor.MouseDown += Monitor_clicked;
                monitor.order = configScreensSaverControl.getGroupSettings().Count -1;
                currentScreen = configScreensSaverControl.getTotalNumberofGroups() - 1;
                monitor.passtitleRef(ref gs.groupName);
                MonitorMenu.Children.Add(monitor);

                configScreensSaverControl.getOwnedMonitors().Add(new List<string>());
                foreach (MonitorSetting ms in gs.monitors)
                {
                    configScreensSaverControl.getOwnedMonitors()[configScreensSaverControl.getOwnedMonitors().Count - 1].Add(ms.monitorId);
                }
            }

        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            Save load = new Save(configScreensSaverControl.getGroupSettings(), this);
            load.setLoad();
            load.ShowDialog();
        }
    }
}
