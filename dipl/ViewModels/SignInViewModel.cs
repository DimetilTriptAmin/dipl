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
    class SignInViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ICommand NavigateSignUpCommand
        {
            get
            {
                return new RelayCommand((obj) => {
                    _navigationStore.CurrentViewModel = new SignUpViewModel(_navigationStore);
                });
            }
        }

        public SignInViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
