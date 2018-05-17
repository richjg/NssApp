using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
    public class ApiProtected
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
        /// List of the protection applied
        /// </summary>
        public List<ApiProtectedLevel> ProtectedLevels { get; set; } = new List<ApiProtectedLevel>();

        /// <summary>
        /// List of policies not in a protection level
        /// </summary>
        public List<ApiPolicy> UnmatchedPolicies { get; set; } = new List<ApiPolicy>();
    }
}
