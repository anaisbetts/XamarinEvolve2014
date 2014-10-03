using System;
using ReactiveUI;
using XamarinEvolve.Core.Models;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using Splat;
using Akavache;
using System.Net.Http;
using Fusillade;
using Refit;

namespace XamarinEvolve.Core.ViewModels
{
    [DataContract]
    public class ChannelViewModel : ReactiveObject, IRoutableViewModel
    {
        [IgnoreDataMember]
        public string UrlPathSegment {
            get { return "slack"; }
        }

        [IgnoreDataMember]
        public IScreen HostScreen { get; protected set; }

        [IgnoreDataMember] string visibleChannel;
        [DataMember] public string VisibleChannel {
            get { return visibleChannel; }
            set { this.RaiseAndSetIfChanged(ref visibleChannel, value); }
        }

        [IgnoreDataMember] public ReactiveCommand<string> LoadChannelToDisplay { get; protected set; }
        [IgnoreDataMember] public ReactiveCommand<MessageListForRoom> LoadMessages { get; protected set; }

        [DataMember] public ReactiveList<Message> Messages { get; protected set; }
        [DataMember] public IReactiveDerivedList<MessageTileViewModel> MessageTiles { get; protected set; }

        public ChannelViewModel(string token, string teamId, IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            Messages = new ReactiveList<Message>();

            var client = new HttpClient(NetCache.UserInitiated) {
                BaseAddress = new Uri("https://slack.com"),
            };

            var api = RestService.For<ISlackApi>(client);

            LoadChannelToDisplay = ReactiveCommand.CreateAsyncTask(async _ => {
                var channelList = await BlobCache.LocalMachine.GetOrFetchObject("channels_" + teamId, 
                    () => api.GetChannels(token),
                    RxApp.MainThreadScheduler.Now + TimeSpan.FromMinutes(5.0));

                // Try to find ReactiveUI, then general, then whatever the first one is
                var channel =
                    channelList.channels.FirstOrDefault(x => x.name.ToLowerInvariant() == "reactiveui") ??
                    channelList.channels.FirstOrDefault(x => x.name.ToLowerInvariant() == "general") ??
                    channelList.channels.First();

                return channel.id;
            });

            LoadChannelToDisplay.Subscribe(x => VisibleChannel = x);

            LoadMessages = ReactiveCommand.CreateAsyncTask(async _ => {
                var channelToLoad = VisibleChannel;
                if (channelToLoad == null) {
                    channelToLoad = await LoadChannelToDisplay.ExecuteAsync();
                }

                return await api.GetLatestMessages(token, channelToLoad);
            });

            LoadMessages.Subscribe(xs => {
                Messages.Clear();
                Messages.AddRange(xs.messages);
            });

            LoadMessages.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Couldn't load messages for room", ex));

            MessageTiles = Messages.CreateDerivedCollection(
                x => new MessageTileViewModel(x),
                x => !String.IsNullOrWhiteSpace(x.text),
                (x, y) => x.Model.date.CompareTo(y.Model.date));
        }
    }

    [DataContract]
    public class MessageTileViewModel : ReactiveObject
    {
        [DataMember]
        public Message Model { get; protected set; }

        public MessageTileViewModel(Message model) 
        {
            this.Model = model;
        }
    }
}

