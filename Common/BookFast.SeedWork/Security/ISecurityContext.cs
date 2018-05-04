namespace BookFast.Security
{
    public interface ISecurityContext
    {
        string GetCurrentUser();
        string GetCurrentTenant();
    }
}