using System;
using System.Collections.Generic;
using System.Linq;
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
using MahApps.Metro.Controls;
using TimeScheduler.Services;
using TimeScheduler.Stores;
using TimeScheduler.Utils;

namespace TimeScheduler
{

    public partial class MainWindow : MetroWindow
    {
        private bool DEFAULT_WORK_STATE = true;

        private TimerService _timerService = null;

        public MainWindow()
        {
            InitializeComponent();
            BaseStatesStore.Init(DEFAULT_WORK_STATE);
            _timerService = new TimerService();
            _timerService.Add(onTimerTick, onTimerRestart);
        }

        private void onStart(object sender, RoutedEventArgs e)
        {
            timerTickProgress.Maximum = TimerStore.Timelapse;
            _timerService.Set(TimerStore.Timelapse, TimerStore.TickInterval);
            _timerService.Start();
        }

        public void onTimerTick(long allTime, long currentTime)
        {
            timerTickProgress.Value = currentTime;
        }

        public void onTimerRestart()
        {
            BaseStatesStore.Revert();
            _timerService.Set(TimerStore.Timelapse, TimerStore.TickInterval);
            timerTickProgress.Maximum = TimerStore.Timelapse;
        }

        private void workTimelapseSetter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (restTimelapseSetter != null)
            {
                restTimelapseSetter.Minimum = (double)e.NewValue * 0.2;
                onValueChanged((long) e.NewValue, (long) restTimelapseSetter.Value);
            }
        }

        private void restTimelapseSetter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (workTimelapseSetter != null)
            {
                onValueChanged((long) workTimelapseSetter.Value, (long) e.NewValue);
            }
        }

        private void onValueChanged(long workValue, long restValue)
        {
            TimerStore.Set(workValue, restValue);
            if (timerStartButton != null)
            if (workValue == 0 || restValue == 0) timerStartButton.IsEnabled = false;
            else timerStartButton.IsEnabled = true;
        }
    }
}
