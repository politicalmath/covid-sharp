using CovidSharp.NytData;
using CovidSharp.NytData.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using static CovidSharp.Utils.CsvUtility;

namespace CovidSharpUi.ViewModels
{
    public class NytViewModel : ObservableObject
    {
        #region NytFolderLocation
        private string _nytFolderLocation { get; set; }
        public string NytFolderLocation
        {
            get { return _nytFolderLocation; }
            set
            {
                _nytFolderLocation = value;
                RaisePropertyChanged("NytFolderLocation");
            }
        }
        #endregion 

        #region NytProcessStatus
        private string _nytProcessStatus { get; set; }
        public string NytProcessStatus
        {
            get { return _nytProcessStatus; }
            set
            {
                _nytProcessStatus = value;
                RaisePropertyChanged("NytProcessStatus");
            }
        }
        #endregion 

        //private StorageFolder nytFolder { get; set; }
        private StorageFile countyDataFile { get; set; }
        private List<NytCountyDay> nytCountyData { get; set; }
        private List<NytFileOutput> ProcessedNytData { get; set; }

        private NytDataService nytDataService = new NytDataService();

        public StorageFolder ExportFolder { get; set; }

        public RelayCommand SelectNytSourceCommand { get; private set; }
        public RelayCommand ParseAndExportNytDataCommand { get; private set; }


        public NytViewModel()
        {
            nytCountyData = new List<NytCountyDay>();
            ProcessedNytData = new List<NytFileOutput>();

            SelectNytSourceCommand = new RelayCommand(new Action(SelectNytSource), true);
            ParseAndExportNytDataCommand = new RelayCommand(new Action(ProcessNytData), true);
        }

        public async Task Init()
        {
            countyDataFile= await StorageApplicationPermissions.FutureAccessList.GetFileAsync("CountyPopFile");
            if (countyDataFile != null)
                NytFolderLocation = countyDataFile.Path;
            else
                NytFolderLocation = "no county population file selected";
        }

        public async void SelectNytSource()
        {

            NytProcessStatus = "Downloading NYT Data";

            nytCountyData = await nytDataService.GetCountyCovidData();

            //Status = "Picking John Hopkins Folder";
            // Step 1: select folder for vaccine data 
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            countyDataFile = await picker.PickSingleFileAsync();
            if (countyDataFile != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("CountyPopFile", countyDataFile);
                NytFolderLocation = countyDataFile.Path;
            }
            else
            {
                //Status = "No data detected.";
                return;
            }

            NytProcessStatus = "County Pop Source Selected";
        }

        public async void ProcessNytData()
        {
            if (ExportFolder == null){
                NytProcessStatus = "Must Select Export Folder";
                return;
            }

            // 1: pull in the NYT data (Done)
            NytProcessStatus = "Processing County Populations";
            DateTime startTime = DateTime.Now;
            // 2: pull in the county population data
            var stream = await countyDataFile.OpenStreamForReadAsync();
            var countyPopulationData = await nytDataService.GetCountyPopData(stream);

            // 2.1: update nyt data to includ population info
            Dictionary<string, List<NytCountyDay>> sortedCounties = new Dictionary<string, List<NytCountyDay>>();

            foreach (CountyPop cp in countyPopulationData) {
                sortedCounties.Add(cp.Fips, new List<NytCountyDay>());

                foreach(NytCountyDay county in nytCountyData.Where(x => x.Fips == cp.Fips)) {
                    county.Population = Convert.ToDouble(cp.Population);
                    if (sortedCounties.ContainsKey(county.Fips)){
                        sortedCounties[cp.Fips].Add(county);
                    }
                }
            }

            Debug.WriteLine("County Population Sorting => " + (DateTime.Now - startTime).TotalMilliseconds.ToString() + "ms");

            startTime = DateTime.Now;

            // 2.2 Sort counties into a per-county dictionary
            // 3: calculate the 7-day average
            var countyStartTime = DateTime.Now;
            foreach (KeyValuePair<string, List<NytCountyDay>> kvp in sortedCounties)
            {
                //NytProcessStatus = "Processing County " + kvp.Key;
                //Debug.WriteLine("Processing County " + kvp.Key + " => " + (DateTime.Now - countyStartTime).TotalMilliseconds.ToString() + "ms");
                countyStartTime = DateTime.Now;

                foreach (var county in kvp.Value)
                {
                    var PreviousWeek = kvp.Value.Where(c => c.Date.Date == (county.Date - new TimeSpan(7, 0, 0, 0)).Date).FirstOrDefault();
                    county.PreviousWeekday = PreviousWeek;
                    county.ProcessValues();
                }
            }

            Debug.WriteLine("Processing Counties => " + (DateTime.Now - startTime).TotalMilliseconds.ToString() + "ms");
            startTime = DateTime.Now;
            // Additional thought: Could pre-sort by day instead of county

            // 5: export as a set of flat csv files (1 per day)
            // 5.1: get a list of days
            var listOfDays = nytCountyData.Where(sno => sno.Fips == "53061");

            foreach(var day in listOfDays)
            {
                countyStartTime = DateTime.Now;

                List<NytCountyDay> dayInfo = new List<NytCountyDay>();
                foreach(KeyValuePair<string, List<NytCountyDay>> kvp in sortedCounties)
                {
                    var fipsDay = kvp.Value.Where(co => co.DateString == day.DateString).FirstOrDefault();
                    if (fipsDay != null) {
                        dayInfo.Add(fipsDay);
                    }
                }
                //var dayInfo = nytCountyData.Where(co => co.DateString == day.DateString);
                await ExportToFile(dayInfo);
            }

            Debug.WriteLine("Day Process + Export Counties => " + (DateTime.Now - startTime).TotalMilliseconds.ToString() + "ms");

        }

        private async Task ExportToFile(IEnumerable<NytCountyDay> CountyDayInfo)
        {
            StorageFile csvFile = await ExportFolder?.CreateFileAsync(CountyDayInfo.FirstOrDefault().DateString + "-county-info.csv", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            using (CsvFileWriter dataWriter = new CsvFileWriter(await csvFile.OpenStreamForWriteAsync()))
            {
                CsvRow headerRow = new CsvRow();
                headerRow.Add("fips");
                headerRow.Add("cases");
                headerRow.Add("deaths");
                headerRow.Add("cases7dayavg");
                headerRow.Add("deaths7dayavg");
                headerRow.Add("casesper100K");
                headerRow.Add("deathsper100K");
                dataWriter.WriteRow(headerRow);

                foreach (var county in CountyDayInfo)
                {
                    CsvRow nextRow = new CsvRow();
                    nextRow.Add(county.Fips);
                    nextRow.Add(county.Cases.ToString());
                    nextRow.Add(county.Deaths.ToString());
                    nextRow.Add(county.Cases7Avg.ToString());
                    nextRow.Add(county.Deaths7Avg.ToString());
                    nextRow.Add(county.Cases7AvgPer100K.ToString());
                    nextRow.Add(county.Deaths7AvgPer100K.ToString());
                    dataWriter.WriteRow(nextRow);
                }
                dataWriter.Close();
            }
        }
    }
}
