using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace myWeatherApp
{
    public class Forecast
    {
        private string _Day;
        public string Day
        {
            set { this._Day = value; }
            get { return this._Day; }
        }
        private string _Icon;
        public string Icon 
        {
            set { this._Icon = value; }
            get { return this._Icon; }
        }
        private string _High;
        public string High
        {
            set { this._High = value; }
            get { return this._High; }
        }
        private string _Low;
        public string Low
        { 
             set { this._Low = value; }
             get { return this._Low; }
            }

    public Forecast(string day, string icon, string high, string low)
        {
            this.Day = day;
            this.Icon = icon;
            this.High = high;
            this.Low = low;
        }
    }
}