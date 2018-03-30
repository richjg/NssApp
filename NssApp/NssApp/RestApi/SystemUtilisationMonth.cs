using System;
using System.Collections.Generic;
using System.Text;

namespace NssApp.RestApi
{
    public class SystemUtilisationMonth
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public long EndTotalImageSizeBytes { get; set; }
        public long EndTotalTransferredSizeBytes { get; set; }
    }
}
