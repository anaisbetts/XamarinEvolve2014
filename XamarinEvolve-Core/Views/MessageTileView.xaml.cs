using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinEvolve.Core.ViewModels;
using ReactiveUI;

namespace XamarinEvolve.Core.Views
{
    public partial class MessageTileView : ContentView, IViewFor<MessageTileViewModel>
    {
        public MessageTileView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Model.user, v => v.User.Text);
            this.OneWayBind(ViewModel, vm => vm.Model.text, v => v.Message.Text);
        }
                
        public MessageTileViewModel ViewModel {
            get { return (MessageTileViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create<MessageTileView, MessageTileViewModel>(x => x.ViewModel, default(MessageTileViewModel), BindingMode.OneWay);

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (MessageTileViewModel)value; }
        }
    }
}