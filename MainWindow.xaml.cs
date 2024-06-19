using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace ImageDataCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileManipulation fileM;
        public MainWindow()
        {
            InitializeComponent();
            fileM = new FileManipulation();
        }
        private void cleanData(object sender, RoutedEventArgs e) {
            string folder = "";
            if (checkCopy.IsChecked == true)
            {
                folder = folderText.Text;
            }
            fileM.runFiles(folder, prefixText.Text);
            filesText.Clear();
            prefixText.Clear();
            folderText.Clear();
            //MessageBox.Show("Description and comentary data deleted","Operation successful",MessageBoxButton.OK);
        }

        private void selectFolder(object sender, RoutedEventArgs e)
        {
            folderText.Text =fileM.getFolder();

        }
        private void selectFiles(object sender, RoutedEventArgs e)
        {
            List<string> fileNames=fileM.getImages();
            filesText.Text = String.Join(";", fileNames);

        }
    }
}
