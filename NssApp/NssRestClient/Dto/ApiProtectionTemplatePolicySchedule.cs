using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiProtectionTemplatePolicySchedule
    {
        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the ProtectionTemplatePolicy
        /// </summary>
        public int ProtectionTemplatePolicyId { get; set; }

        /// <summary>
        /// Name of the Schedule that will be updated
        /// </summary>
        public string ScheduleName { get; set; }

        /// <summary>
        /// The frequency in seconds to set against the schedule
        /// </summary>
        public int? Frequency { get; set; }

        /// <summary>
        /// The retention level to set against the schedule
        /// </summary>
        public int? RetentionLevel { get; set; }

        /// <summary>
        /// The window within which the backups will start for this schedule
        /// </summary>
        public ApiScheduleWindow BackupWindow { get; set; }
    }
}
