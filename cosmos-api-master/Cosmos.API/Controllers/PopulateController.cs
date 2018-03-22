using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmos.API.Models;
using Cosmos.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Cosmos.API.Controllers
{
    /// <summary>
    /// Controller used to populate Cosmos DB instance
    /// </summary>
    [Produces("application/json")]
    public class PopulateController : Controller
    {
        /// <summary>
        /// Populates the Cosmos DB instance
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /populate
        ///     {
        ///        "quantity": 5
        ///     }
        ///
        /// </remarks>
        /// <param name="context">ICosmosRepository context</param>
        /// <param name="options">Population options</param>
        /// <returns>Newly-created items</returns>
        /// <response code="201">Returns the newly-created items</response>
        /// <response code="400">If the payload is null</response> 
        [HttpPost]
        [Route("[controller]")]
        [ProducesResponseType(typeof(IEnumerable<Person>), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromServices]IDataRepository context, [FromBody]PopulateOptions options)
        {
            if (options == null)
            {
                return BadRequest();
            }

            await context.CreateGeneratorStoredProcedure();

            dynamic result = await context.GenerateDocuments(options.Quantity);

            return CreatedAtAction("Get", "Documents", new { }, result);
        }
    }
}
