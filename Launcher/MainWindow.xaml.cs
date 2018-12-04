using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady == false) continue;

                ListBoxItem driveItem = new ListBoxItem();

                driveItem.Content = drive.Name;
                drives.Items.Add(driveItem);
            }
        }

        private void OnSelected(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem lbi = new ListBoxItem();
            lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            var directories = Directory.GetDirectories(lbi.Content.ToString());

            foreach (var dir in directories)
            {
                drives.Items.Add(dir.ToString());
            }
        }
    }
}
