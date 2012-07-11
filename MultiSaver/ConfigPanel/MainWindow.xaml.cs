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

namespace ConfigPanel
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            List<GroupSetting> groups = new List<GroupSetting>();
            GroupSetting group = new GroupSetting();
            group.addMonitor(1);
            groups.Add(group);
            XMLHandler.save(groups);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            XMLHandler.load("config.xml");
        }
    }
}
