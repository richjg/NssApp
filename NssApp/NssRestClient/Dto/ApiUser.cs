using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiUser
    {
        /// <summary>
        /// FrontOffice UserId
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// FrontOffice User Guid
        /// </summary>
        public string UserGuid { get; set; }
        /// <summary>
        /// FrontOffice User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Id of the tenant this user is associated with.  Null if the user is not a tenant user.
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// Customer Code of the tenant this user is associated with.
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// FrontOffice access rights associated with the user.  Allowed values are ClientAdmin, ClientUser and Msp.
        /// </summary>
        public NssAccessRights NssAccessRights { get; set; } = new NssAccessRights();
        /// <summary></summary>
        public override string ToString() => this.ToJson();
        /// <summary></summary>
        public bool HasAccessRight(NssAccessRights.AccessRight right) => NssAccessRights.HasAccessRight(right);
        /// <summary>
        /// Returns true if the user has the ClientAdmin access right.
        /// </summary>
        public bool IsTenantAdmin => this.HasAccessRight(NssAccessRights.AccessRight.ClientAdmin);
        /// <summary>
        /// Returns true if the user has the Msp access right.
        /// </summary>
        public bool IsMsp => this.HasAccessRight(NssAccessRights.AccessRight.Msp);
    }
}
