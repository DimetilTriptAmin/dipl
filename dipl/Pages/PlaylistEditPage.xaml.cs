using dipl.Models;
using dipl.ViewModels;
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
    /// Логика взаимодействия для PlaylistEditPage.xaml
    /// </summary>
    public partial class PlaylistEditPage : Page
    {
        public PlaylistEditPage(ref Playlist playlistToEdit)
        {
            InitializeComponent();
            DataContext = new PlaylistEditViewModel(ref playlistToEdit);
        }
    }
}
