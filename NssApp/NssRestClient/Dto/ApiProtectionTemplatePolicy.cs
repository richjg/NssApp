using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiProtectionTemplatePolicy
    {
        /// <summary>
        ///  Default Constructor
        /// </summary>
        public ApiProtectionTemplatePolicy()
        {
            SingleClientBackupScheduleName = string.Empty;
            StorageLifecyclePolicyName = string.Empty;
            Schedules = new List<ApiProtectionTemplatePolicySchedule>();
        }

        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the parent protection level.
        /// </summary>
        public int ProtectionLevelId { get; set; }

        /// <summary>
        /// Name of the protection template policy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The NetBackup policy type Id e.g. 40 (VMWare), 13 (Windows)
        /// </summary>
        public int PolicyTypeId { get; set; }

        /// <summary>
        /// The name of the NetBackup policy used as a template to create a policy used for protection.
        /// </summary>
        public string PolicyTemplateName { get; set; }

        /// <summary>
        /// The number of hours since something has been backed up before a traffic light warning occurs. 
        /// If not set the warning will occur immediately.
        /// </summary>
        public Nullable<int> WarningThresholdHours { get; set; }

        /// <summary>
        /// Flag set to determine if the policy runs immediately (once only) when something is protected.
        /// </summary>
        public bool IsImmediate { get; set; }

        /// <summary>
        /// Code that is used to build up the policy name that NetBackup Self Service creates in NetBackup.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// A file protection template policy allows protect individual files or folders on a machine.
        /// </summary>
        public bool IsFileProtect { get; set; }

        /// <summary>
        /// SingleClientBackup policies can be used to backup now a single client in the policy.
        /// </summary>
        public bool SupportsSingleClientBackup { get; set; }

        /// <summary>
        /// SingleClientBackupScheduleName is the name of the schedule in the policy to be used when performing a single client backup.
        /// </summary>
        public string SingleClientBackupScheduleName { get; set; }

        /// <summary>
        /// Name of StorageLifecyclePolicy that will be set on the created netbackup policy
        /// </summary>
        public string StorageLifecyclePolicyName { get; set; }

        /// <summary>
        /// List of Schedules that will be updated on the created netbackup policy
        /// </summary>
        public List<ApiProtectionTemplatePolicySchedule> Schedules { get; set; }
    }
}
