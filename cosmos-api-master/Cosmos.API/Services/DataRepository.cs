using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cosmos.API.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;

namespace Cosmos.API.Services
{
    /// <summary>
    /// Repository class for database access
    /// </summary>
    public class DataRepository : IDataRepository
    {
        /// <summary>
        /// Azure Cosmos DB Settings
        /// </summary>
        protected CosmosSettings CosmosSettings { get; }

        /// <summary>
        /// Azure Search Settings
        /// </summary>
        protected SearchSettings SearchSettings { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DataRepository(IOptions<CosmosSettings> cosmosSettings, IOptions<SearchSettings> searchSettings)
        {
            CosmosSettings = cosmosSettings.Value;
            SearchSettings = searchSettings.Value;
        }

        /// <summary>
        /// Reads all documents
        /// </summary>
        public async Task<IEnumerable<dynamic>> ReadDocuments()
        {
            using(DocumentClient client = new DocumentClient(new Uri(CosmosSettings.EndpointUrl), CosmosSettings.AuthorizationKey))
            {
                Uri collectionLink = UriFactory.CreateDocumentCollectionUri(CosmosSettings.DatabaseId, CosmosSettings.CollectionId);

                List<dynamic> documents = new List<dynamic>();
                string continuationToken = null;
                do
                {
                    FeedResponse<dynamic> documentFeed = await client.ReadDocumentFeedAsync(collectionLink, new FeedOptions { MaxItemCount = 5, RequestContinuation = continuationToken });
                    continuationToken = documentFeed.ResponseContinuation;
                    foreach (var document in documentFeed)
                    {
                        documents.Add(document);
                    }
                }
                while (continuationToken != null);


                return documents;
            }
        }
        
        /// <summary>
        /// Retrieve a single documents
        /// </summary>
        public async Task<dynamic> RetrieveDocument(string id)
        {
            using(DocumentClient client = new DocumentClient(new Uri(CosmosSettings.EndpointUrl), CosmosSettings.AuthorizationKey))
            {
                Uri documentLink = UriFactory.CreateDocumentUri(CosmosSettings.DatabaseId, CosmosSettings.CollectionId, id);

                ResourceResponse<Microsoft.Azure.Documents.Document> response = await client.ReadDocumentAsync(documentLink);

                return response.Resource;
            }
        }
        
        /// <summary>
        /// Searches all documents
        /// </summary>
        public async Task<IEnumerable<dynamic>> SearchDocuments(string query)
        {
            using (SearchIndexClient client = new SearchIndexClient(SearchSettings.AccountName, SearchSettings.IndexId, new SearchCredentials(SearchSettings.QueryKey)))
            {
                SearchParameters parameters = new SearchParameters()
                {
                    Top = 1000
                };

                DocumentSearchResult<dynamic> results = await client.Documents.SearchAsync<dynamic>(query, parameters);
                return results.Results.Select(r => r.Document);
            }
        }

        /// <summary>
        /// Creates documents using standardized stored procedure
        /// </summary>
        public async Task<IEnumerable<dynamic>> GenerateDocuments(int count)
        {
            using(DocumentClient client = new DocumentClient(new Uri(CosmosSettings.EndpointUrl), CosmosSettings.AuthorizationKey))
            {
                Uri storedProcLink = UriFactory.CreateStoredProcedureUri(CosmosSettings.DatabaseId, CosmosSettings.CollectionId, CosmosSettings.StoredProcedureId);

                StoredProcedureResponse<IEnumerable<dynamic>> response = await client.ExecuteStoredProcedureAsync<IEnumerable<dynamic>>(storedProcLink, count);

                return response.Response;
            }
        }

        /// <summary>
        /// Creates a standardized stored procedure
        /// </summary>
        public async Task CreateGeneratorStoredProcedure()
        {
            using(DocumentClient client = new DocumentClient(new Uri(CosmosSettings.EndpointUrl), CosmosSettings.AuthorizationKey))
            {
                Uri collectionLink = UriFactory.CreateDocumentCollectionUri(CosmosSettings.DatabaseId, CosmosSettings.CollectionId);

                string scriptFileName = @"Scripts\genDocuments.js";
                string scriptId = CosmosSettings.StoredProcedureId;

                StoredProcedure sproc = new StoredProcedure
                {
                    Id = scriptId,
                    Body = System.IO.File.ReadAllText(scriptFileName)
                };

                StoredProcedure sprocRef = client.CreateStoredProcedureQuery(collectionLink).Where(s => s.Id == sproc.Id).AsEnumerable().FirstOrDefault();
                if (sprocRef != null)
                {
                    await client.DeleteStoredProcedureAsync(sprocRef.SelfLink);
                }

                sproc = await client.CreateStoredProcedureAsync(collectionLink, sproc);
            }          
        }
    }
}