﻿using System;
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
using CovidSharp.owiData.Models;
using CovidSharp.owiData;

namespace CovidSharpUi.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private CsvUtility csvWriter { get; set; }
        public List<State> StateData { get; set; }
        public List<OwidCountry> CountryData { get; set; }
        private CovidTrackService ctService { get; set; }
        private OwidService owidService { get; set; }
        private List<ProcessedState> ProcessedStateData { get; set; }
        private List<ProcessedCountry> ProcessedCountryData { get; set; }


        public bool UseCovidDataSource { get; set; }
        public bool UseNytDataSource { get; set; }
        public bool UseOwidSource { get; set; }
        public bool IsCollectingData { get; set; }
        private bool isDataLoaded { get; set; }
        private bool isDataProcessed { get; set; }

        public bool SortStatesByRegion { get; set; }
        public bool SortCountriesByContinent { get; set; }
        public bool FilterSmallCountries { get; set; }

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
            CountryData = new List<OwidCountry>();

            ProcessedStateData = new List<ProcessedState>();
            ProcessedCountryData = new List<ProcessedCountry>();

            ctService = new CovidTrackService();
            owidService = new OwidService();
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

            if (UseCovidDataSource)
            {

                StateData.Clear();
                var rawStateData = await ctService.GetHistoricStateData();
                var stateBaseInfo = StatesConstants.GetStatesList();
                foreach (StateBase sb in stateBaseInfo)
                {
                    var newState = new State(sb);
                    newState.CovidData = rawStateData.FindAll(sd => sd.State.ToString().ToLower() == newState.StateBase.Code.ToString().ToLower());
                    StateData.Add(newState);
                }

                if (SortStatesByRegion)
                {
                    StateData = PerformStateSortByRegion(StateData);
                }

            }
            else if (UseOwidSource)
            {
                CountryData.Clear();
                CountryData = await owidService.GetAllWorldData();

                CountryData = CountryData.OrderBy(c => c.CountryName).ToList();

                if (SortCountriesByContinent)
                {
                    CountryData = PerformCountrySort(CountryData);
                }

                if (FilterSmallCountries)
                {
                    CountryData = PerformSmallCountryFilter(CountryData);
                }
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
            ProcessedCountryData.Clear();

            List<Metrics> metrics = new List<Metrics>();
            if (IncludeDeaths) metrics.Add(Metrics.Deaths);
            if (IncludeCases) metrics.Add(Metrics.Cases);
            if (IncludeTests) metrics.Add(Metrics.Tests);
            if (IncludePercentPositive) metrics.Add(Metrics.PercentPositive);
            if (IncludeCurrentHospitalizations) metrics.Add(Metrics.HospitalCurrent);
            if (IncludeNewHospitalizations) metrics.Add(Metrics.HospitalNew);

            if (UseCovidDataSource)
            {
                foreach (State s in StateData)
                {
                    var processedState = new ProcessedState(s);
                    if (UseDailyData)
                    {
                        int rollingAvg = 7;
                        var parseSuccess = Int32.TryParse(RollingAverageNumber, out rollingAvg);
                        if (rollingAvg == 0) rollingAvg = 7;
                        processedState.ProcessDailyData(metrics, ProcessRollingAverage, rollingAvg, ProcessPerCapita);
                    }
                    else if (UseCumeData)
                    {
                        processedState.ProcessCumulativeData(metrics, ProcessPerCapita);
                    }

                    ProcessedStateData.Add(processedState);
                }
            }
            else if (UseOwidSource)
            {
                foreach (OwidCountry c in CountryData)
                {
                    var processedCountry = new ProcessedCountry(c);
                    if (UseDailyData)
                    {
                        int rollingAvg = 7;
                        var parseSuccess = Int32.TryParse(RollingAverageNumber, out rollingAvg);
                        if (rollingAvg == 0) rollingAvg = 7;
                        processedCountry.ProcessDailyData(metrics, ProcessRollingAverage, rollingAvg, ProcessPerCapita);
                    }
                    else if (UseCumeData)
                    {
                        processedCountry.ProcessCumulativeData(metrics, ProcessPerCapita);
                    }

                    ProcessedCountryData.Add(processedCountry);
                }
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
            Dictionary<string, List<CalculatedValue>> listOfFiles = null;
            if (UseCovidDataSource)
                listOfFiles = ProcessedStateData?.FirstOrDefault()?.OutputFiles;
            else if (UseOwidSource)
            {
                listOfFiles = ProcessedCountryData?.FirstOrDefault()?.OutputFiles;
                startDate = new DateTime(2020, 1, 1);
            }

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
                    if (UseCovidDataSource)
                    {
                        foreach (ProcessedState ps in ProcessedStateData)
                            headerRow.Add(ps.CoreStateData.StateBase.Code.ToString());
                    }
                    else if (UseOwidSource)
                    {
                        foreach (ProcessedCountry pc in ProcessedCountryData)
                            headerRow.Add(pc.CoreCountryData.CountryName);
                    }
                    dataWriter.WriteRow(headerRow);

                    while (currentDate.Date <= latestDate)
                    {
                        CsvRow nextRow = new CsvRow();
                        nextRow.Add(currentDate.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                        if (UseCovidDataSource)
                        {
                            foreach (ProcessedState ps in ProcessedStateData)
                            {
                                var calcVals = ps.OutputFiles[kvp.Key].FirstOrDefault(calcv => calcv.Date.Date == currentDate.Date);
                                if (calcVals == null)
                                    nextRow.Add("0");
                                else
                                    nextRow.Add(calcVals.Value.ToString("F3", CultureInfo.InvariantCulture));
                            }
                        }
                        else if (UseOwidSource)
                        {
                            foreach (ProcessedCountry pc in ProcessedCountryData)
                            {
                                var calcVals = pc.OutputFiles[kvp.Key].FirstOrDefault(calcv => calcv.Date.Date == currentDate.Date);
                                if (calcVals == null)
                                    nextRow.Add("0");
                                else
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

        private List<State> PerformStateSortByRegion(List<State> unsortedStates)
        {
            List<State> sortedStates = new List<State>();
            // Midwest states
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.IA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.IL));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.IN));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MI));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MN));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MO));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.OH));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.WI));
            // Mountain States
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.CO));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.ID));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NV));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.UT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.WY));
            // Northeast States
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.CT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.DC));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.DE));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MD));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NJ));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NY));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.RI));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.PA));
            // Southern Border
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.AL));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.AZ));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.CA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.FL));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.LA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MS));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NM));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.TX));

            // Mid South
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.AR));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.GA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.KY));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NC));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.SC));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.TN));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.VA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.WV));

            // Plain States
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.KS));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.MT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.ND));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NE));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.OK));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.SD));

            //West Coast
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.CA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.OR));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.WA));

            // Upper Northeast + AK + HI
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.NH));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.VT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.ME));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.AK));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.StateBase.Code == StateCode.HI));

            return sortedStates;

        }

        private List<OwidCountry> PerformCountrySort(List<OwidCountry> unsortedCountries)
        {
            var sortedCountries = unsortedCountries.OrderBy(country => country.Continent).ToList();
            return sortedCountries;
        }
        private List<OwidCountry> PerformSmallCountryFilter(List<OwidCountry> unfilteredCountries)
        {
            var sortedCountries = unfilteredCountries.Where(country => country.Population > 10000000.00).ToList();
            return sortedCountries;
        }
    }

}
