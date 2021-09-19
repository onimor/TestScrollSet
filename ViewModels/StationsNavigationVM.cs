using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TestScrollSet.Base;

namespace TestScrollSet.ViewModels
{
    public class StationsNavigationVM:ViewModelBase
    {
        private ObservableCollection<StationVM> _displayedCollectionStationVM;
        ///<summary>Коллекция постов взвешивания для представления</summary>
        public ObservableCollection<StationVM> DisplayedCollectionStationVM
        {
            get => _displayedCollectionStationVM;
            set => Set(ref _displayedCollectionStationVM, value);
        }


        public StationsNavigationVM()
        {
            List<StationVM> stations = new();
            for (int i = 0; i < 3; i++)
            {
                stations.Add(new StationVM());
            }
            SelectStation(stations.FirstOrDefault());
        }

        /// <summary>Выбрать все посты для представления</summary>
        private void SelectAllStation()
        {

           
        }

        /// <summary>Выбрать только 1 пост для представления </summary>
        /// <param name="station">Выбранный пост</param>
        private void SelectStation(StationVM station)
        {

            DisplayedCollectionStationVM = new ObservableCollection<StationVM>(new List<StationVM> { station });
        }
    }
}
