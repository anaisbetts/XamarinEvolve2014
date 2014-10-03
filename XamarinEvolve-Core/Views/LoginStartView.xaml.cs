using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ReactiveUI;
using XamarinEvolve.Core.ViewModels;

namespace XamarinEvolve.Core.Views
{    
    public partial class LoginStartView : ContentPage, IViewFor<LoginStartViewModel>
    {    
        public LoginStartView ()
        {
            InitializeComponent ();

            this.OneWayBind(ViewModel, vm => vm.TeamList, v => v.TeamList.ItemsSource);
            this.Bind(ViewModel, vm => vm.Email, v => v.Email.Text);
        }
        
        public LoginStartViewModel ViewModel {
            get { return (LoginStartViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly BindableProperty ViewModelProperty =
            BindableProperty.Create<LoginStartView, LoginStartViewModel>(x => x.ViewModel, default(LoginStartViewModel), BindingMode.OneWay);

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (LoginStartViewModel)value; }
        }
    }
}

