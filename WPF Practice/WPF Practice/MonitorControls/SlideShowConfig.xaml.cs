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
        public bool Alphabetical
        {
            get {return (bool)radioA.IsChecked;}
            set {radioA.IsChecked = value;}
        }
        public bool RevAlphebetical
        {
            get { return (bool)radioRevA.IsChecked; }
            set { radioRevA.IsChecked = value; }
        }
        public bool Random
        {
            get { return (bool)radioRandom.IsChecked; }
            set { radioRandom.IsChecked = value; }
        }
        public char DirectionInChar
        {
            get
            {
                string tmp = (directionInBox.SelectedItem as ComboBoxItem).Content.ToString();
                return tmp[0];
            }
            set
            {
                string tmp = value.ToString();
                setComparisions(tmp, directionInBox);
            }
        }
        public char DirectionOutChar
        {
            get
            {
                string tmp = (directionoutBox.SelectedItem as ComboBoxItem).Content.ToString();
                return tmp[0];
            }
            set
            {
                string tmp = value.ToString();
                setComparisions(tmp, directionoutBox);
            }
        }
        public SlideShowConfig()
        {
            InitializeComponent();
        }

        //function is only used to set direction txt boxes
        private void setComparisions(string tmp, ComboBox tmpCombo)
        {
            if (String.Equals(tmp, "N", StringComparison.InvariantCultureIgnoreCase))
                tmpCombo.SelectedIndex = 0;
            else if (String.Equals(tmp, "S", StringComparison.InvariantCultureIgnoreCase))
                tmpCombo.SelectedIndex = 1;
            else if (String.Equals(tmp, "W", StringComparison.InvariantCultureIgnoreCase))
                tmpCombo.SelectedIndex = 2;
            else if (String.Equals(tmp, "E", StringComparison.InvariantCultureIgnoreCase))
                tmpCombo.SelectedIndex = 3;
        }

        public void fillForm(SlideShowInfo targetGroup)
        {
            FadeTime = targetGroup.FadeTime;
            DisplayTime = targetGroup.DisplayTime;
            PanTime = targetGroup.PanTime;
            Rotation = targetGroup.Rotation;
            Clockwise = targetGroup.Clockwise;
            Alphabetical = targetGroup.Alphabetical;
            RevAlphebetical = targetGroup.RevAlphabetical;
            Random = targetGroup.Random;
        }

        public void getFormInfo(SlideShowInfo targetGroup)
        {
            targetGroup.FadeTime = FadeTime;
            targetGroup.DisplayTime = DisplayTime;
            targetGroup.PanTime = PanTime;
            targetGroup.Rotation = Rotation;
            targetGroup.Clockwise = Clockwise;
            targetGroup.Alphabetical = Alphabetical;
            targetGroup.RevAlphabetical = RevAlphebetical;
            targetGroup.Random = Random;
        }
    }
}
