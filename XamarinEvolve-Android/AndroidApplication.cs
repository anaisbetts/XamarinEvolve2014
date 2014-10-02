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
                        
            // CoolStuff: The job of AutoSuspendHelper is, that it will 
            // automatically save and reload exactly *one* object of your 
            // choice when the app is suspended. If the object can't be
            // reloaded (i.e. if the app is starting for the first time),
            // we're telling ReactiveUI here how to create a new one from
            // scratch.
            RxApp.SuspensionHost.CreateNewAppState = () => {
                return new AppBootstrapper();
            };

            autoSuspendHelper = new AutoSuspendHelper(this);
            RxApp.SuspensionHost.SetupDefaultSuspendResume();
        }
    }
}

