using System;
using Android.App;
using Android.Runtime;
using ReactiveUI;
using XamarinEvolve.Core.ViewModels;

namespace XamarinEvolve
{
    [Application(Label = "XamarinEvolve-Android")]
    public class AndroidApplication : Application
    {
        AutoSuspendHelper autoSuspendHelper;
        public AndroidApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
                        
            autoSuspendHelper = new AutoSuspendHelper(this);
            RxApp.SuspensionHost.CreateNewAppState = () => {
                return new AppBootstrapper();
            };

            RxApp.SuspensionHost.SetupDefaultSuspendResume();
        }
    }
}

