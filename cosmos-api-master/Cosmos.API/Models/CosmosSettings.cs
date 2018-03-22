namespace Cosmos.API.Models
{
    /// <summary>
    /// Database configuration settings
    /// </summary>
    public class CosmosSettings
    {        
        /// <summary>
        /// Database Identifier
        /// </summary>
        public string DatabaseId { get; set; }
        
        /// <summary>
        /// Collection Identifier
        /// </summary>
        public string CollectionId { get; set; }
        
        /// <summary>
        /// Endpoint Url
        /// </summary>
        public string EndpointUrl { get; set; }
        
        /// <summary>
        /// Authorization Key
        /// </summary>
        public string AuthorizationKey { get; set; }

        /// <summary>
        /// Stored Procedure Id
        /// </summary>
        public string StoredProcedureId { get; set; }
    }
}