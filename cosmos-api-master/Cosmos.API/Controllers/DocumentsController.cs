using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmos.API.Models;
using Cosmos.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;

namespace Cosmos.API.Controllers
{
    /// <summary>
    /// Controller used to manage documents in a Cosmos DB instance
    /// </summary>
    [Produces("application/json")]
    public class DocumentsController : Controller
    {
        /// <summary>
        /// Retrives all documents from the Cosmos DB instance
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /documents
        ///
        /// </remarks>
        /// <returns>All documents</returns>
        /// <response code="200">Returns all documents</response>
        [HttpGet]
        [Route("[controller]")]
        [ProducesResponseType(typeof(IEnumerable<Person>), 200)]
        public async Task<IActionResult> Get([FromServices]IDataRepository context)
        {
            IEnumerable<dynamic> documents = await context.ReadDocuments();

            return Ok(documents);
        }
        
        /// <summary>
        /// Retrives a single document from the Cosmos DB instance
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /documents/00000000-0000-0000-0000-000000000000
        ///
        /// </remarks>
        /// <returns>A documents</returns>
        /// <response code="200">Returns a documents</response>
        /// <response code="400">If the id is invalid</response> 
        [HttpGet]
        [Route("[controller]/{id}")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get([FromServices]IDataRepository context, string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            dynamic document = await context.RetrieveDocument(id);

            return Ok(document);
        }
        
        /// <summary>
        /// Searches the documents in the Azure Search instance
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /documents/search/Amtas
        ///
        /// </remarks>
        /// <returns>Collection of search results</returns>
        /// <response code="200">Returns search results</response>
        /// <response code="400">If the search query is empty</response> 
        [HttpGet]
        [Route("[controller]/search")]
        [ProducesResponseType(typeof(IEnumerable<Person>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ApiExplorerSettings(GroupName = "v2")]
        public async Task<IActionResult> Search([FromServices]IDataRepository context, string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            dynamic document = await context.SearchDocuments(query);

            return Ok(document);
        }
    }
}