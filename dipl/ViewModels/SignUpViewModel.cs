using dipl.Stores;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class SignUpViewModel:ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ICommand NavigateSignInCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    _navigationStore.CurrentViewModel = new SignInViewModel(_navigationStore);
                });
            }
        }

        public SignUpViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
