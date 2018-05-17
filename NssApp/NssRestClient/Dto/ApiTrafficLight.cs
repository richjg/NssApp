using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiTrafficLight
    {
        /// <summary>
        /// Id of the tenant that owns the machines.
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// Customer Code of the tenant that owns the machines.
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Number of machines with a 'Red' traffic light status.
        /// </summary>
        public int RedCount { get; set; }
        /// <summary>
        /// Number of machines with a 'Amber' traffic light status.
        /// </summary>
        public int AmberCount { get; set; }
        /// <summary>
        /// Number of machines with a 'Green' traffic light status.
        /// </summary>
        public int GreenCount { get; set; }
    }
}
