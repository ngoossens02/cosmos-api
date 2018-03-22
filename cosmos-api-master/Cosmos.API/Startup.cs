using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cosmos.API.Models;
using Cosmos.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Cosmos.API
{
    /// <summary>
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddCors();
            
            services.AddOptions();

            services.Configure<CosmosSettings>(Configuration.GetSection("CosmosDB"));
            services.Configure<SearchSettings>(Configuration.GetSection("Search"));

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddScoped<IDataRepository, DataRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Cosmos DB API",
                    Version = "v1.0.0",
                    Description = "Azure API App to connect to Cosmos DB",
                    License = new License { Name = "MIT", Url = "https://github.com/azure-labs/cosmos-api/blob/master/LICENSE" }
                });

                c.SwaggerDoc("v2", new Info
                {
                    Title = "Cosmos DB & Azure Search API",
                    Version = "v2.0.0",
                    Description = "Azure API App to connect to Cosmos DB",
                    License = new License { Name = "MIT", Url = "https://github.com/azure-labs/cosmos-api/blob/master/LICENSE" }
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            });

            app.UseDeveloperExceptionPage();

            var indexSettings = new IndexSettings();
            indexSettings.JSConfig.SwaggerEndpoints.Add(new EndpointDescriptor()
            {
                Url = "/v1/swagger.json",
                Description = "Cosmos DB API v1.0.0"
            });
            indexSettings.JSConfig.SwaggerEndpoints.Add(new EndpointDescriptor()
            {
                Url = "/v2/swagger.json",
                Description = "Cosmos DB API v2.0.0"
            });

            var fileServerOptions = new FileServerOptions()
            {
                FileProvider = new SwaggerUIFileProvider(indexSettings.ToTemplateParameters()),
                EnableDefaultFiles = true
            };

            fileServerOptions.StaticFileOptions.ContentTypeProvider = new FileExtensionContentTypeProvider();
            app.UseFileServer(fileServerOptions);

            app.UseMvc();
        }
    }
}
