using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace TimeScheduler
{ 
    public partial class App : Application
    {

        private static List<CultureInfo> _languages = new List<CultureInfo>();
        public static event EventHandler LanguageChanged;

        public static List<CultureInfo> Languages
        {
            get
            {
                return _languages;
            }
        }

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

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dict = new ResourceDictionary();

                switch (value.Name)
                {
                    case "ru-RU":
                        dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
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

                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        public App() {
            _languages.Clear();
            _languages.Add(new CultureInfo("en-US"));
            _languages.Add(new CultureInfo("ru-RU"));
            App.LanguageChanged += App_LanguageChanged;
        }

        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Language = TimeScheduler.Properties.Settings.Default.DefaultLanguage;
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            TimeScheduler.Properties.Settings.Default.DefaultLanguage = Language;
            TimeScheduler.Properties.Settings.Default.Save();
        }
    }
}
