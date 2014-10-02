using System;
using System.Collections.Generic;

// CoolStuff: You don't have to actually write this code at all, just go to
// json2csharp.com
namespace XamarinEvolve.Core.Models
{
    public class TeamWithUser
    {
        public string url { get; set; }
        public string team { get; set; }
        public string user { get; set; }
        public string team_id { get; set; }
        public string user_id { get; set; }
    }

    public class AvailableTeamList
    {
        public bool ok { get; set; }
        public string email { get; set; }
        public string domain { get; set; }
        public List<TeamWithUser> users { get; set; }
        public string create { get; set; }
    }

    public class AuthenticationResult
    {
        public bool ok { get; set; }
        public string token { get; set; }
        public string user { get; set; }
        public string team { get; set; }
    }
}
