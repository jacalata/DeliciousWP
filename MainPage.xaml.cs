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
using System.Threading;
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
            urlText.Text = "http:\\google.com";
            tagText.Text = "search,google,internet,example";
            // Sample code to localize the ApplicationBar
            BuildLocalizedApplicationBar();
        }
         
        /// <summary>
        /// If there is no account saved, ask the user if they'd like to add account details
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.hasAccount)
                return;


            // show a message box asking the user if they want to go to settings, 
            string msgContent = "Account details needed";
            string msgTitle = "You can't add a link until you've entered your account details. Would you like to go to the settings page and enter your account details now?";


            ThreadPool.QueueUserWorkItem((stateInfo) => {
                Dispatcher.BeginInvoke(() => {
                    if (MessageBoxResult.OK == MessageBox.Show(msgTitle, msgContent, MessageBoxButton.OKCancel))
                        NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
                });
            });
            
        }
        // Sample code for building a localized ApplicationBar

        private void BuildLocalizedApplicationBar()
      {
        // Set the page's ApplicationBar to a new instance of ApplicationBar.
          ApplicationBar = new ApplicationBar();
 
           // Create a new button and set the text value to the localized string from AppResources.
          ApplicationBarIconButton appBarSettingsButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative));
          appBarSettingsButton.Text = AppResources.AppBarSettingsButtonText;
          appBarSettingsButton.Click += new EventHandler(SettingsButton_Click);
          ApplicationBar.Buttons.Add(appBarSettingsButton);

          ApplicationBarIconButton appBarAboutButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/questionmark.png", UriKind.Relative));
          appBarAboutButton.Text = AppResources.AppBarAboutButtonText;
          appBarAboutButton.Click += new EventHandler(AboutButton_Click);
          ApplicationBar.Buttons.Add(appBarAboutButton);
       }

        #region button click handlers

        /// <summary>
        ///  creates a delicous request and sends it
        /// </summary>
        private void sendUrlToDelicious()
        {
            Delicious.Connection.Password = Settings.RetrievePassword();
            Delicious.Connection.Username = Settings.RetrieveUsername();
            if (Delicious.Connection.Password == "" || Delicious.Connection.Username == "")
            {
                MessageBoxResult noCredentialsAction = MessageBox.Show("Would you like to enter your account details now?", 
                    "You need an account to post links", MessageBoxButton.OKCancel);
                if (noCredentialsAction == MessageBoxResult.Cancel)
                    return;
                else
                    NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
            }
            if (urlText.Text == null)
                MessageBox.Show( "You need to add a url to send to Delicious!", "No URL entered", MessageBoxButton.OK);
            if (!urlText.Text.StartsWith("http"))
                MessageBox.Show( "Sorry, your URL needs to start with http", "Incomplete url",MessageBoxButton.OK);
            if (descriptionText.Text == null)
                descriptionText.Text = "url sent from WPDelicious";

            Delicious.Post.Add(urlText.Text, descriptionText.Text, null, tagText.Text, null); //if you add user-entered urls, make sure to remind them they need http, or auto add it for them
        }

        /// <summary>
        /// receives the button click event and calls the method to create a new request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO delete this is only for debugging
            MessageBox.Show("we are sending: " + Settings.RetrieveUsername() + ", " + Settings.RetrievePassword() + ", " + urlText.Text);
            sendUrlToDelicious();
        }

        /// <summary>
        /// handle app bar button for settings page click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        /// <summary>
        /// handle app bar button for about page click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        #endregion

    }
}