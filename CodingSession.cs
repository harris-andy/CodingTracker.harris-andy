using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class CodingSession
    {
        public int Id { get; set; }
        public required string StartDayTime { get; set; }
        public required string EndDayTime { get; set; }
        public string? Activity { get; set; }
        public DateTime StartDateTime => DateTime.Parse(StartDayTime);
        public DateTime EndDateTime => DateTime.Parse(EndDayTime);
        public int Duration => (int)(EndDateTime - StartDateTime).TotalMinutes;
        public CodingSession() { }

        public CodingSession(string startDateTime, string endDateTime, string activity)
        {
            // Id = id;
            StartDayTime = startDateTime;
            EndDayTime = endDateTime;
            Activity = activity;
        }
    }

    public class SummaryReport
    {
        /*
        Day
        TotalTime
        AvgTime
        Sessions
        Activity
        */
        public required string DateRange;
        public decimal TotalTime;
        public decimal AvgTime;
        public int Sessions;
        public required string Activity;

        public SummaryReport() { }
    }
}