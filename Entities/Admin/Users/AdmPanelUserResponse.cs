namespace Feipder.Entities.Admin.Users
{
    public class AdmPanelUserResponse
    {
        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset UpdateDate { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
