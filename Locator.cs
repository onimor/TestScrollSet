using System.Windows.Input;
using TestScrollSet.Base;
using TestScrollSet.ViewModels;

namespace TestScrollSet
{
    public class Locator : ViewModelBase
    {

        private WeightVM _weightVM;
        public WeightVM WeightVM { get => _weightVM; set => Set(ref _weightVM, value); }


        private StationsNavigationVM _stationsNavigationVM;
        public StationsNavigationVM StationsNavigationVM { get => _stationsNavigationVM; set => Set(ref _stationsNavigationVM, value); }

        private object _content;
        public object Content { get => _content; set => Set(ref _content, value); }


        public void GoWeight()
        {

            Content = WeightVM;
        }

        private void GoStationsNavigation()
        {

            Content = StationsNavigationVM;
        }


        private ICommand _goWeightCommand;
        public ICommand GoWeightCommand => _goWeightCommand ??= new RelayCommand(GoWeight);

        private ICommand _goStationsNavigationCommand;
        public ICommand GoStationsNavigationCommand => _goStationsNavigationCommand ??= new RelayCommand(GoStationsNavigation);
    }
}
