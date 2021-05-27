using dipl.Models.Data;
using dipl.Stores;
using dipl.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace dipl.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += delegate { DragMove(); };
            DataContext = new MainViewModel(new NavigationStore());
        }

        private void MinimizeWindow_Exec(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void ProgressBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            ((MainViewModel)(this.DataContext)).ThumbDragStarted.Execute(null);
        }

        private void ProgressBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            ((MainViewModel)(this.DataContext)).ThumbDragCompleted.Execute(null);
        }
        private void ProgressBar_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            ((MainViewModel)(this.DataContext)).ThumbDragDelta.Execute(null);
        }
    }
}
