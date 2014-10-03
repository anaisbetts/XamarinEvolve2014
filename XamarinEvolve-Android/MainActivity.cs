using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using XamarinEvolve.Core.ViewModels;
using Akavache;
using Xamarin.Forms.Platform.Android;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace XamarinEvolve.Views
{
    [Activity (Label = "XamarinEvolveDemo", MainLauncher = true)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);

            var bootstrapper = RxApp.SuspensionHost.GetAppState<AppBootstrapper>();
            this.SetPage(bootstrapper.CreateMainPage());
        }
    }
}
