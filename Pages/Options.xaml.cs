using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace PictureQuiz
{
    public partial class Options : PhoneApplicationPage
    {
        public Options()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("HidePictureDescription"))
                PictureDescriptionToggle.IsChecked = (bool)IsolatedStorageSettings.ApplicationSettings["HidePictureDescription"];
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("HidePictureDescription"))
            {
                IsolatedStorageSettings.ApplicationSettings.Add("HidePictureDescription", PictureDescriptionToggle.IsChecked);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["HidePictureDescription"] = PictureDescriptionToggle.IsChecked;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();

            base.OnNavigatingFrom(e);
        }
    }
}