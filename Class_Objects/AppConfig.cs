using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker.harris_andy
{
    public class AppConfig
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public static string dbPath = ConfigurationManager.AppSettings["DB-Path"] ?? "./";
    }
}