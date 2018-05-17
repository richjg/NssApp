using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiScheduleWindowOption
    {
        /// <summary>
        /// Start time in seconds since midnight 
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// Duration of window in seconds. Must be less than 604,800 ( 1 week in seconds)
        /// </summary>
        public int Duration { get; set; }
    }
}
