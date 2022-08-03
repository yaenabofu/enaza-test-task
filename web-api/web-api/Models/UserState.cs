using web_api.Enums;

namespace web_api.Models
{
    public class UserState 
    {
        public int UserStateId { get; set; }
        public UserStateCode Code { get; set; }
        public string Description { get; set; }
    }
}
