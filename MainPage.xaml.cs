using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ToDelicious.Resources;
using Microsoft.Phone.Tasks; //marketplace review, email
using Delicious;
using System.Xml;
using System.IO.IsolatedStorage;
using System.IO;

namespace ToDelicious
{
    public partial class MainPage : PhoneApplicationPage
    {
        // save the user values to iso storage
        private static string isoStorePasswordLocation = "password";
        private static string isoStoreUsernameLocation = "username";

        // properties for databinding from ui
        public string username { get; set; }
        public string password { get; set; }
        public string url { get; set; }
        // app version number for feedback emails and bug reports
        private double _version = 0.1;
        public double version { get { return _version; } private set { _version = value; } }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            versionDisplay.Text = version.ToString();
        }

        private void sendUrlToDelicious()
        {
            Delicious.Connection.Password = passwordText.Password;
            Delicious.Connection.Username = usernameText.Text;
            Delicious.Post.Add(urlText.Text, "my newest url"); //if you add user-entered urls, make sure to remind them they need http, or auto add it for them
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

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            sendUrlToDelicious();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            // delete any saved user data from isolated storage
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}