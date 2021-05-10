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
        public ObservableCollection<Account> Accounts;

        public ICommand RemoveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    Accounts.RemoveAt((int)obj);
                });
            }
        }

        public AdminViewModel()
        {
            Accounts = new ObservableCollection<Account>(DataHandler.GetAccounts());
        }
    }
}
