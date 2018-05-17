using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiSystemUtilizationMonth
    {
        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The Date for this system utilization data.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The Date the utilization was last updated.
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// The Month for this system utilization data.
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// The Year for this system utilization data.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The total image size at the beginning of the month.
        /// </summary>
        public long BeginTotalImageSizeBytes { get; set; }

        /// <summary>
        /// The total transferred size at the beginning of the month.
        /// </summary>
        public long BeginTotalTransferredSizeBytes { get; set; }

        /// <summary>
        /// The number of images stored for the system at the beginning of the month.
        /// </summary>
        public int BeginTotalImageCount { get; set; }

        /// <summary>
        /// The total image size on the last day of the month. For the current month, this is the value on the day of the calculation.
        /// </summary>
        public long EndTotalImageSizeBytes { get; set; }

        /// <summary>
        /// The total transferred size on the last day of the month. For the current month, this is the value on the day of the calculation.
        /// </summary>
        public long EndTotalTransferredSizeBytes { get; set; }

        /// <summary>
        /// Number of images on the last day of the month. For the current month, this is the value on the day of the calculation.
        /// </summary>
        public int EndTotalImageCount { get; set; }

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
        /// Total transferred size of new images created this month.
        /// </summary>
        public long NewTransferredSizeBytes { get; set; }

        /// <summary>
        /// Number of new images created this month.
        /// </summary>
        public int NewImageCount { get; set; }

        /// <summary>
        /// Size of images which have expired this month.
        /// </summary>
        public long ExpiredImageSizeBytes { get; set; }

        /// <summary>
        /// Total size of data transferred that has expired this month.
        /// </summary>
        public long ExpiredTransferredSizeBytes { get; set; }

        /// <summary>
        /// Number of images which have expired this month.
        /// </summary>
        public int ExpiredImageCount { get; set; }
    }
}
