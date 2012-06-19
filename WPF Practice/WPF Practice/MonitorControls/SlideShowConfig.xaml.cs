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
    /// Interaction logic for SlideShowConfig.xaml
    /// </summary>
    public partial class SlideShowConfig : UserControl
    {
        public int FadeTime
        {
            get { return Int32.Parse(txtFadeTime.Text); } 
            set { txtFadeTime.Text = value.ToString(); }
        }
        public int DisplayTime
        {
            get{return Int32.Parse(txtDisplayTime.Text);}
            set { txtDisplayTime.Text = value.ToString(); }
        }
        public int PanTime
        {
            get {return Int32.Parse(txtPanTime.Text);}
            set { txtPanTime.Text = value.ToString(); }
        }
        public int Rotation
        {
            get{return Int32.Parse(txtRotation.Text);}
            set { txtRotation.Text = value.ToString(); }
        }
        public bool Clockwise
        {
            get { return (bool)isClockwise.IsChecked; }
            set { isClockwise.IsChecked = value; }
        }
        public bool isAlphabetical
        {
            get
            {
                if (radioA.IsChecked)
                    return true;
                else if(radioRevA)
                    return false;
                }
        }
        public SlideShowConfig()
        {
            InitializeComponent();
        }

        
    }
}
