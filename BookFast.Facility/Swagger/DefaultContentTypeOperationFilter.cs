using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace BookFast.Facility.Swagger
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