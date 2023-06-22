using Password_Manager.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MessageBox = ModernWpf.MessageBox;
using static Password_Manager.PasswordsModel;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Threading;
using Windows.UI.Xaml.Controls;
using Button = System.Windows.Controls.Button;
using ToolTip = System.Windows.Controls.ToolTip;
using System.Windows.Threading;
using ScrollViewer = System.Windows.Controls.ScrollViewer;
using Brushes = System.Windows.Media.Brushes;
using System.Threading.Tasks;
using Color = System.Windows.Media.Color;

namespace Password_Manager
{

    public partial class MainWindow : Window
    {
        readonly PasswordsModel model = new PasswordsModel();
        string randomPassword = string.Empty;
        string masterPassword = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            InitializeThemes();
            LengthSlider.ValueChanged -= Slider_ValueChanged;
            LengthSlider.Value = 16;
            LengthSlider.ValueChanged += Slider_ValueChanged;
            PasswordList.ItemsSource = model.Data;

        }

        private void InitializeThemes()
        {
            List<string> colorNames = new List<string>();
            ColorComboBox.ItemsSource = colorNames;
            var colors = typeof(Colors).GetProperties();
            colorNames.Add("Rainbow");
            foreach (var color in colors)
            {
                colorNames.Add(color.Name);
            }
            if (Settings.Default.RainbowMode)
                ColorComboBox.Text = "Rainbow";
            else
                ColorComboBox.Text = Settings.Default.PrimaryColor;

            Thread thread = new Thread(RainbowMode);
            thread.Start();
        }

        private async void RainbowMode()
        {
            var colors = GenerateRainbowColors();
            int i = 0;

            while (true)
            {
                if (Settings.Default.RainbowMode)
                {
                    await Task.Delay(20);
                    Settings.Default.PrimaryColor = colors[i].ToString();

                    if (i == 255)
                        i = 0;
                    else
                        i++;
                }
                else
                    await Task.Delay(300);
            }
        }

        private List<Color> GenerateRainbowColors()
        {
            List<Color> colors = new List<Color>();

            for (int i = 0; i < 256; i++)
            {
                double hue = i / 255.0 * 360;
                Color color = HsvToRgb(hue, 1, 1);
                colors.Add(color);
            }
            return colors;
        }

        private Color HsvToRgb(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromRgb((byte)v, (byte)t, (byte)p);
            else if (hi == 1)
                return Color.FromRgb((byte)q, (byte)v, (byte)p);
            else if (hi == 2)
                return Color.FromRgb((byte)p, (byte)v, (byte)t);
            else if (hi == 3)
                return Color.FromRgb((byte)p, (byte)q, (byte)v);
            else if (hi == 4)
                return Color.FromRgb((byte)t, (byte)p, (byte)v);
            else
                return Color.FromRgb((byte)v, (byte)p, (byte)q);
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
                GenerateButton_Click(null, null);
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            randomPassword = string.Empty;
            Random rnd = new Random();
            string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if ((bool)NumbersBox.IsChecked)
                characters += "0123456789";

            if ((bool)SpecialCharactersBox.IsChecked)
                characters += "!#$%()*+,-./:;=?@[]^_{}|~";

            var charactersArr = characters.ToCharArray();
            for (int i = 0; i < LengthSlider.Value; i++)
            {
                randomPassword += charactersArr[rnd.Next(0, charactersArr.Length)];
            }
            OutputTextBlock.Text = randomPassword;
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
                    AuthenticatedTextBlock.Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#FF00CD13");
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
            string plainTextPassword = OutputTextBlock.Text;
            string name = NameTextBox.Text;

            if (!(masterPassword != string.Empty))
            {
                MessageBox.Show("Please authenticate before saving passwords.");
                return;
            }

            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                MessageBox.Show("Please generate a password first.");
                return;
            }


            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please give it a name.");
                return;
            }

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
            if (dict.ContainsKey(name))
            {
                MessageBox.Show("Please use a name that isn't already in use.");
                return;
            }

            string encryptedPassword = Encryption.Encrypt(plainTextPassword, masterPassword);
            dict.Add(name, encryptedPassword);
            model.Data.Add(new PasswordEntity(name));
            PasswordList.Items.Refresh();
            Settings.Default.SavedPasswords = JsonConvert.SerializeObject(dict);
            Settings.Default.Save();
            NameTextBox.Text = string.Empty;
            OutputTextBlock.Text = string.Empty;
        }

        private void DeleteBtn_Clicked(object sender, RoutedEventArgs e)
        {
            string name = (string)((Button)sender).Tag;
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
            model.Data.RemoveAll(x => x.Name == name);
            PasswordList.Items.Refresh();
            dict.Remove(name);
            Settings.Default.SavedPasswords = JsonConvert.SerializeObject(dict);
            Settings.Default.Save();
        }

        private void CopyButton_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = (string)((Button)sender).Tag;
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Settings.Default.SavedPasswords);
                string encryptedPassword = dict[name];
                string plainTextPassword = Encryption.Decrypt(encryptedPassword, masterPassword);
                Clipboard.SetText(plainTextPassword);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                CheckPassword();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Environment.Exit(0);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            if (masterPassword != string.Empty)
            {
                string downloadsPath = GetDownloadFolderPath();
                string destination = downloadsPath + "\\Export_" + DateTime.Now.ToString("dd_MM_yy-HH_mm_ss_fff") + ".txt";
                string data = Settings.Default.MasterPassword + "\n" + Settings.Default.SavedPasswords;
                File.WriteAllText(destination, data);
                Process.Start(downloadsPath);
            }
            else
            {
                MessageBox.Show("Please authenticate to export your data.");
            }
        }

        public static string GetDownloadFolderPath()
        {
            return Convert.ToString(Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty));
        }

        private void CopyButton_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            ToolTip toolTip = (ToolTip)button.ToolTip;
            toolTip.PlacementTarget = button;
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
            toolTip.IsOpen = true;
            toolTip.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            toolTip.Foreground = Brushes.White;
            toolTip.FontSize = 12;
            toolTip.FontWeight = FontWeights.SemiBold;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(800)
            };
            timer.Tick += (_, __) => CloseToolTip(toolTip, timer);
            timer.Start();

        }

        private static void CloseToolTip(ToolTip toolTip, DispatcherTimer timer)
        {
            timer.Stop();
            toolTip.IsOpen = false;
        }

        private void ColorComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var color = e.AddedItems[0].ToString();
            if(color != "Rainbow")
            {
                Settings.Default.RainbowMode = false;
                Thread.Sleep(50);
                Settings.Default.PrimaryColor = color;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.RainbowMode = true;
                Settings.Default.Save();
            }
        }
    }
}