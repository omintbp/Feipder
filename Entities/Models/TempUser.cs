namespace Feipder.Entities.Models
{
    public class TempUser
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsApproved { get; set; } = false;
        public string ApproveCode { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
