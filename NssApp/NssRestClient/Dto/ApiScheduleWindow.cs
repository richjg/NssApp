using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiScheduleWindow
    {
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Sunday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Monday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Tuesday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Wednesday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Thursday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Friday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Saturday { get; set; }

    }
}
