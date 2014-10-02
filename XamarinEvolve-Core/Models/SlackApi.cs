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
    }
}