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

    public class Activity<T> : Activity
    {
        public T ActivityData { get; set; }
    }

    public class ProtectMachineActivityData
    {
        // "Data": "{\"MachineId\":5556,\"ActivityId\":158904,\"ProtectionLevelId\":240,\"RetentionLevel\":null,\"StorageLifecyclePolicyName\":null,\"RequestId\":null,\"IsBackupNow\":false,\"Paths\":[]}",
        public int MachineId { get; set; }
        public int ProtectionLevelId { get; set; }
        public ApiProtectionLevel ApiProtectionLevel { get; set; }
    }
}
