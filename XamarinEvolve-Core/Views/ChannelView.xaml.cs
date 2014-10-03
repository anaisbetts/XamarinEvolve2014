using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Xamarin.Forms;
using ReactiveUI;
using XamarinEvolve.Core.ViewModels;

namespace XamarinEvolve.Core.Views
{    
    public partial class ChannelView : ContentPage, IViewFor<ChannelViewModel>
    {    
        public ChannelView ()
        {
            InitializeComponent ();

            this.OneWayBind(ViewModel, vm => vm.MessageTiles, v => v.MessageTiles.ItemsSource);

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(_ => Observable.Timer(DateTimeOffset.MinValue, TimeSpan.FromMinutes(1), RxApp.MainThreadScheduler))
                .Switch()
                .InvokeCommand(this, x => x.ViewModel.LoadMessages);
        }

        public ChannelViewModel ViewModel {
            get { return (ChannelViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create<ChannelView, ChannelViewModel>(x => x.ViewModel, default(ChannelViewModel), BindingMode.OneWay);

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (ChannelViewModel)value; }
        }
    }
}

