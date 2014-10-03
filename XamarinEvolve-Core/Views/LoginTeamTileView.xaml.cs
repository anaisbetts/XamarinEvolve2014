using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ReactiveUI;
using XamarinEvolve.Core.ViewModels;

namespace XamarinEvolve.Core.Views
{
    public partial class LoginTeamTileView : ContentView, IViewFor<LoginTeamTileViewModel>
    {
        public LoginTeamTileView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Model.team, v => v.LoginToThisTeam.Text);
            this.BindCommand(ViewModel, vm => vm.LoginToThisTeam, v => v.LoginToThisTeam);
        }

        public LoginTeamTileViewModel ViewModel {
            get { return (LoginTeamTileViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create<LoginTeamTileView, LoginTeamTileViewModel>(x => x.ViewModel, default(LoginTeamTileViewModel), BindingMode.OneWay);

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (LoginTeamTileViewModel)value; }
        }
    }
}
