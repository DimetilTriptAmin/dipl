using dipl.Stores;
using dipl.ViewModels;
using dipl.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dipl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new SignInViewModel(navigationStore);

            AuthWindow authWindow = new AuthWindow()
            {
                DataContext = new AuthViewModel(navigationStore)
            };

            authWindow.Show();

            base.OnStartup(e);
        }
    }
}
