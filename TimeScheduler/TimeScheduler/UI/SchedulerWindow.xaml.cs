using System;
using System.Collections.ObjectModel;
using System.Windows;
using MahApps.Metro.Controls;
using TimeScheduler.Services;
using TimeScheduler.Stores;

namespace TimeScheduler
{

    public partial class MainWindow : MetroWindow
    {
        private bool _defaultWorkState = true;

        private readonly TimerService _timerService = null;

        public MainWindow()
        {
            InitializeComponent();
            BaseStatesStore.Init(_defaultWorkState);
            _timerService = new TimerService();
            _timerService.Add(OnTimerTick, OnTimerRestart);
            Init();
    }

        private void Init()
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            foreach (var lang in App.Languages)
            {
                list.Add(lang.DisplayName);
            }
            NavLanguageSelector.ItemsSource = list;
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

        private void NavAuthorButton_OnClick(object sender, RoutedEventArgs e)
        {
            AboutFlyout.IsOpen = !AboutFlyout.IsOpen;
        }
    }
}
