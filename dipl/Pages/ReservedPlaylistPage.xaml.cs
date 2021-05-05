using dipl.Models;
using dipl.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace dipl.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReservedPlaylistPage.xaml
    /// </summary>
    public partial class ReservedPlaylistPage : Page
    {
        public ReservedPlaylistPage()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Audio files |*.mp3";
            if (openFileDialog.ShowDialog() == true)
            {
                ((ReservedPlaylistViewModel)(this.DataContext)).AddCommand.Execute(openFileDialog.FileNames);
            }
        }
    }
}
