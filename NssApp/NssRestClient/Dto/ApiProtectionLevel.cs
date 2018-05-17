using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
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
}
