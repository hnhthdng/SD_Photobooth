using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ServerAPI.Filters
{
    public class ResponseTimeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("200"))
            {
                operation.Responses["200"].Headers.Add("X-Response-Time-ms", new OpenApiHeader
                {
                    Description = "Response time in milliseconds",
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}
