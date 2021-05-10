using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace dipl.Assets.UserControls
{
    /// <summary>
    /// Логика взаимодействия для BindingPasswordBox.xaml
    /// </summary>
    public partial class BindingPasswordBox : UserControl
    {
        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(BindingPasswordBox),
                new PropertyMetadata(default(SecureString)));

        public BindingPasswordBox()
        {
            InitializeComponent();

            TextBox.TextChanged += (sender, args) => {
                PasswordBox.Password = TextBox.Text;
            };

            PasswordBox.PasswordChanged += (sender, args) => {
                Password = ((PasswordBox)sender).SecurePassword;
            };

            PasswordBox.LostFocus += (s, a) =>
            {
                TextBox.Text = PasswordBox.Password;
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)ToggleButton.IsChecked)
            {
                PasswordBox.Visibility = Visibility.Hidden;
                TextBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Visibility = Visibility.Visible;
                TextBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
