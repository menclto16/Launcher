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
        CurrentDir CurrentDirObj = new CurrentDir();

        public MainWindow()
        {
            InitializeComponent();

            updateLayout();
        }

        private void updateLayout()
        {
            dirs.Items.Clear();

            foreach (var dir in CurrentDirObj.Directories)
            {
                ListBoxItem dirItem = new ListBoxItem();

                dirItem.Content = dir;
                dirs.Items.Add(dirItem);
            }
        }

        private void OnSelected(object sender, SelectionChangedEventArgs args)
        {
            if (dirs.SelectedItem != null)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
                string path = lbi.Content.ToString();

                CurrentDirObj.ChangeDir(path);

                updateLayout();

                dirs.SelectedIndex = -1;
            }
        }
    }
}
