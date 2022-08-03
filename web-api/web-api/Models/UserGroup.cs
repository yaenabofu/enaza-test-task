using web_api.Enums;

namespace web_api.Models
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public UserGroupCode Code { get; set; }
        public string Description { get; set; }
    }
}
