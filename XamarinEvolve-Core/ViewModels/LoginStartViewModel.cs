using System;
using ReactiveUI;
using System.Runtime.Serialization;
using XamarinEvolve.Core.Models;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Splat;
using System.Net.Http;
using Fusillade;
using Refit;
using System.Collections.Generic;
using Akavache;

namespace XamarinEvolve.Core.ViewModels
{
    // CoolStuff: We're making sure all of our classes that are hosted can be
    // properly serialized. If we do this right, we'll get app lifecycle
    // serialization for free
    [DataContract]
    public class LoginStartViewModel : ReactiveObject, IRoutableViewModel
    {
        // CoolStuff: This badly-named parameter ends up being the Title on
        // iOS UINavigationViewController and Android ActionBar
        [IgnoreDataMember]
        public string UrlPathSegment {
            get { return "Login"; }
        }

        // Not serializing the HostScreen is very important, because you
        // will create a loop in the object graph => crash
        [IgnoreDataMember]
        public IScreen HostScreen { get; protected set; }


        // CoolStuff: All read-write properties in ReactiveUI are declared
        // in exactly this fashion. You never write custom code in the setter,
        // if you are you're Doing It Wrong™.
        [IgnoreDataMember] string email;
        [DataMember] public string Email {
            get { return email; }
            set { this.RaiseAndSetIfChanged(ref email, value); }
        }

        [IgnoreDataMember]
        public ReactiveCommand<List<TeamWithUser>> LoadTeamList { get; protected set; }

        [DataMember]
        public ReactiveList<LoginTeamTileViewModel> TeamList { get; protected set; }

        public LoginStartViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            TeamList = new ReactiveList<LoginTeamTileViewModel>();

            // CoolStuff: We're describing here, in a *declarative way*, the
            // conditions in which the LoadTeamList command is enabled. Now,
            // our Command IsEnabled is perfectly efficient, because we're only
            // updating the UI in the scenario when it should change.
            var canLoadTeamList = this.WhenAny(x => x.Email, x => !String.IsNullOrWhiteSpace(x.Value));

            // CoolStuff: ReactiveCommands have built-in support for background
            // operations. RxCmd guarantees that this block will only run exactly
            // once at a time, and that the CanExecute will auto-disable while it
            // is running.
            LoadTeamList = ReactiveCommand.CreateAsyncTask(canLoadTeamList, async _ => {
                var client = new HttpClient(NetCache.UserInitiated) {
                    BaseAddress = new Uri("https://slack.com"),
                };

                var api = RestService.For<ISlackApi>(client);
                var ret = await BlobCache.LocalMachine.GetOrFetchObject("teams_" + email,
                    async () => {
                        var teams = await api.GetTeamsForUser(this.Email);

                        if (teams.users == null || teams.users.Count == 0) {
                            throw new Exception("No teams for this account");
                        }

                        return teams;
                    },
                    RxApp.MainThreadScheduler.Now + TimeSpan.FromMinutes(5));

                return ret.users;
            });

            // CoolStuff: ReactiveCommands are themselves IObservables, whose value
            // are the results from the async method, guaranteed to arrive on the UI
            // thread. We're going to take the list of teams that the background
            // operation loaded, and put them into our TeamList.
            LoadTeamList.Subscribe(xs => {
                TeamList.Clear();
                TeamList.AddRange(xs.Select(x => new LoginTeamTileViewModel(x)));
            });

            // CoolStuff: ThrownExceptions is any exception thrown from the
            // CreateAsyncTask piped to this Observable. Subscribing to this
            // allows you to handle errors on the UI thread.
            LoadTeamList.ThrownExceptions
                .Subscribe(ex => {
                    TeamList.Clear();
                    UserError.Throw("Invalid Email for User", ex);
                });

            // CoolStuff: Whenever the Email address changes, we're going to wait
            // for one second of "dead airtime", then invoke the LoadTeamList
            // command.
            this.WhenAnyValue(x => x.Email)
                .Throttle(TimeSpan.FromSeconds(1), RxApp.MainThreadScheduler)
                .InvokeCommand(this, x => x.LoadTeamList);
        }
    }

    [DataContract]
    public class LoginTeamTileViewModel : ReactiveObject
    {
        [DataMember]
        public TeamWithUser Model { get; protected set; }

        public ReactiveCommand<Object> LoginToThisTeam { get; protected set; }

        public LoginTeamTileViewModel(TeamWithUser model, IScreen hostScreen = null)
        {
            hostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

            Model = model;

            // CoolStuff: Here, we're creating a Command whose sole job is to
            // use the Router to navigate us to a new page.
            LoginToThisTeam = ReactiveCommand.CreateAsyncObservable(_ =>
                hostScreen.Router.Navigate.ExecuteAsync(new LoginStartViewModel(hostScreen)));
        }
    }
}
