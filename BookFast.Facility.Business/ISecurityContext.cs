namespace BookFast.Facility.Business
{
    public interface ISecurityContext
    {
        string GetCurrentUser();
        string GetCurrentTenant();
    }
}