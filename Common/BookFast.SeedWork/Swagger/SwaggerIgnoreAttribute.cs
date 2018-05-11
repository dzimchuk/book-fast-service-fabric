using System;

namespace BookFast.SeedWork.Swagger
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SwaggerIgnoreAttribute : Attribute
    {
    }
}
