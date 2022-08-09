using System;
using web_api.Enums;

namespace web_api.Models
{
    public class RegisterModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public UserGroupCode UserGroup { get; set; }
    }
}
