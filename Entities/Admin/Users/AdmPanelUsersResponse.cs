namespace Feipder.Entities.Admin.Users
{
    public class AdmPanelUsersResponse
    {
        public IEnumerable<AdmPanelUserResponse> Users { get; set; } = new List<AdmPanelUserResponse>();
    }
}
