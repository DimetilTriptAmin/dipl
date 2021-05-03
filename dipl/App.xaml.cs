using dipl.Models;
using dipl.Stores;
using dipl.ViewModels;
using dipl.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
        private static List<CultureInfo> m_Languages = new List<CultureInfo>();
		private static List<string> m_Themes = new List<string>();

        public static Account CurrentAccount { get; set; }

        public static List<CultureInfo> Languages
        {
            get
            {
                return m_Languages;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {

			System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

			m_Languages.Clear();
			m_Languages.Add(new CultureInfo("en-US"));
			m_Languages.Add(new CultureInfo("ru-RU"));

			NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new SignInViewModel(navigationStore);

            AuthWindow authWindow = new AuthWindow()
            {
                DataContext = new AuthViewModel(navigationStore)
            };

            authWindow.Show();

            base.OnStartup(e);
        }

		public static event EventHandler LanguageChanged;

		public static CultureInfo Language
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

				//1. Меняем язык приложения:
				System.Threading.Thread.CurrentThread.CurrentUICulture = value;

				//2. Создаём ResourceDictionary для новой культуры
				ResourceDictionary dict = new ResourceDictionary();
				switch (value.Name)
				{
					case "ru-RU":
						dict.Source = new Uri("Assets/Dictionaries/langs/lang.ru-RU.xaml", UriKind.Relative);
						break;
					case "en-ES":
						dict.Source = new Uri("Assets/Dictionaries/langs/lang.en-US.xaml", UriKind.Relative);
						break;
					default:
						dict.Source = new Uri("Assets/Dictionaries/langs/lang.en-US.xaml", UriKind.Relative);
						break;
				}

				//3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
				ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
											  where d.Source != null && d.Source.OriginalString.StartsWith("Assets/Dictionaries/langs/lang.")
											  select d).First();
				if (oldDict != null)
				{
					int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
					Application.Current.Resources.MergedDictionaries.Remove(oldDict);
					Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
				}
				else
				{
					Application.Current.Resources.MergedDictionaries.Add(dict);
				}
			}
		}

		public static List<string> Themes
		{
			get
			{
				return m_Themes;
			}
		}

		public static string Theme
		{
			set
			{

				if (value == null) throw new ArgumentNullException("value");

				ResourceDictionary dict = new ResourceDictionary();
				switch (value)
				{
					case "light":
						dict.Source = new Uri("Assets/Dictionaries/themes/theme.light.xaml", UriKind.Relative);
						break;
					default:
						dict.Source = new Uri("Assets/Dictionaries/themes/theme.dark.xaml", UriKind.Relative);
						break;
				}

				//3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
				ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
											  where d.Source != null && d.Source.OriginalString.StartsWith("Assets/Dictionaries/themes/theme.")
											  select d).First();
				if (oldDict != null)
				{
					int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
					Application.Current.Resources.MergedDictionaries.Remove(oldDict);
					Application.Current.Resources.MergedDictionaries.Insert(ind, dict);
				}
				else
				{
					Application.Current.Resources.MergedDictionaries.Add(dict);
				}

			}
		}
	}
}
