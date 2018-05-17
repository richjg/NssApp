using System;
using System.Collections.Generic;
using System.Text;

namespace NssRestClient.Dto
{
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
}
