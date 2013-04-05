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
using Delicious;
using System.Xml;

namespace ToDelicious
{
    public partial class MainPage : PhoneApplicationPage
    {
        public string url { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar

        private void BuildLocalizedApplicationBar()
      {
        // Set the page's ApplicationBar to a new instance of ApplicationBar.
          ApplicationBar = new ApplicationBar();
 
           // Create a new button and set the text value to the localized string from AppResources.
          ApplicationBarIconButton appBarSettingsButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative));
            // TODO where do I set the result for clicking on it?
          ApplicationBarIconButton appBarAboutButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/questionmark.png", UriKind.Relative));
          appBarSettingsButton.Text = AppResources.AppBarSettingsButtonText;
          appBarAboutButton.Text = AppResources.AppBarAboutButtonText;
         ApplicationBar.Buttons.Add(appBarAboutButton);
         ApplicationBar.Buttons.Add(appBarSettingsButton);
       }

        private void sendUrlToDelicious()
        {
            Delicious.Connection.Password = Settings.RetrievePassword();
            Delicious.Connection.Username = Settings.RetrieveUsername();
            if (Delicious.Connection.Password == null || Delicious.Connection.Username == null)
            {
                MessageBoxResult noCredentialsAction = MessageBox.Show("You need an account to post links", 
                    "Would you like to enter your account details now?", MessageBoxButton.OKCancel);
                if (noCredentialsAction == MessageBoxResult.Cancel)
                    return;
                else
                    NavigationService.Navigate(new Uri("Settings.xaml", UriKind.Relative));
            }
            if (urlText.Text == null)
                MessageBox.Show("No URL entered", "You need to add a url to send to Delicious!", MessageBoxButton.OK);
            if (!urlText.Text.StartsWith("http"))
                MessageBox.Show("Incomplete url", "Your URL needs to start with http", MessageBoxButton.OK);
            if (descriptionText.Text == null)
                descriptionText.Text = "url sent from WPDelicious";

            Delicious.Post.Add(urlText.Text, descriptionText.Text, null, tagText.Text, null); //if you add user-entered urls, make sure to remind them they need http, or auto add it for them
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("we are sending: " + Settings.RetrieveUsername() + ", " + Settings.RetrievePassword() + ", " + urlText.Text);
            sendUrlToDelicious();
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