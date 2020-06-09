using CovidSharp;
using CovidSharp.Constants;
using CovidSharp.CovidTrack;
using CovidSharp.CovidTrack.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CovidSharpTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CovidTrackService covidTracking = new CovidTrackService();

            Console.WriteLine("Welcome to the CovidSharp sample app!");
            Console.WriteLine("This is a bare bones CLI explorer so you can see how CovidSharp works.");
            //Console.WriteLine("You can start by setting a data source (New York Times and covidtracking.com currently supported)");
            //Console.WriteLine("Just type 'setSource NYT' or 'setSource covidTrack'");
            Console.WriteLine("Type 'get us' to see all the US COVID data. Or 'get WA' to get all Washington COVID data");
            Console.WriteLine("Type 'get us today' to see the latest US COVID data. Or 'get WA today' for the latest COVID state data");
            Console.WriteLine("To get a specific date, type 'get us 05/02/2020' for the US data or 'get WA 05/02/2020' for state-specific data");
            Console.WriteLine("To get all historic state data, type 'get all");
            Console.WriteLine("(Type 'exit' to exit the application)");
            string input = "";
            while(input != "exit")
            {   
                input = Console.ReadLine();
                string dataResult = await GetData(input, covidTracking);
                Console.WriteLine(dataResult);

            }            
        }

        static async Task<string> GetData(string rawInput, CovidTrackService cService)
        {
            string[] parameters = rawInput.Split(' ');

            if(parameters.Length >= 2 && parameters[0] == "get")
            {
                // Did they request data for the US or a state
                if(parameters[1].ToLower() == "us")
                {
                    // Is there a time-based parameter?
                    if (parameters.Length == 3)
                    {
                        // Is it today?
                        if(parameters[2].ToLower() == "today")
                        {
                            var result = await cService.GetCurrentUsData();
                            return "Current US Data \n " + FormatDayData(result.FirstOrDefault());
                        } else {
                        
                            // Parse the input date
                            string[] dateParams = parameters[2].Split('/');
                            if (dateParams.Length == 3) {
                                var date = new DateTime(Int32.Parse(dateParams[2]), Int32.Parse(dateParams[0]), Int32.Parse(dateParams[1]));
                            
                                // Was it a valid input?
                                if (date != null)
                                {
                                    var result = await cService.GetUsDataOnDate(date);
                                    return FormatDayData(result);
                                }
                            }
                        }
                    } else
                    {
                        // OK, no date. Just dump that data out
                        var result = await cService.GetHistoricUsData();
                        string printResult = "";
                        foreach (UsDay day in result)
                            printResult += FormatDayData(day);

                        return printResult;
                    }

                } else {
                    var stateParam = parameters[1];

                    if(stateParam == "all")
                    {
                        var result = await cService.GetHistoricStateData();
                        string printResult = "";
                        foreach (StateDay stateDay in result)
                            printResult += FormatStateDayData(stateDay);
                        return printResult;
                    }

                    // We want to turn that state code into a state object, b/c that's more flexible
                    var states = StatesConstants.GetStatesList();
                    var targetState = states.FirstOrDefault(aState => aState.Code.ToString().ToLower() == stateParam.ToLower());
                    
                    // Is there a time-based parameter?
                    if(parameters.Length == 3)
                    {
                        // Is it today?
                        if (parameters[2].ToLower() == "today")
                        {
                            var result = await cService.GetCurrentStateData(targetState.Code);
                            return FormatStateDayData(result);
                        }
                        else
                        {

                            // Parse the input date
                            string[] dateParams = parameters[2].Split('/');
                            if (dateParams.Length == 3)
                            {
                                var date = new DateTime(Int32.Parse(dateParams[2]), Int32.Parse(dateParams[0]), Int32.Parse(dateParams[1]));

                                // Was it a valid input?
                                if (date != null)
                                {
                                    var result = await cService.GetStateDataByDate(targetState.Code, date);
                                    return FormatStateDayData(result);
                                }
                            }
                        }
                    } else {
                        var result = await cService.GetHistoricStateData(targetState.Code);
                        string printResult = "";
                        foreach (StateDay stateDay in result)
                            printResult += FormatStateDayData(stateDay);
                        return printResult;
                    }
                }
            }
            return "sorry, that didn't work";
        }

        static string FormatDayData(CovidDay day)
        {            
            string formattedResult = "For " + day.Date.Date.ToShortDateString() + "\n"; 
            formattedResult += "Total Deaths = " + day.Death.ToString() + "\n";
            formattedResult += "Death Increase = " + day.DeathIncrease.ToString() + "\n";
            formattedResult += "COVID Positive Cases = " + day.Positive.ToString() + "\n";
            formattedResult += "COVID Positive Increase = " + day.PositiveIncrease.ToString() + "\n\n";

            return formattedResult;
        }

        static string FormatStateDayData(StateDay stateDay)
        {
            string stateResult = "In " + stateDay.State + "\n";
            stateResult += FormatDayData(stateDay);

            return stateResult;
        }
    }
}
