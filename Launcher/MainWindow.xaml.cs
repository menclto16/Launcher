using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                string path = DirBrowserObj.Paths[selected];

                if (!DirBrowserObj.ChangeDir(path)) showPermissionDialog();

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

        async private void showPermissionDialog()
        {
            await this.ShowMessageAsync("Error", "Insufficient permission!");
        }

        private async void ConfirmPath(object sender, RoutedEventArgs e)
        {
            AppPanel.Children.Clear();
            AppMessage.ClearValue(TextBlock.HeightProperty);
            
            Task task = Task.Run(() => DirBrowserObj.GetExecutables());

            var controller = await this.ShowProgressAsync("Please wait...", "Searching for applications");
            controller.SetIndeterminate();
            await task;

            foreach (var executable in DirBrowserObj.Executables)
            {
                Tile tile = new Tile();
                tile.Name = "executable" + DirBrowserObj.Executables.IndexOf(executable).ToString();
                tile.Title = executable.Filename;
                tile.Click += new RoutedEventHandler(ExecutableClick);
                ImageBrush icon = new ImageBrush()
                {
                    ImageSource = executable.Icon,
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Center,
                    AlignmentY = AlignmentY.Center
                };
                Rectangle rec = new Rectangle()
                {
                    Width = 90,
                    Height = 90,
                    Fill = icon,
                };
                tile.Content = rec;
                AppPanel.Children.Add(tile);
            }

            if (AppPanel.Children.Count != 0) AppMessage.Height = 0;

            TabMenu.SelectedItem = AppTab;
            AppTab.IsSelected = true;

            await controller.CloseAsync();
        }

        private void ExecutableClick(object sender, RoutedEventArgs e)
        {
            Tile tile = (Tile)sender;
            int exeIndex = Int32.Parse(Regex.Match(tile.Name, @"\d+").Value);
            DirBrowserObj.Executables[exeIndex].RunExecutable();
        }
    }
}
