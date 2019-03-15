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
        public void LaunchAR()
        {
            ArViewControllerHelper aRviewControllerHelper = new ArViewControllerHelper();
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(aRviewControllerHelper, true, null);
        }
    }
}