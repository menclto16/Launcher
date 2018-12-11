﻿using System;
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
        CurrentDir CurrentDirObj = new CurrentDir();

        public MainWindow()
        {
            InitializeComponent();
            Dirs.DataContext = CurrentDirObj;
        }

        private void OnSelected(object sender, SelectionChangedEventArgs args)
        {
            if (Dirs.SelectedIndex != -1)
            {
                int selected = Dirs.SelectedIndex; 
                string path = CurrentDirObj.Directories[selected];

                if (!CurrentDirObj.ChangeDir(path)) showDialog();

                CurrentPath.Content = CurrentDirObj.Path;

                Dirs.SelectedIndex = -1;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (CurrentDirObj.Back())
            {
                CurrentPath.Content = CurrentDirObj.Path;
            }
            else
            {
                CurrentPath.Content = @"\";
            }
        }

        async private void showDialog()
        {
            await this.ShowMessageAsync("Error", "You don't have permissions to access the directory");
        }

        private void ConfirmPath(object sender, RoutedEventArgs e)
        {
            CurrentDirObj.GetExecutables();
        }
    }
}
