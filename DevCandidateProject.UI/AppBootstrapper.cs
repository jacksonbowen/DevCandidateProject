using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using DevCandidateProject.UI.ViewModels;

namespace DevCandidateProject.UI
{
  public class AppBootstrapper
    : BootstrapperBase
  {
    public AppBootstrapper()
    {
      Initialize();
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
      var settings = new Dictionary<string, object>
        {
          { "ResizeMode", ResizeMode.NoResize },
          //{ "SizeToContent", SizeToContent.Manual },
          //{ "WindowState" , WindowState.Maximized }
        };

      DisplayRootViewFor<RootViewModel>(settings);
    }
  }
}