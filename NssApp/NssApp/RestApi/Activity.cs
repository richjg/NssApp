using System;
using System.Collections.Generic;
using System.Text;

namespace NssApp.RestApi
{
    public class Activity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime UpdatedDateTime { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public string Exception { get; set; }
        public bool IsScheduled { get; set; }
        public string FrontOfficeTrackingId { get; set; }
        public string EntityType { get; set; }
        public int EntityKey { get; set; }
        public string CustomerCode { get; set; }
        public int? SupersededBy { get; set; }
        public IEnumerable<int> ContinuationIds { get; set; }
    }
}
