using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TestScrollSet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Locator locator;

        private void OnStartap(object sender, StartupEventArgs e)
        {
            locator = (Locator)Resources["locator"];


            locator.WeightVM = new ViewModels.WeightVM();
            locator.StationsNavigationVM = new ViewModels.StationsNavigationVM();

            locator.Content = locator.StationsNavigationVM;
        }
    }
}
