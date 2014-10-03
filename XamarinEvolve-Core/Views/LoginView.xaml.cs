using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ReactiveUI;
using XamarinEvolve.Core.ViewModels;

namespace XamarinEvolve.Core.Views
{    
    public partial class LoginView : ContentPage, IViewFor<LoginViewModel>
    {    
        public LoginView ()
        {
            InitializeComponent ();

            this.OneWayBind(ViewModel, vm => vm.Model.user, v => v.Email.Text);
            this.Bind(ViewModel, vm => vm.Password, v => v.Password.Text);

            this.BindCommand(ViewModel, vm => vm.Login, v => v.Login);
        }

        public LoginViewModel ViewModel {
            get { return (LoginViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create<LoginView, LoginViewModel>(x => x.ViewModel, default(LoginViewModel), BindingMode.OneWay);

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (LoginViewModel)value; }
        }
    }
}
