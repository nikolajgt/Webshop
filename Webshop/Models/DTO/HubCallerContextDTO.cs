namespace Webshop.Models.DTO
{
    public class HubCallerContextDTO
    {
        public string? ConnectionID { get; set; }
        public string? UserID { get; set; }

        public HubCallerContextDTO(string connectionid, string userid)
        {
            ConnectionID = connectionid;
            UserID = userid;
        }
    }
}
