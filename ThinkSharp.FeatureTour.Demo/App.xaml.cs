using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using ThinkSharp.FeatureTouring.Logging;
using ThinkSharp.Logging;

namespace ThinkSharp.FeatureTouring.ExampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.SetLogger(new NullLogger());
            Log.SetLogger(new CustomConsoleLogger());
        }
    }
}
