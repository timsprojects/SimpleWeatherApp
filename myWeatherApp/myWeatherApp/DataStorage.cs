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
using Android.Preferences;

namespace myWeatherApp
{
    public class DataStorage
    {
        private ISharedPreferences mSharedPrefs;
        private ISharedPreferencesEditor mPrefsEditor;
        private Context mContext;

        private static String PREFERENCE_SELECTED_CITY = "PREFERENCE_SELECTED_CITY";
        private static String PREFERENCE_BTN_SELECTED = "PREFERENCE_BTN_SELECTED";
        //private static String PREFERENCE_NOTE_DETAILS = "PREFERENCE_NOTE_DETAILS";

        public DataStorage(Context context)
        {
            this.mContext = context;
            mSharedPrefs = PreferenceManager.GetDefaultSharedPreferences(mContext);
            mPrefsEditor = mSharedPrefs.Edit();
        }

        public void saveSelectedCity(string citynametosave)
        {
            mPrefsEditor.PutString(PREFERENCE_SELECTED_CITY, citynametosave);
            mPrefsEditor.Commit();
        }

        public string getSelectedCity()
        {
            return mSharedPrefs.GetString(PREFERENCE_SELECTED_CITY, "");
        }

        public void saveBtnSelected(string selectedindicator)
        {
            mPrefsEditor.PutString(PREFERENCE_BTN_SELECTED, selectedindicator);
            mPrefsEditor.Commit();
        }

        public string getBtnSelected()
        {
            return mSharedPrefs.GetString(PREFERENCE_BTN_SELECTED, "");
        }

        //public void saveNoteDetails(string notedetails)
        //{
        //    mPrefsEditor.PutString(PREFERENCE_NOTE_DETAILS, notedetails);
        //    mPrefsEditor.Commit();
        //}

        //public string getNoteDetails()
        //{
        //    return mSharedPrefs.GetString(PREFERENCE_NOTE_DETAILS, "");
        //}

    }
}