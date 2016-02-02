using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace TimeScheduler
{ 
    public partial class App
    {

        private static readonly String ERROR_EXCEPTION = "Valus is null";

        private static List<CultureInfo> _languages = new List<CultureInfo>();
        public static event EventHandler LanguageChanged;

        public static List<CultureInfo> Languages => _languages;

        public static CultureInfo Language
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(ERROR_EXCEPTION);
                //if (Equals(value, System.Threading.Thread.CurrentThread.CurrentUICulture)) return;

                System.Threading.Thread.CurrentThread.CurrentUICulture = value;

                ResourceDictionary dictonary = new ResourceDictionary();
                switch (value.Name)
                {
                    case "ru-RU":
                        dictonary.Source = new Uri($"/Resources/lang.{value.Name}.xaml", UriKind.Relative);
                        break;
                    default:
                        dictonary.Source = new Uri("/Resources/lang.xaml", UriKind.Relative);
                        break;
                }

                ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
                                              where d.Source != null && d.Source.OriginalString.StartsWith("/Resources/lang")
                                              select d).First();
                if (oldDict != null)
                {
                    int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
                    Current.Resources.MergedDictionaries.Remove(oldDict);
                    Current.Resources.MergedDictionaries.Insert(ind, dictonary);
                }
                else
                {
                    Current.Resources.MergedDictionaries.Add(dictonary);
                }

                LanguageChanged(Current, new EventArgs());
            }
        }

        public App() {
            InitLanguage();
        }

        private void InitLanguage()
        {
            _languages.Clear();
            _languages.Add(new CultureInfo("en-US"));
            _languages.Add(new CultureInfo("ru-RU"));
            LanguageChanged += App_LanguageChanged;
        }

        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Language = TimeScheduler.Properties.Settings.Default.DefaultLanguage;
            MessageBox.Show(TimeScheduler.Properties.Settings.Default.DefaultLanguage.DisplayName);
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
            TimeScheduler.Properties.Settings.Default.DefaultLanguage = Language;
            TimeScheduler.Properties.Settings.Default.Save();
        }
    }
}
