namespace fintech.Models
{
    using System;
    using System.Collections.Generic;
    public class GetUserAccess_Result
    {
        public GetUserAccess_Result()
        {
            UserAccessDetail = new List<UserAccessDetailList>();
        }

        public List<UserAccessDetailList> UserAccessDetail { get; set; }
    }

    public class UserAccessDetailList
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string LastLoggedIn { get; set; }
        public int Records { get; set; }
    }
}