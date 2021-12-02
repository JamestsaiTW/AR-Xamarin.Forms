﻿using AR.XFSample.DependencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AR.XFSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ArStartButton_Clicked(object sender, EventArgs e)
        {
            var arLaunchType =  await DisplayActionSheet("Start AR", "Cancel", "You can back to this View, when your iOS Device is Portrait!", new []{ "At One Point", "Normal Fly", "Cycle Fly", "Rotate Fly", "Crash Fly" });
            DependencyService.Get<IArDependencyService>().LaunchAR(arLaunchType);
        }
    }
}
