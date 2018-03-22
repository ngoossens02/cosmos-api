using System.Collections.Generic;
using System.Threading.Tasks;
using Cosmos.API.Models;

namespace Cosmos.API.Services
{
    /// <summary>
    /// Repository interface for database access
    /// </summary>
    public interface IDataRepository
    {
        /// <summary>
        /// Reads all documents
        /// </summary>
        Task<IEnumerable<dynamic>> ReadDocuments();
        
        /// <summary>
        /// Retrieve a single documents
        /// </summary>
        Task<dynamic> RetrieveDocument(string id);

        /// <summary>
        /// Creates documents using standardized stored proedure
        /// </summary>
        Task<IEnumerable<dynamic>> GenerateDocuments(int count);

        /// <summary>
        /// Searches all documents
        /// </summary>
        Task<IEnumerable<dynamic>> SearchDocuments(string query);

        /// <summary>
        /// Creates a standardized stored procedure
        /// </summary>
        Task CreateGeneratorStoredProcedure();
    }
}