using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dipl.ViewModels
{
    public class ErrorViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        internal ResourceDictionary mergedDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(md => md.Source.OriginalString.Contains("lang"));
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public bool HasErrors => _errorsByPropertyName.Any();

        private bool _canValidate = true;
        public bool CanValidate
        {
            get => _canValidate;
            set
            {
                _canValidate = value;
                OnPropertyChanged(nameof(CanValidate));
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
        }

        public void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        public void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        public void OnErrorsChanged(string propertyName)
        {
            CanValidate = !_errorsByPropertyName.Any();
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public ErrorViewModelBase()
        {
            App.LanguageChanged += (s, e) =>
            {
                List<string> props = new List<string>();
                foreach (string propertyName in _errorsByPropertyName.Keys)
                {
                    props.Add(propertyName);
                }
                _errorsByPropertyName.Clear();
                foreach (string propertyName in props)
                {
                    OnErrorsChanged(propertyName);
                }
                mergedDict = Application.Current.Resources.MergedDictionaries.FirstOrDefault(md => md.Source.OriginalString.Contains("lang"));
            };
        }
    }
}
