using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AR.XFSample.DependencyServices;
using Foundation;
using UIKit;
using AR.XFSample.iOS.Helpers;
using AR.XFSample.iOS.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(ArDependencyService))]
namespace AR.XFSample.iOS.DependencyServices
{
    public class ArDependencyService : IArDependencyService
    {
        public void LaunchAR(string arLaunchType = "")
        {
            ArViewControllerHelper arViewControllerHelper = new ArViewControllerHelper(arLaunchType);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(arViewControllerHelper, true, null);
        }
    }
}