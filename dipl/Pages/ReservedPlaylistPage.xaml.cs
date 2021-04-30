﻿using dipl.Models;
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
    /// Логика взаимодействия для ReservedPlaylistPage.xaml
    /// </summary>
    public partial class ReservedPlaylistPage : Page
    {
        public ReservedPlaylistPage(Playlist pl, bool isLiked)
        {
            InitializeComponent();
            DataContext = new ReservedPlaylistViewModel(pl, isLiked);
        }
    }
}