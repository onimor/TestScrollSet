using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TestScrollSet.Base;
using TestScrollSet.Dto;

namespace TestScrollSet.ViewModels
{
    public class StationVM:ViewModelBase
    {
        #region ТЕСТ Элд Хасп
        private double _vertical;
        ///<summary>вертикальный скрол</summary>
        public double Vertical
        {
            get => _vertical;
            set
            {
                Set(ref _vertical, value);
                if (_isVerticalSave && value == 0)
                {
                    RecoveryVerticalCroll();
                }

            }
        }
        private async void RecoveryVerticalCroll()
        {
            //await Task.Delay(1);
            _isVerticalSave = false;
            Vertical = _oldVertical;
        }


        private double _horizontal;
        ///<summary>горизонтальный скрол</summary>
        public double Horizontal
        {
            get => _horizontal;
            set
            {
                Set(ref _horizontal, value);
                if (_isHorizontalSave && value == 0)
                {
                    RecoveryHorizontalCroll();
                }
            }
        }
        private async void RecoveryHorizontalCroll()
        {
            //await Task.Delay(1);
            _isHorizontalSave = false;
            Horizontal = _oldHorizontal;
        }
        #endregion

        #region ТЕСТ proa33
        public string UniqId { get; set; }

        #endregion

        private ObservableCollection<TableWeighingLog> _weighingLogCollection;
        ///<summary>Камеры</summary>
        public ObservableCollection<TableWeighingLog> WeighingLogCollection
        {
            get => _weighingLogCollection;
            set => Set(ref _weighingLogCollection, value);
        }

        private TableWeighingLog _selctedWeighingLog;
        ///<summary>Камеры</summary>
        public TableWeighingLog SelctedWeighingLog
        {
            get => _selctedWeighingLog;
            set => Set(ref _selctedWeighingLog, value);
        }

        private double _oldVertical = 0;
        private double _oldHorizontal = 0;
        private bool _isVerticalSave = false;
        private bool _isHorizontalSave = false;
        public StationVM()
        {
            UniqId = System.IO.Path.GetRandomFileName();

            List<TableWeighingLog> weighingLogs = new();
            for (int i = 0; i < 10; i++)
            {
                weighingLogs.Add(new TableWeighingLog { Id = i, PostName = $"Name{i}" });
            }
            WeighingLogCollection = new ObservableCollection<TableWeighingLog>(weighingLogs);
        }

     
        //для теста записываем что было и переходим
        public void GoWeight()
        {
            _oldVertical = Vertical;
            _oldHorizontal = Horizontal;
            _isVerticalSave = true;
            _isHorizontalSave = true;
            App.locator.GoWeight();
        }

        private ICommand _goWeightCommand;
        public ICommand GoWeightCommand => _goWeightCommand ??= new RelayCommand(GoWeight);
    }
}
