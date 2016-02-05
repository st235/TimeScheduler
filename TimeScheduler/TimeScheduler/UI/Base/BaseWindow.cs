using System;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace TimeScheduler.UI.Base
{
    public class BaseWindow : MetroWindow
    {

        private MenuItem Menu { get; set; }

        protected void InitBase(MenuItem menu)
        {
            Menu = menu;
            InitLanguage();
        }

        private void InitLanguage()
        {
            CultureInfo currentLanguage = App.Language;
            App.Language = currentLanguage;

            App.LanguageChanged += LanguageChanged;

            Menu.Items.Clear();

            foreach (var lang in App.Languages)
            {
                var item = new MenuItem { Header = lang.DisplayName, Tag = lang, IsChecked = lang.Equals(currentLanguage) };
                item.Click += Menu_Selected;
                Menu.Items.Add(item);
            }
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            foreach (MenuItem item in Menu.Items)
            {
                CultureInfo ci = item.Tag as CultureInfo;
                item.IsChecked = ci != null && ci.Equals(currLang);
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
