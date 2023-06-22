using Password_Manager.Properties;
using System;
using System.IO;
using System.Windows;
using ModernWpf;
using MessageBox = ModernWpf.MessageBox;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Password_Manager
{
    public partial class SetPasswordWindow : Window
    {
        public SetPasswordWindow()
        {
            InitializeComponent();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            if (PasswordBox.Password.Length > 7)
            {
                if (PasswordBox.Password == ConfirmationPasswordBox.Password)
                {
                    Settings.Default.MasterPassword = GetSHA256Hash(PasswordBox.Password);
                    Settings.Default.Save();
                    MessageBox.Show("Sucessfully set password!");
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Passwords are not matching.");
                }
            }
            else
            {
                MessageBox.Show("The password is too short. Must be at least 8 characters long.");
            }
        }

        private string GetSHA256Hash(string password)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));
                foreach (byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }

        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ConfirmationPasswordBox.Focus();
            }
        }

        private void ConfirmationPasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ValidatePassword();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.InitialDirectory = GetDownloadFolderPath();
                dialog.Filter = "Text files (*.txt)|*.txt";
                dialog.ShowDialog();
                string[] data = File.ReadAllLines(dialog.FileName);
                if (data.Length == 2)
                {
                    if (IsValidSHA256Hash(data[0]))
                    {
                        Settings.Default.MasterPassword = data[0];
                        Settings.Default.SavedPasswords = data[1];
                        Settings.Default.Save();
                        MessageBox.Show("Successfully imported your data!");
                        this.Hide();
                        return;
                    }
                }
                MessageBox.Show("The selected file is not a valid export file.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static bool IsValidSHA256Hash(string hash)
        {
            if (hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z"))
                return false;
            else
                return true;
        }

        public static string GetDownloadFolderPath()
        {
            return Convert.ToString(Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty));
        }
    }
}
