using System;
using System.Collections.Generic;
using System.Text;

namespace NssApp.RestApi
{
    public class MachineUtilisationMonth
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public DateTime Date { get; set; }
        public long EndTotalImageSizeBytes { get; set; }
        public long EndTotalTransferredSizeBytes { get; set; }
    }
}
