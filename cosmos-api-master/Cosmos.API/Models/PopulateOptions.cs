using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cosmos.API.Models
{
    /// <summary>
    /// Options for populating a database
    /// </summary>
    public class PopulateOptions
    {        
        /// <summary>
        /// Quantity of records
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int Quantity { get; set; }
    }
}