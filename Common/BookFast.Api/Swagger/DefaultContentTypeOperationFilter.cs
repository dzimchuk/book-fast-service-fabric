using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookFast.Api.Swagger
{
    internal class DefaultContentTypeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Produces.Clear();
            operation.Produces.Add("application/json");
        }
    }
}