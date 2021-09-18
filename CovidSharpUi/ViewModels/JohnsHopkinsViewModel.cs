using CovidSharp.Constants;
using CovidSharp.JohnsHopkins.Model;
using CovidSharp.Models;
using CovidSharp.Utils;
using CovidSharpUi.Enums;
using CovidSharpUi.Models;
using CsvHelper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using static CovidSharp.Utils.CsvUtility;

namespace CovidSharpUi.ViewModels
{
    public class JohnsHopkinsViewModel : ObservableObject
    {
        #region Status property
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
        #endregion

        #region JHFolderLocation
        private string _jhFolderLocation { get; set; }
        public string JHFolderLocation
        {
            get { return _jhFolderLocation; }
            set
            {
                _jhFolderLocation = value;
                RaisePropertyChanged("JHFolderLocation");
            }
        }
        #endregion 

        #region RollingAverage property
        private string _rollingAverage { get; set; }
        public string RollingAverage
        {
            get { return _rollingAverage; }
            set
            {
                _rollingAverage = value;
                RaisePropertyChanged("RollingAverage");
            }
        }
        #endregion 

        private StorageFolder jhFolder { get; set; }
        private List<JhState> jhStateData {get; set;}
        private List<ProcessedJhState> ProcessedJhStateData { get; set; }

        public RelayCommand SelectJhSourceCommand { get; private set; }
        public RelayCommand ParseAndExportJhDataCommand { get; private set; }

        public JohnsHopkinsViewModel()
        {
            jhStateData = new List<JhState>();
            ProcessedJhStateData = new List<ProcessedJhState>();
            SelectJhSourceCommand = new RelayCommand(new Action(SelectHopkinsSource), true);
            ParseAndExportJhDataCommand = new RelayCommand(new Action(ProcessHopkinsData), true);
            Init();
        }

        public async Task Init()
        {
            jhFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("JohnsHopkinsFolder");
            if (jhFolder != null)
                JHFolderLocation = jhFolder.Path;
            else
                JHFolderLocation = "no Johns Hopkins folder selected";
        }

        public async void SelectHopkinsSource()
        {
            Status = "Picking John Hopkins Folder";
            // Step 1: select folder for vaccine data 
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            jhFolder = await picker.PickSingleFolderAsync();
            if (jhFolder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("JohnsHopkinsFolder", jhFolder);
                JHFolderLocation = jhFolder.Path;
            }
            else {
                Status = "No data detected.";
                return;
            }
        }

        public async void ProcessHopkinsData()
        {
            Status = "Loading John Hopkins Data";
            var jhFiles = await jhFolder.GetFilesAsync();
            jhStateData.Clear();

            // set up all the states with all the dates
            foreach (StorageFile file in jhFiles)
            {
                if (file.FileType.Contains("csv")) { 
                    var stream = await file.OpenStreamForReadAsync();
                    using (var reader = new StreamReader(stream))
                    {
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var rawData = new List<RawJhState>();
                            csv.Read();
                            csv.ReadHeader();
                            while (csv.Read())
                            {
                                var tests = csv[11];
                                var Province_State = csv.GetField("Province_State");
                                var Country_Region = csv.GetField("Country_Region");
                                var Last_Update = csv.GetField("Last_Update");

                                var Lat = Double.TryParse(csv.GetField("Lat"), out double lat) ? lat : 0;
                                var Long_ = Double.TryParse(csv.GetField("Long_"), out double lon) ? lon : 0;
                                var Confirmed = Int32.TryParse(csv.GetField("Confirmed"), out int con) ? con : 0;
                                var Deaths = Int32.TryParse(csv.GetField("Deaths"), out int d) ? d : 0;
                                var Recovered = Int32.TryParse(csv.GetField("Recovered"), out int rec) ? rec : 0;
                                var Active = Int32.TryParse(csv.GetField("Active"), out int activ) ? activ : 0;
                                var FIPS = Int32.TryParse(csv.GetField("FIPS"), out int fips) ? fips : 0;
                                var Incident_Rate = Double.TryParse(csv.GetField("Incident_Rate"), out double incident) ? incident : 0;
                                var People_Tested = Int32.TryParse(csv[11], out int test) ? test : 0;
                                var People_Hospitalized = Int32.TryParse(csv.GetField("People_Hospitalized"), out int hosp) ? hosp : 0;
                                var Mortality_Rate = Double.TryParse(csv[13], out double mort) ? mort : 0;
                                var UID = Int32.TryParse(csv.GetField("UID"), out int uid) ? uid : 0;
                                var ISO3 = csv.GetField("ISO3");
                                var Testing_Rate = Double.TryParse(csv.GetField("Testing_Rate"), out double testrate) ? testrate : 0;
                                //var Hospitalization_Rate = Double.TryParse(csv.GetField("Hospitalization_Rate"), out double hosprate) ? hosprate : 0;

                                var newDay = new RawJhState()
                                {
                                    Province_State = Province_State,
                                    Country_Region = Country_Region,
                                    Last_Update = Last_Update,
                                    Lat = Lat,
                                    Long_ = Long_,
                                    Confirmed = Confirmed,
                                    Deaths = Deaths,
                                    Recovered = Recovered,
                                    Active = Active,
                                    FIPS = FIPS,
                                    Incident_Rate = Incident_Rate,
                                    People_Tested = People_Tested,
                                    People_Hospitalized = People_Hospitalized,
                                    Mortality_Rate = Mortality_Rate,
                                    UID = UID,
                                    ISO3 = ISO3,
                                    Testing_Rate = Testing_Rate,
                                    //Hospitalization_Rate = Hospitalization_Rate
                                };
                                rawData.Add(newDay);

                            }

                            foreach (RawJhState rjhs in rawData)
                            {
                                if (jhStateData.FirstOrDefault(st => st.StateString == rjhs.Province_State) == null)
                                {
                                    jhStateData.Add(new JhState(rjhs.Province_State));
                                }

                                JhCovidDay covidDay = new JhCovidDay(rjhs, file.Name);

                                jhStateData.FirstOrDefault(st => st.StateString == rjhs.Province_State).CovidData.Add(covidDay);
                            }
                        }
                    }
                }
            }

            jhStateData.RemoveAll(jhs => jhs.State == null);

            jhStateData = PerformStateSortByRegion(jhStateData);

            Status = "Processing Johns Hopkins Data";
            ProcessedJhStateData.Clear();
            List<Metrics> metrics = new List<Metrics>() { Metrics.Deaths, Metrics.Tests, Metrics.Cases, Metrics.PercentPositive };
            foreach(JhState state in jhStateData)
            {
                if (state?.State != null)
                {
                    var processedJhState = new ProcessedJhState(state);

                    var rollAverage = Int32.TryParse(RollingAverage, out int rollingAvg) ? rollingAvg : 7;
                    if (rollAverage == 0) rollAverage = 7;
                    processedJhState.ProcessDailyData(metrics, true, rollAverage);
                    //processedJhState.ProcessDailyData(metrics, false, rollAverage);
                    processedJhState.ProcessCumulativeData(metrics);
                    ProcessedJhStateData.Add(processedJhState);
                }
            }

            ExportHopkinsData();
        }


        private List<JhState> PerformStateSortByRegion(List<JhState> unsortedStates)
        {
            List<JhState> sortedStates = new List<JhState>();
            // Midwest states
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.IA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.IL));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.IN));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MI));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MN));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MO));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.OH));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.WI));
            // Mountain States
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.CO));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.ID));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NV));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.UT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.WY));
            // Northeast States
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.CT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.DC));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.DE));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MD));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NJ));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NY));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.RI));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.PA));
            // Southern Border
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.AL));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.AZ));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.CA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.FL));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.LA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MS));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NM));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.TX));

            // Mid South
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.AR));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.GA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.KY));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NC));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.SC));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.TN));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.VA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.WV));

            // Plain States
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.KS));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.MT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.ND));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NE));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.OK));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.SD));

            //West Coast
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.CA));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.OR));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.WA));

            // Upper Northeast + AK + HI
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.NH));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.VT));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.ME));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.AK));
            sortedStates.Add(unsortedStates.FirstOrDefault(s => s.State.Code == StateCode.HI));

            return sortedStates;

        }

        public async void ExportHopkinsData()
        {
            Status = "Exporting Johns Hopkins Data";
            StorageFolder exportFolder = null;
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("ExportFolder")) {
                exportFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("ExportFolder");
            } else {
                var picker = new Windows.Storage.Pickers.FolderPicker();
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                picker.FileTypeFilter.Add("*");

                exportFolder = await picker.PickSingleFolderAsync();
                if (exportFolder != null) { 
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("ExportFolder", exportFolder);
                }
            }

            DateTime startDate = new DateTime(2020, 3, 1);
            Dictionary<string, List<CalculatedValue>> listOfFiles = null;
            listOfFiles = ProcessedJhStateData?.FirstOrDefault()?.OutputFiles;
            
            if (exportFolder == null || listOfFiles == null) return;

            foreach (KeyValuePair<string, List<CalculatedValue>> kvp in listOfFiles)
            {
                DateTime currentDate = startDate;
                DateTime latestDate = listOfFiles.Values.FirstOrDefault().Max(cv => cv.Date);
                StorageFile csvFile = await exportFolder?.CreateFileAsync(kvp.Key + ".csv", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                using (CsvFileWriter dataWriter = new CsvFileWriter(await csvFile.OpenStreamForWriteAsync()))
                {
                    CsvRow headerRow = new CsvRow();
                    headerRow.Add("Date");
                    foreach (ProcessedJhState ps in ProcessedJhStateData)
                        headerRow.Add(ps.CoreStateData.State.Code.ToString());
                    dataWriter.WriteRow(headerRow);

                    while (currentDate.Date <= latestDate)
                    {
                        CsvRow nextRow = new CsvRow();
                        nextRow.Add(currentDate.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                        foreach (ProcessedJhState ps in ProcessedJhStateData)
                        {
                            var calcVals = ps.OutputFiles[kvp.Key].FirstOrDefault(calcv => calcv.Date.Date == currentDate.Date);
                            if (calcVals == null)
                                nextRow.Add("0");
                            else
                                nextRow.Add(calcVals.Value.ToString("F3", CultureInfo.InvariantCulture));
                        }

                        dataWriter.WriteRow(nextRow);
                        currentDate = currentDate + new TimeSpan(1, 0, 0, 0);
                    }
                }
            }
            
            Status = "Done with JH Data";
        }

        public async void CtpToJHTransition(List<State> CtpData)
        {
            StorageFolder exportFolder = null;
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem("ExportFolder"))
            {
                DateTime currentDate = new DateTime(2020, 3, 2);
                DateTime endDate = new DateTime(2020, 4, 11);
                while (currentDate <= endDate)
                {
                    exportFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("ExportFolder");
                    StorageFile csvFile = await exportFolder?.CreateFileAsync(currentDate.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + ".csv", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    using (CsvFileWriter dataWriter = new CsvFileWriter(await csvFile.OpenStreamForWriteAsync()))
                    {
                        CsvRow headerRow = new CsvRow();
                        headerRow.Add("Province_State");
                        headerRow.Add("Country_Region");
                        headerRow.Add("Last_Update");
                        headerRow.Add("Lat");
                        headerRow.Add("Long_");
                        headerRow.Add("Confirmed");
                        headerRow.Add("Deaths");
                        headerRow.Add("Recovered");
                        headerRow.Add("Active");
                        headerRow.Add("FIPS");
                        headerRow.Add("Incident_Rate");
                        headerRow.Add("Total_Test_Results");
                        headerRow.Add("People_Hospitalized");
                        headerRow.Add("Case_Fatality_Ratio");
                        headerRow.Add("UID");
                        headerRow.Add("ISO3");
                        headerRow.Add("Testing_Rate");
                        headerRow.Add("Testing_Rate");

                        dataWriter.WriteRow(headerRow);
                        foreach (State s in CtpData)
                        {
                            var dataPoint = s.CovidData.FirstOrDefault(dat => dat.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) == currentDate.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture));
                            if (dataPoint != null)
                            {
                                CsvRow nextRow = new CsvRow();
                                nextRow.Add(StateMapHelper.StateBaseToString(s.StateBase.Name));
                                nextRow.Add("US");
                                nextRow.Add(currentDate.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " 4:30:00 PM");
                                nextRow.Add("");
                                nextRow.Add("");

                                if (dataPoint.Positive != null)
                                    nextRow.Add(dataPoint.Positive.ToString());
                                else
                                    nextRow.Add("0");

                                if (dataPoint.Death != null)
                                    nextRow.Add(dataPoint.Death.ToString());
                                else
                                    nextRow.Add("0");

                                nextRow.Add(""); //Recovered
                                nextRow.Add(""); //active
                                nextRow.Add(""); // FIPS
                                nextRow.Add(""); // Incident_Rate

                                if (dataPoint.TotalTestResults != null)
                                    nextRow.Add(dataPoint.TotalTestResults.ToString());
                                else
                                    nextRow.Add("0");

                                if (dataPoint.Hospitalized != null)
                                    nextRow.Add(dataPoint.Hospitalized.ToString());
                                else
                                    nextRow.Add("0");

                                if (dataPoint.Positive > 0)
                                {
                                    nextRow.Add(((Convert.ToDouble(dataPoint.Death) / Convert.ToDouble(dataPoint.Positive)) * 100).ToString());
                                }
                                else
                                {
                                    nextRow.Add("0.0");
                                }
                                nextRow.Add(""); // UID
                                nextRow.Add(""); // ISO3
                                nextRow.Add(""); // TestingRate
                                nextRow.Add(""); // Hospitalization Rate

                                dataWriter.WriteRow(nextRow);
                            }
                            else
                            {
                                CsvRow nextRow = new CsvRow();
                                nextRow.Add(s.StateBase.Name.ToString());
                                nextRow.Add("US");
                                nextRow.Add(currentDate.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " 4:30:00 PM");
                                nextRow.Add("");
                                nextRow.Add("");
                                nextRow.Add("0");
                                nextRow.Add("0");
                                nextRow.Add(""); //Recovered
                                nextRow.Add(""); //active
                                nextRow.Add(""); // FIPS
                                nextRow.Add(""); // Incident_Rate
                                nextRow.Add("0");
                                nextRow.Add("0");
                                nextRow.Add("0.0");
                                nextRow.Add(""); // UID
                                nextRow.Add(""); // ISO3
                                nextRow.Add(""); // TestingRate
                                nextRow.Add(""); // Hospitalization Rate

                                dataWriter.WriteRow(nextRow);
                            }
                        }
                        
                        currentDate = currentDate + new TimeSpan(1, 0, 0, 0);
                    }
                }
            }
        }
    }
}
