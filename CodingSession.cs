using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class CodingSession
    {
        private int Id;
        private DateTime StartTime;
        private DateTime EndTime;
        private int Duration;

        public CodingSession(int id, DateTime startTime, DateTime endTime)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;

            Duration = (int)(EndTime - StartTime).TotalMinutes;
        }


    }
}