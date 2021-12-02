using System;
using System.Collections.Generic;
using System.Text;

namespace AR.XFSample.DependencyServices
{
    public interface IArDependencyService
    {
        void LaunchAR(string arLaunchType = "");
    }
}
