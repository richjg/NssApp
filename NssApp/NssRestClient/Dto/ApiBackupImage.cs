using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiBackupImage
    {
        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Location name used to identify the NetBackup master server the backup is stored on. 
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The name NetBackup uses to identify the machine that the backup belongs to.
        /// </summary>
        public string CatalogName { get; set; }

        /// <summary>
        /// The size of the backup image in bytes.
        /// </summary>
        public long ImageSizeBytes { get; set; }

        /// <summary>
        /// The bytes actually transferred across the network during backup.
        /// </summary>
        public long TransferredSizeBytes { get; set; }

        private DateTime backupTime;
        /// <summary>
        /// The date and time that the backup started.
        /// </summary>
        /// 
        public DateTime BackupTime { get; set; }

        /// <summary>
        /// The date and time that the backup expires. 
        /// </summary>
        public DateTime ImageExpirationTime { get; set; }

        /// <summary>
        /// The name of the policy that caused the backup.
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// The NetBackup identifier for the backup.
        /// </summary>
        public string BackupId { get; set; }

        /// <summary>
        /// The type of backup. 0 = Full, 1 = Differential, 2 = User Backup, 3 = User Archive, 4 = Cumulative, 5 = Transaction Log
        /// </summary>
        public int BackupTypeId { get; set; }

        /// <summary>
        /// The NetBackup policy type Id e.g. 40 (VMWare), 13 (Windows)
        /// </summary>
        public int PolicyTypeId { get; set; }

        /// <summary>
        /// Flag of whether the image has expired.
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// The day that the backup occurred.
        /// </summary>
        public int BackupDay { get; set; }

        /// <summary>
        /// The month that the backup occurred.
        /// </summary>
        public int BackupMonth { get; set; }

        /// <summary>
        /// The year that the backup occurred.
        /// </summary>
        public int BackupYear { get; set; }

        /// <summary>
        /// The day that the backup expires.
        /// </summary>
        public int ExpiredDay { get; set; }

        /// <summary>
        /// The month that the backup expires.
        /// </summary>
        public int ExpiredMonth { get; set; }

        /// <summary>
        /// The year that the backup expires.
        /// </summary>
        public int ExpiredYear { get; set; }

        /// <summary>
        /// The number of copies of the backup that exist.
        /// </summary>
        public int NumCopies { get; set; }
    }
}
