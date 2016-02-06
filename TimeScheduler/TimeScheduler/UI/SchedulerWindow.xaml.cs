using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
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
            Init();
        }

        private void Init()
        {
            _timerService = new TimerService();
            _timerService.Add(OnTimerTick, OnTimerRestart, () =>
            {
               // Add change toast
            });
            BaseStatesStore.Init(DefaultWorkState);
            ActivityDataGrid.FormDataGrid();
            AddLanguageChangedEvent(() => ActivityDataGrid.SetHeaders());
            ActivityManager.ToList(list =>
                list.ForEach(activity => ActivityDataGrid.AddRow(activity.Duration, activity.Type)));
            TimerStatesStore.AddStateChangedEvent(ButtonVisibilityArea);
            ButtonVisibilityArea();
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
            ActivityDataGrid.AddRow((int)TimerValuesStore.Timelapse, BaseStatesStore.IsWork);
            ActivityManager.Create(TimerValuesStore.Timelapse, BaseStatesStore.IsWork);
            _timerService.Set(TimerValuesStore.Timelapse, TimerValuesStore.TickInterval);
            TimerTickProgress.Maximum = TimerValuesStore.Timelapse;
        }

        private void OnValueChanged(long workValue, long restValue)
        {
            TimerValuesStore.Set(workValue, restValue);
            if (TimerStartButton == null) return;
            if (workValue == 0 || restValue == 0) TimerStatesStore.CurrentState = TimerStatesStore.StateUndefiend;
            else TimerStatesStore.CurrentState = _timerService.IsActive ? TimerStatesStore.StateWork : TimerStatesStore.StateStop;
        }

        public void ButtonVisibilityArea()
        {
            switch (TimerStatesStore.CurrentState)
            {
                case TimerStatesStore.StateStop:
                    TimerStopButton.IsEnabled = false;
                    TimerStartButton.IsEnabled = true;
                    TimerPauseButton.IsEnabled = false;
                    break;
                case TimerStatesStore.StateWork:
                    TimerStopButton.IsEnabled = true;
                    TimerStartButton.IsEnabled = false;
                    TimerPauseButton.IsEnabled = true;
                    break;
                case TimerStatesStore.StatePause:
                    TimerStopButton.IsEnabled = false;
                    TimerStartButton.IsEnabled = false;
                    TimerPauseButton.IsEnabled = true;
                    break;
                case TimerStatesStore.StateResume:
                    TimerStopButton.IsEnabled = true;
                    TimerStartButton.IsEnabled = false;
                    TimerPauseButton.IsEnabled = true;
                    break;
                default:
                    TimerStopButton.IsEnabled = false;
                    TimerStartButton.IsEnabled = false;
                    TimerPauseButton.IsEnabled = false;
                    break;
            }
        }


        private void OnTimerStart(object sender, RoutedEventArgs e)
        {
            if (_timerService.IsActive) _timerService.Stop();
            TimerStatesStore.CurrentState = TimerStatesStore.StateWork;
            TimerTickProgress.Maximum = TimerValuesStore.Timelapse;
            ActivityDataGrid.AddRow((int)TimerValuesStore.Timelapse, BaseStatesStore.IsWork);
            ActivityManager.Create(TimerValuesStore.Timelapse, BaseStatesStore.IsWork);
            _timerService.Set(TimerValuesStore.Timelapse, TimerValuesStore.TickInterval);
            _timerService.Start();
        }

        private void OnTimerStop(object sender, RoutedEventArgs e)
        {
            TimerStatesStore.CurrentState = TimerStatesStore.StateStop;
            ElapsedTimeTile.Count = Application.Current.FindResource("StateOff") as string;
            LeftTimeTile.Count = Application.Current.FindResource("StateOff") as string;
            _timerService.Stop();
        }

        private void OnTimerPause(object sender, RoutedEventArgs e)
        {
            if (TimerStatesStore.IsAnotherThan(TimerStatesStore.StateWork, TimerStatesStore.StateResume)) return;
            TimerStatesStore.CurrentState = TimerStatesStore.StatePause;
            TimerValuesStore.CurrentTime = _timerService.CurrentTime;
            _timerService.Stop();
            ShowResumeDialog();
        }

        private void OnTimerResume()
        {
            if (TimerStatesStore.IsAnotherThan(TimerStatesStore.StatePause)) return;
            TimerStatesStore.CurrentState = TimerStatesStore.StateResume;
            _timerService.CurrentTime = TimerValuesStore.CurrentTime;
            _timerService.Start();
        }

        private async void ShowResumeDialog()
        {
            await this.ShowMessageAsync(Application.Current.FindResource("ResumeDialogTitle") as string,
             Application.Current.FindResource("ResumeDialogBody") as string);
            OnTimerResume();
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

        private void SendMail_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:st235@yandex.ru?subject=Time Scheduler Bugs");
        }

        private void ToggleSwitch_OnIsCheckedChanged(object sender, EventArgs e)
        {
            NotificationStatesStore.Revert();
        }
    }
}
