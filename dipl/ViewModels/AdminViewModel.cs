using dipl.Models;
using dipl.Models.Data;
using dipl.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dipl.ViewModels
{
    class AdminViewModel : ViewModelBase
    {

        private int _index = 0;
        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    App.CurrentAccount = Accounts[Index];
                });
            }
        }

        public AdminViewModel()
        {
            Accounts = new ObservableCollection<Account>(DataHandler.GetAccounts());
        }
    }
}
