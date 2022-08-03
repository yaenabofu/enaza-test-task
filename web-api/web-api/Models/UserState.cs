using web_api.Enums;

namespace web_api.Models
{
    public class UserState : BaseDescription
    {
        public int UserStateId { get; set; }
        public UserStateCode Code { get; set; }
    }
}
