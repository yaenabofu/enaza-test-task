using System;

namespace web_api.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserGroupId { get; set; }
        public int UserStateId { get; set; }
    }
}
