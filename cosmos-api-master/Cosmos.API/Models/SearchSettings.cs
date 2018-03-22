namespace Cosmos.API.Models
{
    /// <summary>
    /// Search Engine configuration settings
    /// </summary>
    public class SearchSettings
    {                
        /// <summary>
        /// Index Identifier
        /// </summary>
        public string IndexId { get; set; }
        
        /// <summary>
        /// Account Name
        /// </summary>
        public string AccountName { get; set; }
        
        /// <summary>
        /// Query Key
        /// </summary>
        public string QueryKey { get; set; }
    }
}