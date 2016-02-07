using System;
using System.Globalization;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace TimeScheduler.UI.Base
{
    public class BaseWindow : MetroWindow
    {
        private event Action OnLanguageChanged;  

        private MenuItem Menu { get; set; }

        protected void InitBase(MenuItem menu)
        {
            Menu = menu;
            InitLanguage();
        }

        protected void AddLanguageChangedEvent(Action onLanguageChanged)
        {
            OnLanguageChanged += onLanguageChanged;
        }

        private void InitLanguage()
        {
            App.Language = Properties.Settings.Default.DefaultLanguage;
            CultureInfo currentLanguage = App.Language;
            App.LanguageChanged += LanguageChanged;
            BuildMenu(currentLanguage);
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currentLanguage = App.Language;

            foreach (MenuItem item in Menu.Items)
            {
                CultureInfo cultureInfo = item.Tag as CultureInfo;
                item.IsChecked = cultureInfo != null && cultureInfo.Equals(currentLanguage);
            }

            OnLanguageChanged?.Invoke();
        }

        private void BuildMenu(CultureInfo currentLanguage)
        {
            Menu.Items.Clear();
            foreach (var lang in App.Languages)
            {
                var item = new MenuItem { Header = lang.DisplayName, Tag = lang, IsChecked = lang.Equals(currentLanguage) };
                item.Click += Menu_Selected;
                Menu.Items.Add(item);
            }
        }

        private void Menu_Selected(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            CultureInfo language = menuItem?.Tag as CultureInfo;
            if (language != null)
            {
                App.Language = language;
            }
        }
    }
}
