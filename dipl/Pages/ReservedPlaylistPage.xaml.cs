using dipl.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

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
