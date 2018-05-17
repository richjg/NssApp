using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
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
}
