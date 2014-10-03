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
