using AR.XFSample.DependencyServices;
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

        private void ArStartButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IArDependencyService>().LaunchAR();
        }
    }
}
