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
using Java.Net;
using Android.Graphics;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net;
using System.IO;


namespace myWeatherApp
{

	public class DataAdapter : BaseAdapter<Forecast> {

		Forecast[] items;

		Activity context;
		public DataAdapter(Activity context, Forecast[] items)
			: base()
		{
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override Forecast this[int position]
		{
			get { return items[position]; }
		}
		public override int Count
		{
			get { return items.Length; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
                              //view = context.LayoutInflater.Inflate(Resource.Layout.CustomRow, null);
                              view = context.LayoutInflater.Inflate(Resource.Layout.CustomRow, null);

            view.FindViewById<TextView>(Resource.Id.txtdayOfWeek).Text = item.Day;
			view.FindViewById<TextView>(Resource.Id.txtForecastHigh).Text = item.High;
            view.FindViewById<TextView>(Resource.Id.txtForecastLow).Text = item.Low;

            var test = view.FindViewById<ImageView>(Resource.Id.ivWeeklyForecast);

            GetImage(item.Icon,test);

            //string ImgUrl = "http://openweathermap.org/img/w/" + item.Icon + ".png";

            //var imageBitMap = GetImageBitmapFromUrl(ImgUrl);
            //view.FindViewById<ImageView>(Resource.Id.ivWeeklyForecast).SetImageBitmap(imageBitMap);

            return view;
		}

        public void GetImage(string namedImage, ImageView elementName)
        {
            string ImgUrl;

            ImgUrl = "http://openweathermap.org/img/w/" + namedImage + ".png";

            Koush.UrlImageViewHelper.SetUrlDrawable(elementName, ImgUrl);
        }

        //private Bitmap GetImageBitmapFromUrl(string url)
        //{
        //    Bitmap imageBitmap = null;
        //    if (!(url == "null"))
        //        try
        //        {
        //            using (var webClient = new WebClient())
        //            {
        //                var imageBytes = webClient.DownloadData(url);
        //                if (imageBytes != null && imageBytes.Length > 0)
        //                {
        //                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //                }
        //            }                   
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    return imageBitmap;
        //}

    }
}
