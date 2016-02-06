using TimeScheduler.Models;

namespace TimeScheduler
{
    using System.Data.Entity;

    public partial class ActivitiesEntity : DbContext
    {
        public DbSet<ActivityModel> Activities { get; set; }

        public ActivitiesEntity()
            : base("name=ActivitiesEntity")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
