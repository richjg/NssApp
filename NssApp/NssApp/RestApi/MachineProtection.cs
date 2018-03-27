using System;
using System.Collections.Generic;
using System.Text;

namespace NssApp.RestApi
{

    public class MachineProtection
    {
        public int EntityId { get; set; }

        public string EntityType { get; set; }

        public List<ApiProtectedLevel> ProtectedLevels { get; set; } = new List<ApiProtectedLevel>();

        public List<ApiPolicy> UnmatchedPolicies { get; set; } = new List<ApiPolicy>();
    }

    public class ApiPolicy
    {
        /// <summary>
        /// Id of the policy.   Unique within EntityType
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the entity. An entity is either a Machine, VApp or Vdc.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// The type of entity. Allowed values are Machine, VApp or Vdc.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Name of the NetBackup policy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The list of files and folder which are backed up by this policy.
        /// If this is not a file protect policy this property will be null.
        /// </summary>
        public List<string> Paths { get; set; }

        /// <summary>
        /// The template policy associated with this machine policy.
        /// If the machine policy is not matched (i.e. is not part of a protection level), then this property will be null.
        /// </summary>
        public ApiProtectionTemplatePolicy TemplatePolicy { get; set; }

        /// <summary>
        /// Used to determine if backups are happening regularly for the policy.
        /// If a backup has occured within the warning threshold (Defined in the TemplatePolicy WarningThresholdHours) then this property will return true.
        /// If no backup has occured within the warning threshold then this property will return false.
        /// If the machine policy is not matched (i.e. is not part of a protection level), then this property will be null.
        /// </summary>
        public bool? IsWithinThreshold { get; set; }
    }

    public class ApiProtectedLevel
    {
        /// <summary>
        /// Id of the entity. An entity is either a Machine, VApp or Vdc.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// The type of entity. Allowed values are Machine, VApp or Vdc.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Protection Level information
        /// </summary>
        public ApiProtectionLevel ProtectionLevel { get; set; }

        /// <summary>
        /// List of policies this protection level contains
        /// </summary>
        public List<ApiPolicy> Policies { get; set; }
    }

    public class ApiProtectionLevel
    {
        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the parent protection type.
        /// </summary>
        public int ProtectionTypeId { get; set; }

        /// <summary>
        /// Name of the protection level displayed to the end user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Code that is used to build up the policy name that NetBackup Self Service creates in NetBackup.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Description of the protection level displayed to the end user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Sequence of the protection level display in NetBackup Self Service.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// Color of the level displayed in NetBackup Self Service. Can be any HTML color e.g. '#FF0000', 'Red'.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Code of the request type to be created when protecting something.
        /// </summary>
        public string RequestTypeCode { get; set; }

        /// <summary>
        /// Flag to set visibility of the protection for selection in NetBackup Self Service.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Flag to set if this protection level will be used for 'Backup Now' functionality. 
        /// If set to true IsImmediate should be true for child protection template policies.
        /// </summary>
        public bool IsBackupNow { get; set; }

        /// <summary>
        /// Flag to set if this protection level is managed by the NSS system.
        /// If IsManaged is false NSS will monitor netbackup images for this protection level, but it will not be possible add or remove protection.
        /// </summary>
        public bool IsManaged { get; set; }
    }

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

    public class ApiScheduleWindow
    {
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Sunday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Monday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Tuesday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Wednesday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Thursday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Friday { get; set; }
        /// <summary>
        /// Options
        /// </summary>
        public ApiScheduleWindowOption Saturday { get; set; }

    }

    public class ApiScheduleWindowOption
    {
        /// <summary>
        /// Start time in seconds since midnight 
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// Duration of window in seconds. Must be less than 604,800 ( 1 week in seconds)
        /// </summary>
        public int Duration { get; set; }
    }
}
