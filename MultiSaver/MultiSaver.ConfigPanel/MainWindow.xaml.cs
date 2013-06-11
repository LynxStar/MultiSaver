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
using System.Windows.Forms;
using System.IO;
using MultiSaver.ConfigData;
using System.Xml;

namespace MultiSaver.ConfigPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Screen[] Screens;
        BitmapImage MonitorIconImage = new BitmapImage();

        Point StartPoint;

        Settings ConfigSettings = new Settings();

        int PreviousID = -1;

        public MainWindow()
        {

            InitializeComponent();

            SetupImage();
            SetupMonitors();

            ConfigSettings.Load();

            UnassignedStackPanel.Children.Clear();

            List<int> UsedIDs = new List<int>();

            for (int i = 0; i < ConfigSettings.Unassigned.Count; i++)
            {

                if (ScreenExists(ConfigSettings.Unassigned[i].IDNumber))
                {

                    UsedIDs.Add(ConfigSettings.Unassigned[i].IDNumber);
                    CreateIcon(UnassignedStackPanel, GetScreen(ConfigSettings.Unassigned[i].IDNumber));

                }

                else
                {

                    ConfigSettings.Unassigned.RemoveAt(i);
                    i--;

                }

            }

            for (int i = 0; i < ConfigSettings.Maze.Count; i++)
            {

                if (ScreenExists(ConfigSettings.Maze[i].IDNumber))
                {

                    UsedIDs.Add(ConfigSettings.Maze[i].IDNumber);
                    CreateIcon(MazeStackPanel, GetScreen(ConfigSettings.Maze[i].IDNumber));

                }

                else
                {

                    ConfigSettings.Maze.RemoveAt(i);
                    i--;

                }

            }

            for (int i = 0; i < ConfigSettings.Slideshow.Count; i++)
            {

                if (ScreenExists(ConfigSettings.Slideshow[i].IDNumber))
                {

                    UsedIDs.Add(ConfigSettings.Slideshow[i].IDNumber);
                    CreateIcon(SlideshowStackPanel, GetScreen(ConfigSettings.Slideshow[i].IDNumber));

                }
                else
                {

                    ConfigSettings.Slideshow.RemoveAt(i);
                    i--;

                }

            }

            foreach (int i in UsedIDs)
            {

                foreach (Screen S in Screens)
                {

                    if (i.ToString() != S.DeviceName.Replace("\\\\.\\DISPLAY", ""))
                    {

                        CreateIcon(UnassignedStackPanel, S); 
                        Monitor M = new Monitor { ID = S.DeviceName, Bounds = S.Bounds };
                        ConfigSettings.Unassigned.Add(M);

                    }

                }

            }

            ShowOptions();

        }

        public bool ScreenExists(int ID)
        {

            foreach (Screen S in Screens)
            {

                if (S.DeviceName.Replace("\\\\.\\DISPLAY", "") == ID.ToString())
                    return true;

            }

            return false;

        }

        public Screen GetScreen(int ID)
        {

            foreach (Screen S in Screens)
            {

                if (S.DeviceName.Replace("\\\\.\\DISPLAY", "") == ID.ToString())
                    return S;

            }

            return null;

        }

        private void SetupImage()
        {

            MonitorIconImage.BeginInit();
            MonitorIconImage.UriSource = new Uri("pack://application:,,,/MultiSaver.ConfigPanel;component/Images/MonitorIcon.png");
            MonitorIconImage.EndInit();

        }

        private void SetupMonitors()
        {

            Screens = Screen.AllScreens;

            foreach (Screen S in Screens)
            {

                #region MonitorIcons

                CreateIcon(UnassignedStackPanel, S);

                #endregion

                OptionComboBox.Items.Add(S.DeviceName.Replace("\\\\.\\", "").Replace("Y", "Y "));
                OptionComboBox.SelectionChanged += new SelectionChangedEventHandler(OptionComboBox_SelectionChanged);
                OptionComboBox.SelectedIndex = 0;

                Monitor M = new Monitor { ID = S.DeviceName, Bounds = S.Bounds };

                ConfigSettings.Unassigned.Add(M);

            }

        }

        private void CreateIcon(StackPanel OnPanel, Screen S)
        {

            Grid G = new Grid();
            G.Margin = new Thickness(0, 25, 0, 25);
            G.Style = FindResource("MonitorGrid") as Style;

            Image I = new Image();
            I.Height = 128;
            I.Width = 128;
            I.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            I.Name = "MonitorImage" + S.DeviceName.Replace("\\\\.\\DISPLAY", "");
            I.Stretch = Stretch.Fill;
            I.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            I.Source = MonitorIconImage;
            I.AllowDrop = false;

            G.PreviewMouseLeftButtonDown += MonitorIcon_PreviewMouseLeftButtonDown;
            G.PreviewMouseMove += MonitorIcon_PreviewMouseMove;

            TextBlock TB = new TextBlock();
            TB.Text = S.DeviceName;
            TB.Style = FindResource("MonitorIconText") as Style;
            TB.AllowDrop = false;

            G.Children.Add(I);
            G.Children.Add(TB);

            OnPanel.Children.Add(G);

        }

        void OptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (PreviousID != -1)
            {

                if (SyncCheckBox.IsChecked.Value)
                {

                    SyncOptions(PreviousID);

                }

                else
                    SaveOptions(PreviousID);

            }

            ShowOptions();

        }

        private void SaveOptions(int ID)
        {

            bool IsSlideShow = false;

            foreach (Monitor Mon in ConfigSettings.Slideshow)
            {

                if (Mon.IDNumber == ID)
                    IsSlideShow = true;

            }

            if (!IsSlideShow)
                return;

            Monitor M = ConfigSettings.FindMonitor(ID);

            M.TransitionMode = (SlideShowMode.SelectedItem as ComboBoxItem).Content.ToString();
            M.Source = ImageSourceBox.Text;
            M.Order = AlphaOrder.IsChecked.Value ? "Alpha" : "Random";
            
            M.TileType = FixedTiles.IsChecked.Value ? "Fixed" : "Random";
            M.FixedTiles = Convert.ToInt32(FixedTileCount.Text);
            M.MinTiles = Convert.ToInt32(MinRangeTileCount.Text);
            M.MaxTiles = Convert.ToInt32(MaxRangeTileCount.Text);

            M.TransitionTime = FixedTime.IsChecked.Value ? "Fixed" : "Random";
            M.FixedTime = Convert.ToInt32(FixedTimeCount.Text);
            M.MinTime = Convert.ToInt32(MinRangeTimeCount.Text);
            M.MaxTime = Convert.ToInt32(MaxRangeTimeCount.Text);

            M.IsSynced = SyncCheckBox.IsChecked.Value;

        }

        private void SyncOptions(int ID)
        {

            foreach (Monitor M in ConfigSettings.Slideshow)
                SaveOptions(Convert.ToInt32(M.ID.Replace("\\\\.\\DISPLAY", "")));

        }

        private void LoadOptions(int ID)
        {

            Monitor M = ConfigSettings.FindMonitor(ID);

            if (M.TransitionMode == "Random")
                SlideShowMode.SelectedIndex = 0;
            else if (M.Mode == "Fade")
                SlideShowMode.SelectedIndex = 1;
            else if (M.Mode == "Pan")
                SlideShowMode.SelectedIndex = 2;
            else if (M.Mode == "Spiral")
                SlideShowMode.SelectedIndex = 3;

            ImageSourceBox.Text = M.Source;

            if (M.Order == "Alpha")
                AlphaOrder.IsChecked = true;
            else
                RandomOrder.IsChecked = true;

            if (M.TileType == "Fixed")
                FixedTiles.IsChecked = true;
            else
                RangeTiles.IsChecked = true;

            FixedTileCount.Text = Convert.ToString(M.FixedTiles);
            MinRangeTileCount.Text = Convert.ToString(M.MinTiles);
            MaxRangeTileCount.Text = Convert.ToString(M.MaxTiles);

            if (M.TransitionTime == "Fixed")
                FixedTime.IsChecked = true;
            else
                RangeTime.IsChecked = true;

            FixedTimeCount.Text = Convert.ToString(M.FixedTime);
            MinRangeTimeCount.Text = Convert.ToString(M.MinTime);
            MaxRangeTimeCount.Text = Convert.ToString(M.MaxTime);

            SyncCheckBox.IsChecked = M.IsSynced;

        }

        private void ShowOptions()
        {

            //Get the monitor number.
            int ID = Convert.ToInt32(OptionComboBox.SelectedItem.ToString().Replace("DISPLAY ", ""));
            PreviousID = ID;

            //Find out what mode it is assigned to
            String Mode = "Unassigned";

            for (int i = 0; i < MazeStackPanel.Children.Count; i++)
            {

                if (((MazeStackPanel.Children[i] as Grid).Children[1] as TextBlock).Text == "\\\\.\\DISPLAY" + ID)
                {

                    Mode = "Maze";
                    break;

                }

            }

            if (Mode == "Unassigned")
            {

                for (int i = 0; i < SlideshowStackPanel.Children.Count; i++)
                {

                    if (((SlideshowStackPanel.Children[i] as Grid).Children[1] as TextBlock).Text == "\\\\.\\DISPLAY" + ID)
                    {

                        Mode = "Slideshow";
                        break;

                    }

                }

            }

            switch (Mode)
            {

                case "Unassigned":

                    UnassignedLabel.Visibility = System.Windows.Visibility.Visible;
                    MazeLabel.Visibility = System.Windows.Visibility.Collapsed;
                    OptionsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                    break;

                case "Maze":

                    UnassignedLabel.Visibility = System.Windows.Visibility.Collapsed;
                    MazeLabel.Visibility = System.Windows.Visibility.Visible;
                    OptionsStackPanel.Visibility = System.Windows.Visibility.Collapsed;
                    break;

                case "Slideshow":

                    UnassignedLabel.Visibility = System.Windows.Visibility.Collapsed;
                    MazeLabel.Visibility = System.Windows.Visibility.Collapsed;
                    OptionsStackPanel.Visibility = System.Windows.Visibility.Visible;
                    LoadOptions(ID);
                    break;

            }

        }

        private void MonitorIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            StartPoint = e.GetPosition(null);

        }

        private void MonitorIcon_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            Point MousePosition = e.GetPosition(null);
            Vector Difference = StartPoint - MousePosition;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(Difference.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(Difference.Y) > SystemParameters.MinimumVerticalDragDistance))
            {

                System.Windows.DataObject DragData = new System.Windows.DataObject("MonitorIcon", sender);
                DragDrop.DoDragDrop((sender as Grid).Parent, DragData, System.Windows.DragDropEffects.Move);

            }

        }

        private void StackPanel_DragEnter(object sender, System.Windows.DragEventArgs e)
        {

            StackPanel SP = (StackPanel)sender;

            if (!e.Data.GetDataPresent("MonitorIcon") || sender == e.Source)
                e.Effects = System.Windows.DragDropEffects.None;

            if (SP.Name == "MazeStackPanel")
            {

                if (SP.Children.Count > 0)
                    e.Effects = System.Windows.DragDropEffects.None;

            }

        }

        private void StackPanel_Drop(object sender, System.Windows.DragEventArgs e)
        {

            if (e.Data.GetDataPresent("MonitorIcon"))
            {

                Grid MovedElement = (Grid)e.Data.GetData("MonitorIcon");

                StackPanel SourcePanel = (StackPanel)MovedElement.Parent;
                StackPanel TargetPanel = (sender as StackPanel);

                if (!(TargetPanel.Name == "MazeStackPanel" && (sender as StackPanel).Children.Count > 0))
                {

                    SourcePanel.Children.Remove(MovedElement);
                    (sender as StackPanel).Children.Add(MovedElement);

                    String Name = (MovedElement.Children[1] as TextBlock).Text;

                    if (TargetPanel.Name == "UnassignedStackPanel")
                        ConfigSettings.SetMonitorMode(Name, "Unassigned");
                    else if (TargetPanel.Name == "MazeStackPanel")
                        ConfigSettings.SetMonitorMode(Name, "Maze");
                    else
                        ConfigSettings.SetMonitorMode(Name, "Slideshow");

                    int Showing = Convert.ToInt32(OptionComboBox.SelectedItem.ToString().Replace("DISPLAY ", ""));
                    int Moved = Convert.ToInt32(Name.Replace("\\\\.\\DISPLAY", ""));

                    if (Showing == Moved)
                        ShowOptions();

                }

            }

        }

        private void FixedTiles_Checked(object sender, RoutedEventArgs e)
        {

            TileFixedStackPanel.Visibility = System.Windows.Visibility.Visible;
            TileRangeStackPanel.Visibility = System.Windows.Visibility.Collapsed;

        }

        private void RangeTiles_Checked(object sender, RoutedEventArgs e)
        {

            TileFixedStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            TileRangeStackPanel.Visibility = System.Windows.Visibility.Visible;

        }

        private void TileCount_TextChanged(object sender, TextChangedEventArgs e)
        {

            System.Windows.Controls.TextBox TB = (System.Windows.Controls.TextBox)sender;

            int result;

            if (!Int32.TryParse(TB.Text, out result))
                TB.Text = "10";

            if (Convert.ToInt32(TB.Text) < 1)
                TB.Text = "1";

            if (Convert.ToInt32(TB.Text) > 100)
                TB.Text = "100";

        }

        private void FixedTime_Checked(object sender, RoutedEventArgs e)
        {

            TimeFixedStackPanel.Visibility = System.Windows.Visibility.Visible;
            TimeRangeStackPanel.Visibility = System.Windows.Visibility.Collapsed;

        }

        private void RangeTime_Checked(object sender, RoutedEventArgs e)
        {

            TimeFixedStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            TimeRangeStackPanel.Visibility = System.Windows.Visibility.Visible;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            this.Close();

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (PreviousID != -1)
            {

                if (SyncCheckBox.IsChecked.Value)
                {

                    SyncOptions(PreviousID);

                }

                else
                    SaveOptions(PreviousID);

            }

            ConfigSettings.Save();

            this.Close();

        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                ImageSourceBox.Text = dialog.SelectedPath;

            int i = 0;

            foreach (String Picture in Directory.GetFiles(dialog.SelectedPath))
            {

                if (Picture.Contains(".png") || Picture.Contains(".PNG") || Picture.Contains(".jpg") || Picture.Contains(".JPG"))
                    i++;

            }

            ImageCountLabel.Content = i;

        }
    
    }

}
