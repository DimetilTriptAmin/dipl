using dipl.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace dipl.ViewModels
{
    class AuthViewModel : ViewModelBase
    {
        private readonly Page SignIn;
        private readonly Page SignUp;

        private Page _currentPage;
        public Page CurrentPage
        {
            get
            {
                return new SignUpPage();
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }
    }
}
