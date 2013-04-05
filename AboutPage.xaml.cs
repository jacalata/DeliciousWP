using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks; //marketplace review, email

namespace ToDelicious
{
    public partial class AboutPage : PhoneApplicationPage
    {
        // app version number for feedback emails and bug reports
        private double _version = 0.1;
        public double version { get { return _version; } private set { _version = value; } }

        public AboutPage()
        {
            InitializeComponent();
            versionDisplay.Text = version.ToString();
        }

        private void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Feedback: WP To Delicious" + version;
            emailComposeTask.Body = "";
            emailComposeTask.To = "jacalata@live.com";

            emailComposeTask.Show();
        }

        private void RateApp_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }
    }
}