using System;
using System.Collections.Generic;
using System.Text;

namespace NssApp.RestApi
{
    public class MachineImage
    {
        public int Id { get; set; }
        public DateTime BackupTime { get; set; }
        public bool IsExpired { get; set; }
    }
}
