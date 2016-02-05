using System;
using System.Windows;
using TimeScheduler.Managers;
using TimeScheduler.Services;
using TimeScheduler.Stores;
using TimeScheduler.Utils;

namespace TimeScheduler
{

    public partial class MainWindow
    {
        private const bool DefaultWorkState = true;

        private TimerService _timerService;

        public MainWindow()
        {
            InitializeComponent();
            InitBase(NavLanguageSelector);
            InitStores();
            InitServices();
        }

        private void InitServices()
        {
            _timerService = new TimerService();
            _timerService.Add(OnTimerTick, OnTimerRestart);
        }

        private void InitStores()
        {
            BaseStatesStore.Init(DefaultWorkState);
        }

        private void OnTimerTick(long allTime, long currentTime)
        {
            TimerTickProgress.Value = currentTime;
            ElapsedTimeTile.Count = TimeConverter.FromSeconds(currentTime);
            LeftTimeTile.Count = TimeConverter.FromSeconds(allTime - currentTime);
        }

        private void OnTimerRestart()
        {
            BaseStatesStore.Revert();
            _timerService.Set(TimerStore.Timelapse, TimerStore.TickInterval);
            TimerTickProgress.Maximum = TimerStore.Timelapse;
        }

        private void OnValueChanged(long workValue, long restValue)
        {
            TimerStore.Set(workValue, restValue);
            if (TimerStartButton == null) return;
            if (workValue == 0 || restValue == 0) TimerStartButton.IsEnabled = false;
            else TimerStartButton.IsEnabled = true;
        }

        private void OnTimerStart(object sender, RoutedEventArgs e)
        {
            TimerTickProgress.Maximum = TimerStore.Timelapse;
            _timerService.Set(TimerStore.Timelapse, TimerStore.TickInterval);
            ActivityManager.Create(TimerStore.Timelapse, BaseStatesStore.IsWork);
            _timerService.Start();
        }

        private void OnTimerStop(object sender, RoutedEventArgs e)
        {
          ActivityManager.ToList(list =>
          {
              foreach (var row in list)
              {
                  MessageBox.Show(row.ToString());
              }
          });
        }

        private void OnPauseStop(object sender, RoutedEventArgs e)
        {

        }

        private void OnTimerResume(object sender, RoutedEventArgs e)
        {

        }

        private void NavAboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            AboutFlyout.IsOpen = !AboutFlyout.IsOpen;
        }

        private void workTimelapseSetter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (RestTimelapseSetter == null || e.NewValue == null || RestTimelapseSetter.Value == null) return;
            RestTimelapseSetter.Minimum = Math.Ceiling((double)e.NewValue * 0.2);
            OnValueChanged((long) e.NewValue, (long) RestTimelapseSetter.Value);
        }

        private void restTimelapseSetter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (WorkTimelapseSetter == null || e.NewValue == null || WorkTimelapseSetter.Value == null) return;
            OnValueChanged((long) WorkTimelapseSetter.Value, (long) e.NewValue);
        }
    }
}
