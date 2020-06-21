using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovidSharp.CovidTrack;
using CovidSharp.Models;
using CovidSharp.Utils;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using CovidSharp.CovidTrack.Models;
using CovidSharp.Constants;

namespace CovidSharpUi.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private CsvUtility csvWriter { get; set; }
        public List<State> StateData { get; set; }
        private CovidTrackService ctService { get; set; }



        public bool UseCovidDataSource { get; set; }
        public bool UseNytDataSource { get; set; }
        public bool IsCollectingData { get; set; }

        public string StateSelectionString { get; set; }
        public List<StateBase> SelectedStates { get; set; } 

        public bool IncludeDeaths { get; set; }
        public bool IncludeCases { get; set; }
        public bool IncludeTests { get; set; }
        public bool IncludePercentPositive { get; set; }
        public bool IncludeNewHospitalizations { get; set; }
        public bool IncludeCurrentHospitalizations { get; set; }

        public bool UseDailyData { get; set; }
        public bool ProcessPerCapita { get; set; }
        public bool ProcessRollingAverage { get; set; }
        public string RollingAverageNumber { get; set; }

        public string ExportFolder { get; set; }

        public RelayCommand GetDataCommand { get; private set; }
        public RelayCommand ParseDataCommand { get; private set; }
        public RelayCommand ExportDataCommand { get; private set; }

        public MainViewModel()
        {
            csvWriter = new CsvUtility();
            StateData = new List<State>();
            ctService = new CovidTrackService();
            UseCovidDataSource = true;

            GetDataCommand = new RelayCommand(new Action(GetData), CanExecuteGetDataCommand);
            ParseDataCommand = new RelayCommand(new Action(ParseData), CanExecuteParseDataCommand);
            ExportDataCommand = new RelayCommand(new Action(ExportData), CanExecuteExportDataCommand);
        }
        
        private bool CanExecuteGetDataCommand()
        {
            return true;
        }

        public async void GetData()
        {
            IsCollectingData = true;

            StateData.Clear();

            var rawStateData = await ctService.GetHistoricStateData();
            var stateBaseInfo = StatesConstants.GetStatesList();
            foreach(StateBase sb in stateBaseInfo)
            {
                var newState = new State(sb);
                newState.CovidData = rawStateData.FindAll(sd => sd.State.ToString().ToLower() == newState.StateBase.Code.ToString().ToLower());
                StateData.Add(newState);
            }

            IsCollectingData = false;
        }

        private bool CanExecuteParseDataCommand()
        {
            return true;
        }

        public void ParseData()
        {

        }

        private bool CanExecuteExportDataCommand()
        {
            return true;
        }

        public void ExportData()
        {

        }

    }

}
