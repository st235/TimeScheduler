using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TimeScheduler.Models;

namespace TimeScheduler.Utils
{
    public static class DataGridFormedUtils
    {
        private static DataGridTextColumn _dateColumn, _timeColumn, _typeColumn;

        private static readonly string _dateBinding = "Date";
        private static readonly string _durationBinding = "Duration";
        private static readonly string _typeBinding = "Type";

        public static void FormDataGrid(this DataGrid dataGrid)
        {
            _dateColumn = new DataGridTextColumn();
            _timeColumn = new DataGridTextColumn();
            _typeColumn = new DataGridTextColumn();

            dataGrid.SetHeaders();

            dataGrid.Columns.Add(_dateColumn);
            dataGrid.Columns.Add(_timeColumn);
            dataGrid.Columns.Add(_typeColumn);

            _dateColumn.Binding = new Binding(_dateBinding);
            _timeColumn.Binding = new Binding(_durationBinding);
            _typeColumn.Binding = new Binding(_typeBinding);
        }

        public static void AddRow(this DataGrid dataGrid, int duration, string type)
        {
            dataGrid.Items.Add(new ActivityModel { Date = DateTime.Now, Duration = TimeConverter.ToMinutes(duration), Type = type });
        }

        public static void AddRow(this DataGrid dataGrid, int duration, bool isWork)
        {
            dataGrid.Items.Add(new ActivityModel { Date = DateTime.Now, Duration = TimeConverter.ToMinutes(duration), Type = isWork ? Application.Current.FindResource("StateWork") as string : Application.Current.FindResource("StateRest") as string });
        }

        public static void SetHeaders(this DataGrid dataGrid)
        {
            _dateColumn.Header = Application.Current.FindResource("ColumnData") as string;
            _timeColumn.Header = Application.Current.FindResource("ColumnTime") as string;
            _typeColumn.Header = Application.Current.FindResource("ColumnType") as string;
        }
    }
}
