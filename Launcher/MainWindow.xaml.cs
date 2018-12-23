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
using MahApps.Metro.Controls.Dialogs;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        DirBrowser DirBrowserObj = new DirBrowser();

        public MainWindow()
        {
            InitializeComponent();
            Dirs.DataContext = DirBrowserObj;
        }

        private void OnSelected(object sender, SelectionChangedEventArgs args)
        {
            if (Dirs.SelectedIndex != -1)
            {
                int selected = Dirs.SelectedIndex; 
                string path = DirBrowserObj.Directories[selected];

                if (!DirBrowserObj.ChangeDir(path)) showDialog();

                CurrentPath.Content = DirBrowserObj.Path;

                Dirs.SelectedIndex = -1;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (DirBrowserObj.Back())
            {
                CurrentPath.Content = DirBrowserObj.Path;
            }
            else
            {
                CurrentPath.Content = @"\";
            }
        }

        async private void showDialog()
        {
            await this.ShowMessageAsync("Error", "Insufficient permission to access the directory");
        }

        private void ConfirmPath(object sender, RoutedEventArgs e)
        {
            DirBrowserObj.GetExecutables();

            foreach (var executable in DirBrowserObj.Executables)
            {
                Tile tile = new Tile();
                tile.Title = executable.Filename;
                ImageBrush icon = new ImageBrush();
                icon.ImageSource = executable.Icon;
                icon.Stretch = Stretch.None;
                icon.AlignmentX = AlignmentX.Center;
                icon.AlignmentY = AlignmentY.Center;
                tile.Background = icon;
                AppPanel.Children.Add(tile);
            }
        }
    }
}
