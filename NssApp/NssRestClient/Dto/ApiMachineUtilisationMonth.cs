using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiMachineUtilisationMonth
    {
        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Date for this machine utilization data.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The Date the machine utilization data was last updated.
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }
        /// <summary>
        /// The Month for this machine utilization data.
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// The Year for this machine utilization data.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// The total image size at the beginning of the month.
        /// </summary>
        public long BeginTotalImageSizeBytes { get; set; }
        /// <summary>
        /// The number of images stored for the system at the beginning of the month.
        /// </summary>
        public int BeginTotalImageCount { get; set; }
        /// <summary>
        /// The total data transferred at the beginning of the month.
        /// </summary>
        public long BeginTotalTransferredSizeBytes { get; set; }
        /// <summary>
        /// The total image size on the last day of the month. For the current month, this is the value on the day of the calculation.
        /// </summary>
        public long EndTotalImageSizeBytes { get; set; }
        /// <summary>
        /// Number of images on the last day of the month. For the current month, this is the value on the day of the calculation.
        /// </summary>
        public int EndTotalImageCount { get; set; }
        /// <summary>
        /// The total data transferred on the last day of the month. For the current month, this is the value on the day of the calculation.
        /// </summary>
        public long EndTotalTransferredSizeBytes { get; set; }

        /// <summary>
        /// Maximum daily value of TotalImageSizeBytes for this month.
        /// </summary>
        public long MaxImageSizeBytes { get; set; }

        /// <summary>
        /// Minimum daily value of TotalImageSizeBytes, for this month.
        /// </summary>
        public long MinImageSizeBytes { get; set; }

        /// <summary>
        /// Maximum daily value of TotalTransferredSizeBytes for this month.
        /// </summary>
        public long MaxTransferredSizeBytes { get; set; }

        /// <summary>
        /// Minimum daily value of TotalTransferredSizeBytes, for this month.
        /// </summary>
        public long MinTransferredSizeBytes { get; set; }

        /// <summary>
        /// Average number of images for this month. For the current month, this is the average so far.
        /// </summary>
        public double AverageTotalImageCount { get; set; }

        /// <summary>
        /// Average used backup space for this month. For the current month, this is the average so far.
        /// </summary>
        public long AverageTotalImageSizeBytes { get; set; }

        /// <summary>
        /// Average data transferred for this month. For the current month, this is the average so far.
        /// </summary>
        public long AverageTotalTransferredSizeBytes { get; set; }

        /// <summary>
        /// Total size of new images created this month.
        /// </summary>
        public long NewImageSizeBytes { get; set; }

        /// <summary>
        /// Number of new images created this month.
        /// </summary>
        public int NewImageCount { get; set; }

        /// <summary>
        /// Total data transferred for new images created this month.
        /// </summary>
        public long NewTransferredSizeBytes { get; set; }

        /// <summary>
        /// Size of images which have expired this month.
        /// </summary>
        public long ExpiredImageSizeBytes { get; set; }

        /// <summary>
        /// Data transferred for images which have expired this month.
        /// </summary>
        public long ExpiredTransferredSizeBytes { get; set; }

        /// <summary>
        /// Number of images which have expired this month.
        /// </summary>
        public int ExpiredImageCount { get; set; }

        /// <summary>
        /// Unique Identifier for the customer (tenant).
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// The Display Name of the machine.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The Unique Id of the machine.
        /// </summary>
        public int MachineId { get; set; }

        /// <summary>
        /// Obsolete.  This property now returns the DisplayName.  If you want to find the Catalog Names, use the machineId to look them up in the CatalogNameHistory.
        /// </summary>
        public string CatalogName => DisplayName;

        /// <summary>
        /// The Location of the machine.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The name of the tenant.
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// Unique database identifier for the tenant.
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Total cost of the images stored for this month.  For the current month this is the cost incurred so far.
        /// </summary>
        public decimal TotalImageCost { get; set; }

        /// <summary>
        /// Total cost of the new images for this month. For the current month this is the cost incurred so far.
        /// </summary>
        public decimal NewImageCost { get; set; }

        /// <summary>
        /// Total cost of the data transferred for images stored for this month.  For the current month this is the cost incurred so far.
        /// </summary>
        public decimal TotalTransferredCost { get; set; }

        /// <summary>
        /// Total cost of the data transferred for new images for this month. For the current month this is the cost incurred so far.
        /// </summary>
        public decimal NewTransferredCost { get; set; }

        /// <summary>
        /// Record of charge rate for this month.  Costs are calculated on a monthly basis, but the charge rate can vary from month to month.
        /// </summary>
        public decimal CostPerGb { get; set; }

        /// <summary>
        /// The Currency Code used for this months costs.
        /// </summary>
        public string CurrencyCode { get; set; }
    }
}
