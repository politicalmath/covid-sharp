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
using CovidSharpUi.Enums;
using CovidSharpUi.Models;
using Windows.Storage;
using System.IO;
using static CovidSharp.Utils.CsvUtility;
using System.Globalization;

namespace CovidSharpUi.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private CsvUtility csvWriter { get; set; }
        public List<State> StateData { get; set; }
        private CovidTrackService ctService { get; set; }
        private List<ProcessedState> ProcessedStateData { get; set; }


        public bool UseCovidDataSource { get; set; }
        public bool UseNytDataSource { get; set; }
        public bool IsCollectingData { get; set; }
        private bool isDataLoaded { get; set; }
        private bool isDataProcessed {get; set;}

        public string StateSelectionString { get; set; }
        public List<StateBase> SelectedStates { get; set; } 

        public bool IncludeDeaths { get; set; }
        public bool IncludeCases { get; set; }
        public bool IncludeTests { get; set; }
        public bool IncludePercentPositive { get; set; }
        public bool IncludeNewHospitalizations { get; set; }
        public bool IncludeCurrentHospitalizations { get; set; }

        public bool UseDailyData { get; set; }
        public bool UseCumeData { get; set; }
        public bool ProcessPerCapita { get; set; }
        public bool ProcessRollingAverage { get; set; }
        public string RollingAverageNumber { get; set; }

        public string ExportFolder { get; set; }

        private string _status { get; set; }
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        public RelayCommand GetDataCommand { get; private set; }
        public RelayCommand ParseDataCommand { get; private set; }

        public RelayCommand PickExportFolderCommand { get; private set; }
        public RelayCommand ExportDataCommand { get; private set; }

        public MainViewModel()
        {
            csvWriter = new CsvUtility();
            StateData = new List<State>();

            ProcessedStateData = new List<ProcessedState>();
            ctService = new CovidTrackService();
            UseCovidDataSource = true;
            isDataLoaded = false;
            isDataProcessed = false;
            Status = "Waiting";

            IncludeDeaths = true;
            IncludeCases = true;
            IncludePercentPositive = true;
            IncludeTests = true;
            ProcessPerCapita = true;
            ProcessRollingAverage = true;
            UseDailyData = false;
            
            GetDataCommand = new RelayCommand(new Action(GetData), CanExecuteGetDataCommand);
            ParseDataCommand = new RelayCommand(new Action(ProcessData), CanExecuteProcessDataCommand);
            PickExportFolderCommand = new RelayCommand(new Action(PickExportFolder), CanExecuteExportDataCommand);
            ExportDataCommand = new RelayCommand(new Action(ExportData), CanExecuteExportDataCommand);
        }
        
        private bool CanExecuteGetDataCommand()
        {
            return true;
        }

        public async void GetData()
        {

            Status = "Loading data...";
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
            isDataLoaded = true;

            Status = "Data Loaded!";
        }

        private bool CanExecuteProcessDataCommand()
        {
            return true;
        }

        public void ProcessData()
        {
            Status = "Processing data...";
            ProcessedStateData.Clear();
            List<Metrics> metrics = new List<Metrics>();
            if (IncludeDeaths) metrics.Add(Metrics.Deaths);
            if (IncludeCases) metrics.Add(Metrics.Cases);
            if (IncludeTests) metrics.Add(Metrics.Tests);
            if (IncludePercentPositive) metrics.Add(Metrics.PercentPositive);
            if (IncludeCurrentHospitalizations) metrics.Add(Metrics.HospitalCurrent);
            if (IncludeNewHospitalizations) metrics.Add(Metrics.HospitalNew);

            foreach (State s in StateData) {
                var processedState = new ProcessedState(s);
                if (UseDailyData) {
                    int rollingAvg = 7;
                    var parseSuccess= Int32.TryParse(RollingAverageNumber, out rollingAvg);
                    if (rollingAvg == 0) rollingAvg = 7;
                    processedState.ProcessDailyData(metrics, ProcessRollingAverage, rollingAvg, ProcessPerCapita);
                } else if (UseCumeData)
                {
                    processedState.ProcessCumulativeData(metrics, ProcessPerCapita);
                }

                ProcessedStateData.Add(processedState);
            }

            isDataProcessed = true;

            Status = "Data Processed!";
        }

        private bool CanPickExportFolder()
        {
            return true;
        }

        public async void PickExportFolder()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            var folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("ExportFolder", folder);
            }
        }

        private bool CanExecuteExportDataCommand()
        {
            return true;
        }

        public async void ExportData()
        {
            Status = "Exporting Data...";

            var folder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync("ExportFolder");

            //  https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-using-file-and-folder-pickers

            DateTime startDate = new DateTime(2020, 3, 1);
            var listOfFiles = ProcessedStateData?.FirstOrDefault()?.OutputFiles;
            if (folder == null || listOfFiles == null) return;

            foreach (KeyValuePair<string, List<CalculatedValue>> kvp in listOfFiles)
            {
                DateTime currentDate = startDate;

                DateTime latestDate = listOfFiles.Values.FirstOrDefault().Max(cv => cv.Date);

                StorageFile csvFile = await folder?.CreateFileAsync(kvp.Key + ".csv", Windows.Storage.CreationCollisionOption.ReplaceExisting);

                using (CsvFileWriter dataWriter = new CsvFileWriter(await csvFile.OpenStreamForWriteAsync()))
                {
                    CsvRow headerRow = new CsvRow();
                    headerRow.Add("Date");
                    foreach (ProcessedState ps in ProcessedStateData)
                        headerRow.Add(ps.CoreStateData.StateBase.Code.ToString());

                    dataWriter.WriteRow(headerRow);

                    while(currentDate.Date <= latestDate)
                    {                        
                        CsvRow nextRow = new CsvRow();
                        nextRow.Add(currentDate.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                        foreach(ProcessedState ps in ProcessedStateData)
                        {
                            var calcVals = ps.OutputFiles[kvp.Key].FirstOrDefault(calcv => calcv.Date.Date == currentDate.Date);
                            if (calcVals == null)
                                nextRow.Add("0");
                            else
                            {
                                nextRow.Add(calcVals.Value.ToString("F3", CultureInfo.InvariantCulture));
                            }
                        }
                        dataWriter.WriteRow(nextRow);
                        currentDate = currentDate + new TimeSpan(1, 0, 0, 0);
                    }
                }
            }

            Status = "Data Exported!";

        }

    }

}
