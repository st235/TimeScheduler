using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using TimeScheduler.Models;
using TimeScheduler.Utils;

namespace TimeScheduler.Managers
{
    class ActivityManager
    {

        private static ActivitiesEntity DbContext { get; set; }

        public static void Init(ActivitiesEntity dbContext)
        {
            DbContext = dbContext;
        }

        public static async void Create(long duration, bool isWork)
        {
            await Task.Run(() =>
            {
                DbContext.Activities.Add(entity: new ActivityModel { Date = DateTime.Now, Duration = TimeConverter.ToMinutes(duration), Type = isWork ? Application.Current.FindResource("StateWork") as string : Application.Current.FindResource("StateRest") as string });
                DbContext.SaveChanges();
            });
        }

        public static async void ToList(Action<List<ActivityModel>> onComplete)
        {
            var list = await DbContext.Activities.ToListAsync();
            if (list != null) onComplete(list);
        }
    }
}
