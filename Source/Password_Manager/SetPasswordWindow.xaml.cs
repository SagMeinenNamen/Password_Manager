using Password_Manager.Properties;
using System;
using System.IO;
using System.Windows;
using ModernWpf;
using MessageBox = ModernWpf.MessageBox;
using System.Security.Cryptography;
using System.Text;

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
                MessageBox.Show("The password is too short. Needs to be at least 8 characters.");
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
    }
}
