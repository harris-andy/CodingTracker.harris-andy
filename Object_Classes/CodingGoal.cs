using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class CodingGoal
    {
        public int Id { get; set; }
        public DateTime GoalStartDate { get; set; }
        public DateTime GoalEndDate { get; set; }
        public float GoalHours { get; set; }
        public string Complete = "no";

        public CodingGoal() { }

        public CodingGoal(DateTime startDate, DateTime endDate, float hours, string complete)
        {
            GoalStartDate = startDate;
            GoalEndDate = endDate;
            GoalHours = hours;
            Complete = complete;
        }
    }
}