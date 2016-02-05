using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using TimeScheduler.Models;

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
                DbContext.Activities.Add(entity: new ActivityModel { Date = DateTime.Now, Duration = (int)duration });
                DbContext.SaveChanges();
            });
        }

        public static async void ToList(Action<List<ActivityModel>> onComplete)
        {
            var list = await DbContext.Activities.ToListAsync();
            onComplete(list);
        }
    }
}
