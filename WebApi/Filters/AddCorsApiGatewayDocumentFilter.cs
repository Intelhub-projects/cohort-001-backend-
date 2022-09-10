/*using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Swagger;

namespace WebApi.Filters
{
    public class AddCorsApiGatewayDocumentFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
    {
        private Operation BuildCorsOptionOperation()
        {
            var response = new Response
            {
                description = "Successful operation",
                headers = new Dictionary<string, Header>
                {
                    { "Access-Control-Allow-Origin", new Header(){type="string",description="URI that may access the resource" } },
                    { "Access-Control-Allow-Methods", new Header(){type="string",description="Method or methods allowed when accessing the resource" } },
                    { "Access-Control-Allow-Headers", new Header(){type="string",description="Used in response to a preflight request to indicate which HTTP headers can be used when making the request." } },
                }
            };
            return new Operation
            {
                consumes = new List<string> { "application/json" },
                produces = new List<string> { "application/json" },
                responses = new Dictionary<string, Response> { { "200", response } }
            };
        }

        private object BuildApiGatewayIntegrationExtension()
        {
            return new
            {
                responses = new
                {
                    @default = new
                    {
                        statusCode = "200",
                        responseParameters = new Dictionary<string, string>
                            {
                                { "method.response.header.Access-Control-Allow-Methods", "'POST,GET,OPTIONS'" },
                                { "method.response.header.Access-Control-Allow-Headers", "'Content-Type,X-Amz-Date,Authorization,X-Api-Key'"},
                                { "method.response.header.Access-Control-Allow-Origin", "'*'"}
                            }
                    },
                },
                passthroughBehavior = "when_no_match",
                requestTemplates = new Dictionary<string, string> { { "application/json", "{\"statusCode\": 200}" } },
                type = "mock"
            };
        }

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.paths)
            {
                var corsOptionOperation = BuildCorsOptionOperation();
                var awsApiGatewayExtension = BuildApiGatewayIntegrationExtension();
                corsOptionOperation.vendorExtensions.Add("x-amazon-apigateway-integration", awsApiGatewayExtension);
                path.Value.options = corsOptionOperation;
            }
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}
*/