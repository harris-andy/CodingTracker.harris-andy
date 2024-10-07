using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class RandomDateTime
    {
        private static Random random = new Random();
        public static DateTime RandomDate()
        {
            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = DateTime.Now;
            int range = (end - start).Days;
            int randomDays = random.Next(range);

            return start.AddDays(randomDays);
        }

        public static DateTime RandomStartDateTime(DateTime date)
        {
            int hours = random.Next(0, 24);
            int minutes = random.Next(0, 60);
            int seconds = random.Next(0, 60);

            return date.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }

        public static DateTime RandomEndDateTime(DateTime startTime)
        {
            int minutes = random.Next(1, 181);
            return startTime.AddMinutes(minutes);
        }
    }
}