using BookFast.SeedWork.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace BookFast.Api.Swagger
{
    internal class SwaggerIgnoreSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            var properties = context.SystemType.GetProperties();
            foreach (var property in properties)
            {
                var ignoreAttrs = property.GetCustomAttributes(typeof(SwaggerIgnoreAttribute), true);
                if (ignoreAttrs != null && ignoreAttrs.Any())
                {
                    var propertyName = model.Properties
                        .Where(prop => prop.Key.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                        .Select(prop => prop.Key).FirstOrDefault();
                    if (propertyName != null)
                    {
                        model.Properties.Remove(propertyName); 
                    }
                }
            }
        }
    }
}
