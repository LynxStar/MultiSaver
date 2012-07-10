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
        enum ActiveForm
        {
            SlideShowForm,
            MazeForm,
            GroupControlForm
        };

       private SlideShowConfig slideshowConfig = new SlideShowConfig();
       private MazeConfig mazeConfig = new MazeConfig();
       private GroupControl gcontrol = new GroupControl();
       private ActiveForm activeForm;
        
        //Common Classes holds the classes in which we transfer things from the form to the lists
       private List<SlideShowInfo> slideShowList = new List<SlideShowInfo>();
       private List<MazeInfo> mazeinfoList = new List<MazeInfo>();
       private List<Group> listofGroups = new List<Group>();

       private List<string> unassignedMonitors = new List<string>();
       private int currentActiveGroup;

        public ScreenSaverControl()
        {
            InitializeComponent();
        }


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            unassignedMonitors.Add("Dynex 19\" Monitor");
            unassignedMonitors.Add("Acer 23\" Monitor");
            unassignedMonitors.Add("Projector");
        }

        public int createnewGroup()
        {
            int groupid = listofGroups.Count + 1;
            listofGroups.Add(new Group(groupid));
            listofGroups[listofGroups.Count - 1].name = "Unnamed";
            return listofGroups.Count - 1;
        }

        public void createNewSlideShow(SlideShowInfo info)
        {
            
        }

        public string getCurrentGroupName()
        {
            return gcontrol.Name;
        }

        public void reset()
        {
            if (mainPanel.Children.Count != 0)
                mainPanel.Children.RemoveAt(0);
        }

        public int getTotalNumberofGroups()
        {
            return listofGroups.Count;
        }

        public void displayGroupControl(int selectedScreen)
        {
            reset();
            gcontrol = new GroupControl();
            gcontrol.Name = listofGroups[selectedScreen].name;
            gcontrol.AssignOwnedStrings(ref listofGroups[selectedScreen].ownedMonitors);
            gcontrol.AssignAvailableString(ref unassignedMonitors);
            mainPanel.Children.Add(gcontrol);
            activeForm = ActiveForm.GroupControlForm;
        }

        public void displaySlideShowControl(int selectedScreen)
        {
            reset(); 
            
            slideshowConfig = new SlideShowConfig();
            slideshowConfig.FadeTime = slideShowList[selectedScreen].FadeTime;
            slideshowConfig.DisplayTime = slideShowList[selectedScreen].DisplayTime;
            slideshowConfig.PanTime = slideShowList[selectedScreen].PanTime;
            slideshowConfig.Rotation = slideShowList[selectedScreen].Rotation;
            slideshowConfig.Clockwise = slideShowList[selectedScreen].Clockwise;
            slideshowConfig.Alphabetical = slideShowList[selectedScreen].Alphabetical;
            slideshowConfig.RevAlphebetical = slideShowList[selectedScreen].RevAlphabetical;
            slideshowConfig.Random = slideShowList[selectedScreen].Random;
            slideshowConfig.DirectionInChar = slideShowList[selectedScreen].dIn;
            slideshowConfig.DirectionOutChar = slideShowList[selectedScreen].dout;

            mainPanel.Children.Add(slideshowConfig);

            activeForm = ActiveForm.SlideShowForm;
        }

        public void savecurrentSettings(int selectedScreen)
        {
            if (activeForm == ActiveForm.SlideShowForm)
            {
                slideShowList[selectedScreen].dout = slideshowConfig.DirectionOutChar;
                slideShowList[selectedScreen].dIn = slideshowConfig.DirectionInChar;
                slideShowList[selectedScreen].FadeTime = slideshowConfig.FadeTime;
                slideShowList[selectedScreen].DisplayTime = slideshowConfig.DisplayTime;
                slideShowList[selectedScreen].PanTime = slideshowConfig.PanTime;
                slideShowList[selectedScreen].Rotation = slideshowConfig.Rotation;
                slideShowList[selectedScreen].Clockwise = slideshowConfig.Clockwise;
                slideShowList[selectedScreen].Alphabetical = slideshowConfig.Alphabetical;
                slideShowList[selectedScreen].RevAlphabetical = slideshowConfig.RevAlphebetical;
                slideShowList[selectedScreen].Random = slideshowConfig.Random;
            }
            else if (activeForm == ActiveForm.MazeForm)
            {
                //TODO finish maze form
            }
            else if (activeForm == ActiveForm.GroupControlForm)
            {
                listofGroups[selectedScreen].name = gcontrol.Name;
            }
        }

        public void deleteGroup(int selectedScreen)
        {
            foreach (string monitor in listofGroups[selectedScreen].ownedMonitors)
                unassignedMonitors.Add(monitor);

            slideShowList.RemoveAt(selectedScreen);
            listofGroups.RemoveAt(selectedScreen);
        }

    }
}
