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
using System.IO;
using System.Security.Cryptography; //encrypt user credntials
using System.Text; //encoding

namespace ToDelicious
{

    public partial class Settings: PhoneApplicationPage
    {
        // location to save the user values to iso storage
        private static string isoStorePasswordLocation = "password";
        private static string isoStoreUsernameLocation = "username";

        // property for whether the account data is currently stored
        public bool hasAccount
        {
            get
            {
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                return store.FileExists(isoStorePasswordLocation);
            }
        }


        // property for data binding
        public string username { get; set; }

        public Settings()
        {
            InitializeComponent();
            // might as well display the username if we have one stored
            username = RetrieveUsername();
            usernameText.Text = username;
        }

        /// <summary>
        /// Clear the stored credentials from isolated storage and burn them with fire to be sure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            byte [] randomData = ProtectData("blargywhatsitfakedata");
            // write over any saved user data from isolated storage just for extra fun
            WriteProtectedDataToFile(randomData, isoStorePasswordLocation);
            WriteProtectedDataToFile(randomData, isoStoreUsernameLocation);
            // now delete those files, which should only contain garbage anyway
            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            store.DeleteFile(isoStorePasswordLocation);
            store.DeleteFile(isoStoreUsernameLocation);
        }

        /// <summary>
        /// ecnrypt the entered username and password and then save to isolated storage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            // Store the encrypted password in isolated storage
            byte[] protectedPassword = ProtectData(passwordText.Password);
            WriteProtectedDataToFile(protectedPassword, isoStorePasswordLocation);
            // Might as well encrypt the username too
            byte [] protectedUsername = ProtectData(usernameText.Text);
            WriteProtectedDataToFile(protectedUsername, isoStoreUsernameLocation);
        }

        /// <summary>
        /// retreive from storage and decrypt the user password
        /// </summary>
        /// <returns></returns>
        public static string RetrievePassword()
        {
            // Retrieve the PIN from isolated storage.
            byte[] protectedPassword = ReadDataFromFile(isoStorePasswordLocation);
            if (protectedPassword == null)
                return "";
            // Decrypt the PIN by using the Unprotect method.
            byte[] clearPassword = ProtectedData.Unprotect(protectedPassword, null);
            // Convert the data from byte to string 
            return Encoding.UTF8.GetString(clearPassword, 0, clearPassword.Length);
        }

        /// <summary>
        /// retrieve from storage and decrypt the username
        /// </summary>
        /// <returns></returns>
        public static string RetrieveUsername()
        {
            // Retrieve the username from isolated storage.
            byte[] protectedUsername = ReadDataFromFile(isoStoreUsernameLocation);
            if (protectedUsername == null)
                return "";
            // Decrypt the username by using the Unprotect method.
            byte[] clearUsername = ProtectedData.Unprotect(protectedUsername, null);
            // Convert the data from byte to string 
            return Encoding.UTF8.GetString(clearUsername, 0, clearUsername.Length);
        }


        #region data encryption and storage helper methods


        /// <summary>
        /// encrypts a string using device and account data as salt
        /// </summary>
        /// <param name="input">string to encrypt</param>
        /// <returns>encrypted data</returns>
        private static byte[] ProtectData(string input)
        {
            // Convert the password to a byte[].
            byte[] stringAsBytes = Encoding.UTF8.GetBytes(input);
            // Encrypt the password by using the Protect() method.
            byte[] protectedString = ProtectedData.Protect(stringAsBytes, null);
            return protectedString;
        }

        /// <summary>
        /// save a string of bytes to a specific location in isolated storage
        /// </summary>
        /// <param name="data">data to save</param>
        /// <param name="FilePath">location to save at</param>
        private static void WriteProtectedDataToFile(byte[] data, string FilePath)
        {
            // Create a file in the application's isolated storage.
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream writestream = new IsolatedStorageFileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, file);

            // Write data to the file.
            Stream writer = new StreamWriter(writestream).BaseStream;
            writer.Write(data, 0, data.Length);
            writer.Close();
            writestream.Close();
        }


        /// <summary>
        /// retrieve encrypted data from a given location in isolated storage
        /// </summary>
        /// <param name="FilePath">the location of the data to fetch</param>
        /// <returns>a string of bytes</returns>
        private static byte[] ReadDataFromFile(string FilePath)
        {
            byte[] pinArray;
            try
            {
                // Access the file in the application's isolated storage.
                IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream readstream = new IsolatedStorageFileStream(FilePath, System.IO.FileMode.Open, FileAccess.Read, file);

                // Read the data from the file.
                Stream reader = new StreamReader(readstream).BaseStream;
                pinArray = new byte[reader.Length];

                reader.Read(pinArray, 0, pinArray.Length);
                reader.Close();
                readstream.Close();
            }
            catch (Exception e)
            {
                //Little Watson it? careful not to send any passwords in that
                return null;
            }

            return pinArray;
        }

        #endregion

    }
}