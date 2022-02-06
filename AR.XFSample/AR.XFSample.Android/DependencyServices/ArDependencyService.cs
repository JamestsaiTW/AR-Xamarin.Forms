using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using AR.XFSample.DependencyServices;
using AR.XFSample.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ArDependencyService))]
namespace AR.XFSample.Droid.DependencyServices
{
    public class ArDependencyService : IArDependencyService
    {
        public void LaunchAR(string arLaunchType = "")
        {
            var intent = new Intent(Android.App.Application.Context, typeof(Helpers.ArActivityHelper));
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}