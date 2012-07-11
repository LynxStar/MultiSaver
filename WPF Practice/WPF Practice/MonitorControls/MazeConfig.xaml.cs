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
    /// Interaction logic for MazeConfig.xaml
    /// </summary>
    public partial class MazeConfig : UserControl
    {
        public int MazeSize
        {
            get { return comboMazeSize.SelectedIndex; }
            set { comboMazeSize.SelectedIndex = value; }
        }

        public string MazePalletName
        {
            get { return textBoxPalletN.Text; }
            set { textBoxPalletN.Text = value; }
        }

        public String MazeView
        {
            get { return (comboAIView.SelectedValue as ComboBoxItem).Content.ToString(); }
            set { (comboAIView.SelectedValue as ComboBoxItem).Content = value; }
        }

        public String SearchMethod
        {
            get { return (comboAIMethod.SelectedValue as ComboBoxItem).Content.ToString();}
            set { (comboAIMethod.SelectedValue as ComboBoxItem).Content = value; }
        }

        public MazeConfig()
        {
            InitializeComponent();
        }

        public void fillPage(GroupSetting gSettings, MonitorSetting mSettings)
        {
            this.MazeSize = gSettings.mazeSize;
            this.MazePalletName = gSettings.mazePalletName;
            this.MazeView = mSettings.aiView;
            this.SearchMethod = mSettings.aiMethod;
        }

        public void getPage(GroupSetting gSettings, int numberscreen)
        {
            gSettings.mazeSize = MazeSize;
            gSettings.mazePalletName = MazePalletName;
            gSettings.monitors[numberscreen].aiMethod = SearchMethod;
            gSettings.monitors[numberscreen].aiView = MazeView;
            gSettings.ssType = "Maze";
        }

        public void setGroupSettings(GroupSetting gSettings, int numberscreen)
        {
            SearchMethod = gSettings.monitors[numberscreen].aiMethod;
            MazeView = gSettings.monitors[numberscreen].aiView;
            MazeSize = gSettings.mazeSize;
            MazePalletName = gSettings.mazePalletName;
        } 

    }
}
