using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using TimeScheduler.Services;
using TimeScheduler.Stores;
using TimeScheduler.Utils;

namespace TimeScheduler
{

    public partial class MainWindow : MetroWindow
    {
        private bool _defaultWorkState = true;

        private readonly TimerService _timerService = null;

        public MainWindow()
        {
            InitializeComponent();
            InitLanguage();
            BaseStatesStore.Init(_defaultWorkState);
            _timerService = new TimerService();
            _timerService.Add(OnTimerTick, OnTimerRestart);
        }

        private void InitLanguage()
        {
            CultureInfo currentLanguage = App.Language;
            App.Language = currentLanguage;

            App.LanguageChanged += LanguageChanged;

            NavLanguageSelector.Items.Clear();
            foreach (var lang in App.Languages)
            {
                var item = new MenuItem
                {
                    Header = lang.DisplayName,
                    Tag = lang,
                    IsChecked = lang.Equals(currentLanguage)
                };

                item.Click += NavLanguageSelector_Selected;

                NavLanguageSelector.Items.Add(item);
            }
        }

        private void NavLanguageSelector_Selected(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            foreach (MenuItem i in NavLanguageSelector.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            TimerTickProgress.Maximum = TimerStore.Timelapse;
            _timerService.Set(TimerStore.Timelapse, TimerStore.TickInterval);
            _timerService.Start();
        }

        public void OnTimerTick(long allTime, long currentTime)
        {
            TimerTickProgress.Value = currentTime;
            ElapsedTimeTile.Count = TimeConverter.FromSeconds(currentTime);
            LeftTimeTile.Count = TimeConverter.FromSeconds(allTime - currentTime);
        }

        public void OnTimerRestart()
        {
            BaseStatesStore.Revert();
            _timerService.Set(TimerStore.Timelapse, TimerStore.TickInterval);
            TimerTickProgress.Maximum = TimerStore.Timelapse;
        }

        private void workTimelapseSetter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (RestTimelapseSetter != null && e.NewValue != null)
            {
                RestTimelapseSetter.Minimum = Math.Ceiling((double)e.NewValue * 0.2);
                OnValueChanged((long) e.NewValue, (long) RestTimelapseSetter.Value);
            }
        }

        private void restTimelapseSetter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (WorkTimelapseSetter != null && e.NewValue != null)
            {
                OnValueChanged((long) WorkTimelapseSetter.Value, (long) e.NewValue);
            }
        }

        private void OnValueChanged(long workValue, long restValue)
        {
            TimerStore.Set(workValue, restValue);
            if (TimerStartButton != null)
            if (workValue == 0 || restValue == 0) TimerStartButton.IsEnabled = false;
            else TimerStartButton.IsEnabled = true;
        }

        private void NavAboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            AboutFlyout.IsOpen = !AboutFlyout.IsOpen;
        }
    }
}
