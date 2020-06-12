using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovidSharp.CovidTrack;
using CovidSharp.Models;

namespace CovidSharpUi.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public List<State> StateData { get; set; }
        public MainViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
