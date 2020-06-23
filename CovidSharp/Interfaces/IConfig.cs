using System;
using System.Collections.Generic;
using System.Text;

namespace CovidSharp.Interfaces
{
    public interface IConfig
    {
        string GetStatetUrl();

        string GetCountryUrl();

        string GetCountyUrl();
    }
}
