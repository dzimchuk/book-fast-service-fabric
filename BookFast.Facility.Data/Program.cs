namespace BookFast.Facility.Data
{
    public class Program
    {
        public static void Main()
        {
            // Ugly! But as of today EF Core tooling requires that the startup project be an executable one
            // And it needs the startup project to reference Microsoft.EntityFrameworkCore.Design (but we don't want EF stuff referenced from the web api host project)
            // see https://stackoverflow.com/questions/44430963/is-ef-core-add-migration-supported-from-net-standard-library
        }
    }
}