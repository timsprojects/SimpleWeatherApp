using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using System.Collections.Generic;
using Android.Content.PM;

namespace myWeatherApp
{
    [Activity(Label = "myWeatherApp", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        String lstBtnSelected;

        DataStorage objDs;
        DataAdapter objAdapter;
        RESThandler objRest;

        TextView txtDate;
        Spinner spinnerCity;
        Button btnGetWeather;

        Button btnWeatherMyLocation;

        ImageView ivWeatherToday;
        TextView txtCity;
        TextView txtTemp;
        TextView txtForecast;
        TextView txtLow;
        TextView txtHigh;

        ListView lvWeeklyForecast;

        //string forecastDay;
        Forecast[] forecast;
        String selectedCity;

        Location myCurrentLocation;
        LocationManager myLocationManager;
        String myLocationProvider = "";
        //TextView myLocationTextView;
        //TextView myAddressTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Spinner spinnerCity = FindViewById<Spinner>(Resource.Id.spinnerCity);
            btnGetWeather = FindViewById<Button>(Resource.Id.btnGetWeather);

            btnWeatherMyLocation = FindViewById<Button>(Resource.Id.btnWeatherMyLocation);

            ivWeatherToday = FindViewById<ImageView>(Resource.Id.ivWeatherToday);
            txtCity = FindViewById<TextView>(Resource.Id.txtCity);
            txtTemp = FindViewById<TextView>(Resource.Id.txtTemp);
            txtForecast = FindViewById<TextView>(Resource.Id.txtForecast);
            txtLow = FindViewById<TextView>(Resource.Id.txtLow);
            txtHigh = FindViewById<TextView>(Resource.Id.txtHigh);

            lvWeeklyForecast = FindViewById<ListView>(Resource.Id.lvWeeklyForecast);


            spinnerCity.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.city_array, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerCity.Adapter = adapter;

            btnGetWeather.Click += GetWeather_Click;

            btnWeatherMyLocation.Click += BtnWeatherMyLocation_Click;

            PopulateUI();
            BuildMyLocationManager();
            btnWeatherMyLocation.Enabled = false;
        }

        private void ClearUI()
        {
            txtCity.Text = "";
            txtTemp.Text = "";
            txtForecast.Text = "";
            txtLow.Text = "";
            txtHigh.Text = "";
        }

        private void PopulateUI()
        {
            TextView txtDate = FindViewById<TextView>(Resource.Id.txtDate);
            DateTime now = DateTime.Now;
            txtDate.Text = now.ToString();
        }
        private void GetWeather_Click(object sender, EventArgs e)
        {
            ClearUI();
            DailyWeather(selectedCity);
            WeeklyWeather(selectedCity);
            lstBtnSelected = "yes";
        }

        private void BtnWeatherMyLocation_Click(object sender, EventArgs e)
        {
            ClearUI();
            DailyWeather();
            WeeklyWeather();
            lstBtnSelected = "no";
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinnerCity = (Spinner)sender;

            selectedCity = spinnerCity.GetItemAtPosition(e.Position).ToString();
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void GetImage(string namedImage, ImageView elementName)
        {
            string ImgUrl;

            ImgUrl = "http://openweathermap.org/img/w/" + namedImage + ".png";

            Koush.UrlImageViewHelper.SetUrlDrawable(elementName, ImgUrl);
        }

        public async void DailyWeather()
        {
            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?lat=" + myCurrentLocation.Latitude.ToString() + "&lon=" + myCurrentLocation.Longitude.ToString() + "&units=metric&appid=4523a891deaa6f198b0a55eb53303f6d");

            var Response = await objRest.ExecuteRequestAsync();

            txtCity.Text = Response.name;
            txtTemp.Text = Response.main.temp.ToString() + "°C";
            txtForecast.Text = Response.weather[0].description.ToString();
            txtLow.Text = Response.main.temp_min.ToString() + "°C";
            txtHigh.Text = Response.main.temp_max.ToString() + "°C";

            string iconRef = Response.weather[0].icon;

            GetImage(iconRef, ivWeatherToday);
        }

        public async void DailyWeather(string CityName)
        {
            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?q=" + CityName + ",NZ&units=metric&appid=4523a891deaa6f198b0a55eb53303f6d");
            
            var Response = await objRest.ExecuteRequestAsync();

            txtCity.Text = Response.name;
            txtTemp.Text = Response.main.temp.ToString() + "°C";
            txtForecast.Text = Response.weather[0].description.ToString();
            txtLow.Text = Response.main.temp_min.ToString() + "°C";
            txtHigh.Text = Response.main.temp_max.ToString() + "°C";

            string iconRef = Response.weather[0].icon;

            GetImage(iconRef, ivWeatherToday);
        }

        //public async void WeatherCurrentLocation()
        //{
        //    objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?lat=" + myCurrentLocation.Latitude + "&lon=" + myCurrentLocation.Longitude + "&units=metric&appid=4523a891deaa6f198b0a55eb53303f6d");

        //    var Response = await objRest.ExecuteRequestAsync();

        //    txtCity.Text = Response.name;
        //    txtTemp.Text = Response.main.temp.ToString();
        //    txtForecast.Text = Response.weather[0].description.ToString();
        //    txtLow.Text = Response.main.temp_min.ToString();
        //    txtHigh.Text = Response.main.temp_max.ToString();

        //    string iconRef = Response.weather[0].icon;

        //    Toast.MakeText(this, iconRef, ToastLength.Long).Show();

        //    GetImage(iconRef, ivWeatherToday);
        //}

        public async void WeeklyWeather()
        {
            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/forecast/daily?lat=" + myCurrentLocation.Latitude.ToString() + "&lon=" + myCurrentLocation.Longitude.ToString() + "&units=metric&appid=4523a891deaa6f198b0a55eb53303f6d");

            var Response = await objRest.ExecuteRequestAsync();

            string[] forecastDay = new string[5];
            string[] forecastIcon = new string[5];
            string[] forecastHigh = new string[5];
            string[] forecastLow = new string[5];
            for (int i = 0; i < forecastDay.Length; i++)
            {
                forecastDay[i] = UnixTimeStampToDateTime(Response.list[i + 1].dt).ToString("dddd");
                forecastIcon[i] = Response.list[i + 1].weather[0].icon.ToString();
                forecastHigh[i] = Response.list[i + 1].temp.max.ToString() + "°C";
                forecastLow[i] = Response.list[i + 1].temp.min.ToString() + "°C";
            }

            forecast = new Forecast[forecastDay.Length];

            for (int i = 0; i < forecastDay.Length; i++)
            {
                forecast[i] = new Forecast(forecastDay[i], forecastIcon[i], forecastHigh[i], forecastLow[i]);
            }

            objAdapter = new DataAdapter(this, forecast);

            lvWeeklyForecast.Adapter = objAdapter;
        }

        public async void WeeklyWeather(string CityName)
        {
            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/forecast/daily?q=" + CityName + ",NZ&units=metric&&cnt=6&appid=4523a891deaa6f198b0a55eb53303f6d");

            var Response = await objRest.ExecuteRequestAsync();
            
            string[] forecastDay = new string[5];
            string[] forecastIcon = new string[5];
            string[] forecastHigh = new string[5];
            string[] forecastLow = new string[5];
            for (int i = 0; i < forecastDay.Length; i++)
            {
                forecastDay[i] = UnixTimeStampToDateTime(Response.list[i + 1].dt).ToString("dddd");
                forecastIcon[i] = Response.list[i + 1].weather[0].icon.ToString();
                forecastHigh[i] = Response.list[i + 1].temp.max.ToString() + "°C";
                forecastLow[i] = Response.list[i + 1].temp.min.ToString() + "°C";
            }

            forecast = new Forecast[forecastDay.Length];

            for (int i = 0; i < forecastDay.Length; i++)
            {
                forecast[i] = new Forecast(forecastDay[i], forecastIcon[i], forecastHigh[i], forecastLow[i]);
            }

            objAdapter = new DataAdapter(this, forecast);

            lvWeeklyForecast.Adapter = objAdapter;
        }

        //Save State Code
        private void RestoreSettings()
        {
            objDs = new DataStorage(this);
            selectedCity = objDs.getSelectedCity();
            lstBtnSelected = objDs.getBtnSelected();
            if (lstBtnSelected == "no")
            {
                ClearUI();
                DailyWeather();
                WeeklyWeather();
                return;
            }
            ClearUI();
            DailyWeather(selectedCity);
            WeeklyWeather(selectedCity);
        }

        private void SaveSettings()
        {
            if (selectedCity != null && selectedCity != "")
            {
                objDs = new DataStorage(this);
                objDs.saveSelectedCity(selectedCity);
                objDs.saveBtnSelected(lstBtnSelected.ToString());
            }
        }

        //Location Code
        private void BuildMyLocationManager()
        {
            myLocationManager = (LocationManager)
            GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };

            IList<String> acceptableLocationProviders =
            myLocationManager.GetProviders
            (criteriaForLocationService, true);

            if (acceptableLocationProviders != null &&
            acceptableLocationProviders.Count > 0)
            {
                myLocationProvider = acceptableLocationProviders[0];
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }

        public void OnLocationChanged(Location location)

        {
            myCurrentLocation = location;
            if (myCurrentLocation != null)
            {
                btnWeatherMyLocation.Enabled = true;
            }      
        }

        protected override void OnPause()
        {
            base.OnPause();
            SaveSettings();
            myLocationManager.RemoveUpdates(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RestoreSettings();
            myLocationManager.RequestLocationUpdates
            (myLocationProvider, 0, 0, this);
         }

    }
}

