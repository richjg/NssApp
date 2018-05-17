using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace NssRestClient.Dto
{
    public class NssAccessRights
    {
        public enum AccessRight
        {
            ClientAdmin,
            ClientUser,
            Msp
        }

        public List<AccessRight> AccessRights { get; }

        public NssAccessRights(params AccessRight[] accessRights) : this(accessRights.Select(a => a))
        {
        }

        public NssAccessRights(IEnumerable<AccessRight> accessRights = null)
        {
            this.AccessRights = accessRights?.ToList() ?? new List<AccessRight>();
        }

        public bool HasAccessRight(AccessRight requiredAccessRight)
        {
            return AccessRights.Contains(requiredAccessRight);
        }

        public void AssertHasAccessRight(AccessRight requiredAccessRight)
        {
            if (this.HasAccessRight(requiredAccessRight) == false)
            {
                throw new SecurityException($"User does not have the required rights. '{requiredAccessRight}' is required.");
            }
        }
    }
}
