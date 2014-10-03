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

    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string created { get; set; }
        public string creator { get; set; }
        public bool is_archived { get; set; }
        public bool is_member { get; set; }
        public int num_members { get; set; }
    }

    public class ChannelList
    {
        public bool ok { get; set; }
        public List<Channel> channels { get; set; }
    }

    public class Message
    {
        public string type { get; set; }
        public string ts { get; set; }
        public string user { get; set; }
        public string text { get; set; }
        public bool? is_starred { get; set; }
        public bool? wibblr { get; set; }

        public DateTimeOffset date {
            get { return unixTimeStampToDateTime(Double.Parse(ts)); }
        }

        static DateTimeOffset unixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

            return new DateTimeOffset(dtDateTime);
        }
    }

    public class MessageListForRoom
    {
        public bool ok { get; set; }
        public string latest { get; set; }
        public List<Message> messages { get; set; }
        public bool has_more { get; set; }
    }
}
