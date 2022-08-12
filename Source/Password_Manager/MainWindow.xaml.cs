using Password_Manager.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ModernWpf;
using MessageBox = ModernWpf.MessageBox;
using static Password_Manager.PasswordsModel;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Linq;

namespace Password_Manager
{
    public partial class MainWindow : Window
    {
        readonly PasswordsModel model = new PasswordsModel();
        string random = string.Empty;
        string masterPassword = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            LengthSlider.Value = 16;
            PasswordList.ItemsSource = model.Data;
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SliderValue != null)
            {
                SliderValue.Text = Convert.ToString(Math.Round(e.NewValue));
            }
        }
     
        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            random = string.Empty;
            Random rnd = new Random();
            var characters = "!#$%()*+,-./:;=?@[]^_{}|~0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < LengthSlider.Value; i++)
            {
                random += characters[rnd.Next(0, 86)];
            }
            OutputTextBlock.Text = random;
        }

        private void ConfirmBtn_Clicked(object sender, RoutedEventArgs e)
        {
            CheckPassword();
        }

        private void CheckPassword()
        {
            try
            {
                if (GetSHA256Hash(PasswordBox.Password) == Settings.Default.MasterPassword)
                {
                    masterPassword = PasswordBox.Password;
                    PasswordBox.Visibility = Visibility.Hidden;
                    ConfirmBtn.Visibility = Visibility.Hidden;
                    AuthenticatedTextBlock.Text = "Authenticated";
                    AuthenticatedTextBlock.Foreground = (Brush)new BrushConverter().ConvertFrom("#FF00CD13");
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
                    foreach (var entry in dict)
                    {
                        model.Data.Add(new PasswordEntity(entry.Key));
                    }
                    PasswordList.Items.Refresh();
                    VaultInfo.Visibility = Visibility.Collapsed;
                    return;
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SavePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (masterPassword != string.Empty)
            {
                string plainTextPassword = OutputTextBlock.Text;
                if (!string.IsNullOrWhiteSpace(plainTextPassword))
                {
                    string name = NameTextBox.Text;
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
                        if (!dict.ContainsKey(name))
                        {
                            string encryptedPassword = Encrypter.Encrypt(plainTextPassword, masterPassword);
                            dict.Add(name, encryptedPassword);
                            model.Data.Add(new PasswordEntity(name));
                            PasswordList.Items.Refresh();
                            Settings.Default.SavedPasswords = JsonConvert.SerializeObject(dict);
                            Settings.Default.Save();
                            NameTextBox.Text = string.Empty;
                        }
                        else
                        {
                            MessageBox.Show("Please use a name that isn't already in use.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please give it a name.");
                    }
                }
                else
                {
                    MessageBox.Show("Please generate a password first.");
                }
            }
            else
            {
                MessageBox.Show("Please enter your password first.");
            }
        }

        private void DeleteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            string name = (string)((Button)sender).Tag;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
            DeleteFromModel(name);
            PasswordList.Items.Refresh();
            dict.Remove(name);
            Settings.Default.SavedPasswords = JsonConvert.SerializeObject(dict);
            Settings.Default.Save();
        }

        private void DeleteFromModel(string name)
        {
            for (int i = 0; i < model.Data.Count; i++)
            {
                if (model.Data[i].Name == name)
                {
                    model.Data.RemoveAt(i);
                    break;
                }
            }
        }

        private void CopyButton_Clicked(object sender, RoutedEventArgs e)
        {
            string name = (string)((Button)sender).Tag;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
            string encryptedPassword = dict[name];
            string plainTextPassword = Encrypter.Decrypt(encryptedPassword, masterPassword);
            Clipboard.SetText(plainTextPassword);
            MessageBox.Show("Password copied to your clipboard.");
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineDown();
            }
            else
            {
                scrollViewer.LineUp();
            }
            e.Handled = true;
        }

        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                CheckPassword();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }
    }
}