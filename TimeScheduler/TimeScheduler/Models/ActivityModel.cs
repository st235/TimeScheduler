using System;

namespace TimeScheduler.Models
{
    public class ActivityModel
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"id:#{Id} date={Date} duration={Duration} type={Type}";
        }
    }
}
