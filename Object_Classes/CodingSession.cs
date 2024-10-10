using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class CodingSession
    {
        public int Id { get; set; }
        public string StartDayTime { get; set; }
        public string EndDayTime { get; set; }
        public string? Activity { get; set; }
        public DateTime StartDateTime => DateTime.Parse(StartDayTime);
        public DateTime EndDateTime => DateTime.Parse(EndDayTime);
        public int Duration => (int)(EndDateTime - StartDateTime).TotalMinutes;
        public CodingSession()
        {
            StartDayTime = string.Empty;
            EndDayTime = string.Empty;
        }

        public CodingSession(string startDayTime, string endDayTime, string activity)
        {
            // Id = id;
            StartDayTime = startDayTime;
            EndDayTime = endDayTime;
            Activity = activity;
        }
    }

    public class SummaryReport
    {
        public required string DateRange;
        public decimal TotalTime;
        public decimal AvgTime;
        public int Sessions;
        public required string Activity;
        public SummaryReport() { }
    }
}