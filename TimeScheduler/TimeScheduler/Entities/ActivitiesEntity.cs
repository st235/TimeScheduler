using System.Data;
using TimeScheduler.Models;

namespace TimeScheduler
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

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
