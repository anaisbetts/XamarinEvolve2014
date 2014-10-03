using System;
using ReactiveUI;
using XamarinEvolve.Core.Models;
using System.Runtime.Serialization;
using Splat;
using Akavache;
using Refit;
using Fusillade;
using System.Net.Http;

namespace XamarinEvolve.Core.ViewModels
{
    [DataContract]
    public class LoginViewModel : ReactiveObject, IRoutableViewModel
    {
        [IgnoreDataMember]
        public string UrlPathSegment {
            get { return "Login to " + Model.team; }
        }

        [IgnoreDataMember]
        public IScreen HostScreen { get; protected set; }

        [DataMember]
        public TeamWithUser Model { get; protected set; }

        [IgnoreDataMember]
        public ReactiveCommand<string> Login { get; protected set; }

        [IgnoreDataMember] string password;
        [IgnoreDataMember] public string Password {
            get { return password; }
            set { this.RaiseAndSetIfChanged(ref password, value); }
        }

        public LoginViewModel(TeamWithUser model, IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            Model = model;

            var canLogin = this.WhenAny(x => x.Password, x => !String.IsNullOrWhiteSpace(x.Value));

            Login = ReactiveCommand.CreateAsyncTask(canLogin, async _ => {
                // CoolStuff: Here, we're using Fusillade to set up our
                // HttpClient. We're indicating that this operation is user-initiated,
                // so that it will take priority over any background operations.
                var client = new HttpClient(NetCache.UserInitiated) {
                    BaseAddress = new Uri("https://slack.com"),
                };

                // CoolStuff: We're using Refit here to auto-implement our Slack API
                // client.
                var api = RestService.For<ISlackApi>(client);
                var result = await api.Login(Model.user_id, Model.team_id, Password);

                // CoolStuff: We'll handle this exception in ThrownExceptions when
                // we throw a UserError
                if (!result.ok) {
                    throw new Exception("Result not ok!");
                }

                return result.token;
            });

            Login.ThrownExceptions.Subscribe(ex => {
                // CoolStuff: UserErrors are like "exceptions meant for users".
                // We can throw them in ViewModels, and let Views handle them by
                // displaying UI. We're being lazy here, but UserErrors are a
                // great way to ensure your users have a great app experience
                // when things go wrong
                UserError.Throw("Couldn't log in - check your password", ex);
            });
        }
    }
}

