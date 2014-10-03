using System;
using ReactiveUI;
using Splat;
using XamarinEvolve.Core.Views;
using Xamarin.Forms;
using ReactiveUI.XamForms;
using ModernHttpClient;
using System.Net.Http;
using Akavache;

namespace XamarinEvolve.Core.ViewModels
{
    // CoolStuff: This class and anything under it will automatically get
    // saved and restored by ReactiveUI. This is a great place to put all
    // of your startup code - think of it as the "ViewModel for your app".
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        // The Router holds the ViewModels for the back stack. Because it's
        // in this object, it will be serialized automatically.
        public RoutingState Router { get; protected set; }

        public AppBootstrapper()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            // Set up Akavache
            // 
            // Akavache is a portable data serialization library that we'll use to
            // cache data that we've downloaded
            BlobCache.ApplicationName = "XamarinEvolveDemo";

            // Set up Fusillade
            //
            // Fusillade is a super cool library that will make it so that whenever
            // we issue web requests, we'll only issue 4 concurrently, and if we
            // end up issuing multiple requests to the same resource, it will
            // de-dupe them. We're saying here, that we want our *backing*
            // HttpMessageHandler to be ModernHttpClient.
            Locator.CurrentMutable.RegisterConstant(new NativeMessageHandler(), typeof(HttpMessageHandler));

            // CoolStuff: For routing to work, we need to tell ReactiveUI how to
            // create the Views associated with our ViewModels
            Locator.CurrentMutable.Register(() => new LoginStartView(), typeof(IViewFor<LoginStartViewModel>));
            Locator.CurrentMutable.Register(() => new LoginView(), typeof(IViewFor<LoginViewModel>));
            Locator.CurrentMutable.Register(() => new ChannelView(), typeof(IViewFor<ChannelViewModel>));

            // Kick off to the first page of our app. If we don't navigate to a
            // page on startup, Xamarin Forms will get real mad (and even if it
            // didn't, our users would!)
            Router.Navigate.Execute(new LoginStartViewModel(this));
        }

        public Page CreateMainPage()
        {
            // NB: This returns the opening page that the platform-specific
            // boilerplate code will look for. It will know to find us because
            // we've registered our AppBootstrapper as an IScreen.
            return new RoutedViewHost();
        }
    }
}

