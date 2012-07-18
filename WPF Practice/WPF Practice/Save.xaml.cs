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
using System.Windows.Shapes;
using subform = System.Windows.Forms;
using WPF_Practice.MonitorControls;

namespace WPF_Practice
{
    /// <summary>
    /// Interaction logic for Save.xaml
    /// </summary>
    public partial class Save : Window
    {
        List<GroupSetting> groupSettings;
        public Save(List<GroupSetting> gs)
        {
            groupSettings = gs;
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            subform.FolderBrowserDialog folderDialog = new subform.FolderBrowserDialog();
            folderDialog.SelectedPath = "C:\\";

            subform.DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
                saveLocation.Text = folderDialog.SelectedPath;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            XMLHandler.save(groupSettings, saveLocation.Text + "//");
            this.Close();
        }


    }
}
