using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml.Linq;
using System.Windows.Documents;

namespace PictureQuiz
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();

            string version = XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            VersionText.Inlines.Add("Version " + version);
        }
    }
}