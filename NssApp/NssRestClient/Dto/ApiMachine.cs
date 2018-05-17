 using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiMachine
    {
        /// <summary>
        /// Generated Id unique across NetBackup Self Service (Read Only).
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Machine Code unique for a tenant.
        /// </summary>
        public string MachineCode { get; set; }
        /// <summary>
        /// Customer code of the tenant that owns the machine.
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// Location name used to identify the NetBackup master server the machine is backed up on.
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// NetBackup hardware name e.g. Linux
        /// </summary>
        public string Hardware { get; set; }
        /// <summary>
        /// NetBackup OS name e.g. RedHat2.6.18
        /// </summary>
        public string OS { get; set; }
        /// <summary>
        /// The name of the machine that NetBackup uses to communicate with the NetBackup agent.
        /// Required for file restore.
        /// </summary>
        public string NetBackupClientName { get; set; }
        /// <summary>
        /// The name of the VM. 
        /// Required for VM backup and restore.
        /// </summary>
        public string VMDisplayName { get; set; }
        /// <summary>
        /// Obsolete. The name NetBackup uses to identify the machine that the backup belongs to. Instead use MachineCatalogNameHistory to find Catalog Names.
        /// </summary>
        public string CatalogName { get; set; }
        /// <summary>
        /// Flag to set whether all users have visibility of the machine in NetBackup Self Service.
        /// If set to false the machine will only be visible to users in machine users for the machine.
        /// </summary>
        public bool IsVisibleToAllUsers { get; set; }
        /// <summary>
        /// Name displayed in NetBackup Self Service.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Flag that indicates if a machine is waiting to have its NetBackup data synced (Read Only).
        /// </summary>
        public bool IsSyncingNetBackupData { get; set; }
        /// <summary>
        /// The last error that occurred if a sync has errored.
        /// </summary>
        public string SyncLastError { get; set; }
        /// <summary>
        /// The name of the import source. If using vCloud import it is the name of the import configured in NetBackup Self Service.
        /// </summary>
        public string ImportSource { get; set; }
        /// <summary>
        /// Flag to indicate whether this is a VM in vCloud Director.
        /// </summary>
        public bool IsInVCloud { get; set; }
        /// <summary>
        /// Id of this Machine's containing VApp. Null if Machine does not belong to a VApp.
        /// </summary>
        public int? VAppId { get; set; }
        /// <summary>
        /// Free form storage for storing additional information about a machine for integration purposes.
        /// </summary>
        public string AdditionalData { get; set; }
        /// <summary>
        /// Flag that indicates when the machine has been deleted from its original source e.g. vCloud Director.
        /// </summary>
        public bool IsDeletedFromImportSource { get; set; }
        /// <summary>
        /// The Id of the protection type that the protection level can be selected from.
        /// </summary>
        public int? ProtectionTypeId { get; set; }
        /// <summary>
        /// The traffic light status of a machine (Read Only). Can be 'Red', 'Amber' or 'Green'.
        /// </summary>
        public string TrafficLightStatus { get; set; }
    }
}
