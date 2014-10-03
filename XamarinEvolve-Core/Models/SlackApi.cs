using System;
using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XamarinEvolve.Core.Models
{
    public interface ISlackApi 
    {
        [Post("/api/auth.start")]
        Task<AvailableTeamList> GetTeamsForUserRaw([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> form);

        [Post("/api/auth.signin")]
        Task<AuthenticationResult> LoginRaw([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> form);

        [Post("/api/channels.list")]
        Task<ChannelList> GetChannelsRaw([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> form);
                
        [Post("/api/channels.history")]
        Task<MessageListForRoom> GetMessagesForChannelRaw([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> form);
    }

    public static class SlackApiExtensions
    {
        public static Task<AvailableTeamList> GetTeamsForUser(this ISlackApi This, string email)
        {
            return This.GetTeamsForUserRaw(new Dictionary<string, string> { { "email", email } });
        }

        public static Task<AuthenticationResult> Login(this ISlackApi This, string userId, string teamId, string password)
        {
            var dict = new Dictionary<string, string> {
                { "user", userId },
                { "team", teamId },
                { "password", password },
            };

            return This.LoginRaw(dict);
        }

        public static async Task<ChannelList> GetChannels(this ISlackApi This, string token)
        {
            var dict = new Dictionary<string, string> {
                { "token", token },
                { "exclude_archived", "1" },
            };

            var ret = await This.GetChannelsRaw(dict);

            if (!ret.ok || ret.channels == null || ret.channels.Count == 0) {
                // NB: Slack guarantees that there is always at least one 
                // channel
                throw new Exception("Retrieving channels failed");
            }

            return ret;
        }

        public static async Task<MessageListForRoom> GetLatestMessages(this ISlackApi This, string token, string channelId)
        {
            var dict = new Dictionary<string, string> {
                { "token", token },
                { "channel", channelId },
            };

            var ret = await This.GetMessagesForChannelRaw(dict);

            if (!ret.ok || ret.messages == null) {
                throw new Exception("Can't load messages for " + channelId);
            }

            return ret;
        }
    }
}