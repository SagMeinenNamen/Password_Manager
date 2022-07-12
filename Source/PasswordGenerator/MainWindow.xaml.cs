using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PasswordGenerator
{
    public partial class MainWindow : Window
    {

        bool eingeloggt = false;
        string random = null;
        public MainWindow()
        {
            InitializeComponent();
            Title = "Passwort Manager";
            loggedIn.Visibility = Visibility.Hidden;
            loggedOut.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Visible;
            checkBox.Visibility = Visibility.Visible;
            pw1.Visibility = Visibility.Hidden;
            pw2.Visibility = Visibility.Hidden;
            pw3.Visibility = Visibility.Hidden;
            pw4.Visibility = Visibility.Hidden;
            pw5.Visibility = Visibility.Hidden;
            de1.Visibility = Visibility.Hidden;
            de2.Visibility = Visibility.Hidden;
            de3.Visibility = Visibility.Hidden;
            de4.Visibility = Visibility.Hidden;
            de5.Visibility = Visibility.Hidden;
            copy1.Visibility = Visibility.Hidden;
            copy2.Visibility = Visibility.Hidden;
            copy3.Visibility = Visibility.Hidden;
            copy4.Visibility = Visibility.Hidden;
            copy5.Visibility = Visibility.Hidden;
            string path2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            if (File.Exists(path2 + "\\name1.txt"))
            {
                pw1.Text = File.ReadAllText(path2 + "\\name1.txt");
            }
            else
            {
                pw1.Text = "Passwort 1";
            }
            string path3 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            if (File.Exists(path2 + "\\name2.txt"))
            {
                pw2.Text = File.ReadAllText(path2 + "\\name2.txt");
            }
            else
            {
                pw2.Text = "Passwort 2";
            }
            string path4 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            if (File.Exists(path2 + "\\name3.txt"))
            {
                pw3.Text = File.ReadAllText(path2 + "\\name3.txt");
            }
            else
            {
                pw3.Text = "Passwort 3";
            }
            string path5 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            if (File.Exists(path2 + "\\name4.txt"))
            {
                pw4.Text = File.ReadAllText(path2 + "\\name4.txt");
            }
            else
            {
                pw4.Text = "Passwort 4";
            }
            string path6 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            if (File.Exists(path2 + "\\name5.txt"))
            {
                pw5.Text = File.ReadAllText(path2 + "\\name5.txt");
            }
            else
            {
                pw5.Text = "Passwort 5";
            }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            if (File.Exists(path + "\\passwordManager1.txt"))
            {
                pw1.Visibility = Visibility.Visible;
                copy1.Visibility = Visibility.Visible;
                de1.Visibility = Visibility.Visible;
            }
            if (File.Exists(path + "\\passwordManager2.txt"))
            {
                pw2.Visibility = Visibility.Visible;
                copy2.Visibility = Visibility.Visible;
                de2.Visibility = Visibility.Visible;
            }
            if (File.Exists(path + "\\passwordManager3.txt"))
            {
                pw3.Visibility = Visibility.Visible;
                copy3.Visibility = Visibility.Visible;
                de3.Visibility = Visibility.Visible;
            }
            if (File.Exists(path + "\\passwordManager4.txt"))
            {
                pw4.Visibility = Visibility.Visible;
                copy4.Visibility = Visibility.Visible;
                de4.Visibility = Visibility.Visible;
            }
            if (File.Exists(path + "\\passwordManager5.txt"))
            {
                pw5.Visibility = Visibility.Visible;
                copy5.Visibility = Visibility.Visible;
                de5.Visibility = Visibility.Visible;
            }
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderValue != null)
            {
                double passwordDouble = passwordLength.Value;
                double passwordRounded = Math.Round(passwordDouble);
                string passwordString = Convert.ToString(passwordRounded);
                for (int i = 0; i < passwordLength.Value; i++)
                {
                    int h = 1;
                    int hi = h + i;
                    sliderValue.Text = Convert.ToString(hi);
                }   
            }
        }
        private void copyButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(outputBox.Text == string.Empty)
                MessageBox.Show("Bitte generiere zuerst ein Passwort um es in die Zwischenablage zu speichern!");
            else
                Clipboard.SetText(Convert.ToString(random));
        }
        private void generateButton_Click(object sender, RoutedEventArgs e)
        { 

            //for (int i = 0; i < 10; i++)
            //{
            //    loadingBar.Value++;
            //    Task.Delay(1000).Wait();
            //    this.UpdateLayout();
            //}
            random = null;
            Random rnd = new Random();
            string[] zeichen = new string[87];
            zeichen[0] = "1";
            zeichen[1] = "2";
            zeichen[2] = "3";
            zeichen[3] = "4";
            zeichen[4] = "5";
            zeichen[5] = "6";
            zeichen[6] = "7";
            zeichen[7] = "8";
            zeichen[8] = "9";
            zeichen[9] = "0";
            zeichen[10] = "!";
            zeichen[11] = "#";
            zeichen[12] = "$";
            zeichen[13] = "%";
            zeichen[14] = "(";
            zeichen[15] = ")";
            zeichen[16] = "*";
            zeichen[17] = "+";
            zeichen[18] = ",";
            zeichen[19] = "-";
            zeichen[20] = ".";
            zeichen[21] = "/";
            zeichen[22] = ":";
            zeichen[23] = ";";
            zeichen[24] = "=";
            zeichen[25] = "?";
            zeichen[26] = "@";
            zeichen[27] = "[";
            zeichen[28] = "]";
            zeichen[29] = "^";
            zeichen[30] = "_";
            zeichen[31] = "{";
            zeichen[32] = "}";
            zeichen[33] = "|";
            zeichen[34] = "~";
            zeichen[35] = "a";
            zeichen[36] = "b";
            zeichen[37] = "c";
            zeichen[38] = "d";
            zeichen[39] = "e";
            zeichen[40] = "f";
            zeichen[41] = "g";
            zeichen[42] = "h";
            zeichen[43] = "i";
            zeichen[44] = "j";
            zeichen[45] = "k";
            zeichen[46] = "l";
            zeichen[47] = "m";
            zeichen[48] = "n";
            zeichen[49] = "o";
            zeichen[50] = "p";
            zeichen[51] = "q";
            zeichen[52] = "r";
            zeichen[53] = "s";
            zeichen[54] = "t";
            zeichen[55] = "u";
            zeichen[56] = "v";
            zeichen[57] = "w";
            zeichen[58] = "x";
            zeichen[59] = "y";
            zeichen[60] = "z";
            zeichen[61] = "A";
            zeichen[62] = "B";
            zeichen[63] = "C";
            zeichen[64] = "D";
            zeichen[65] = "E";
            zeichen[66] = "F";
            zeichen[67] = "G";
            zeichen[68] = "H";
            zeichen[69] = "I";
            zeichen[70] = "J";
            zeichen[71] = "K";
            zeichen[72] = "L";
            zeichen[73] = "M";
            zeichen[74] = "N";
            zeichen[75] = "O";
            zeichen[76] = "P";
            zeichen[77] = "Q";
            zeichen[78] = "R";
            zeichen[79] = "S";
            zeichen[80] = "T";
            zeichen[81] = "U";
            zeichen[82] = "V";
            zeichen[83] = "W";
            zeichen[84] = "X";
            zeichen[85] = "Y";
            zeichen[86] = "Z";
            for (int i = 0; i < passwordLength.Value; i++)
                random = random + zeichen[rnd.Next(0, 86)];
            outputBox.Text = random;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            outputBox.Text = random;
            string encrypt = random;
            if (encrypt != null)
            {
                encrypt = Encrypt.EncryptString(random, File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\trowssap.txt"));
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
                if (File.Exists(path + "\\passwordManager1.txt"))
                {
                    if (File.Exists(path + "\\passwordManager2.txt"))
                    {
                        if (File.Exists(path + "\\passwordManager3.txt"))
                        {
                            if (File.Exists(path + "\\passwordManager4.txt"))
                            {
                                if (File.Exists(path + "\\passwordManager5.txt"))
                                {

                                    MessageBox.Show("Du hast die maximale Anzahl erreicht, bitte lösche eins um noch eins speichern zu können.");
                                }
                                else
                                {
                                    pw5.Visibility = Visibility.Visible;
                                    copy5.Visibility = Visibility.Visible;
                                    de5.Visibility = Visibility.Visible;
                                    File.WriteAllText(path + "\\passwordManager5.txt", encrypt);
                                }

                            }
                            else
                            {
                                pw4.Visibility = Visibility.Visible;
                                copy4.Visibility = Visibility.Visible;
                                de4.Visibility = Visibility.Visible;
                                File.WriteAllText(path + "\\passwordManager4.txt", encrypt);
                            }

                        }
                        else
                        {
                            pw3.Visibility = Visibility.Visible;
                            copy3.Visibility = Visibility.Visible;
                            de3.Visibility = Visibility.Visible;
                            File.WriteAllText(path + "\\passwordManager3.txt", encrypt);
                        }

                    }
                    else
                    {
                        pw2.Visibility = Visibility.Visible;
                        copy2.Visibility = Visibility.Visible;
                        de2.Visibility = Visibility.Visible;
                        File.WriteAllText(path + "\\passwordManager2.txt", encrypt);
                    }
                }
                else
                {
                    pw1.Visibility = Visibility.Visible;
                    copy1.Visibility = Visibility.Visible;
                    de1.Visibility = Visibility.Visible;
                    File.WriteAllText(path + "\\passwordManager1.txt", encrypt);
                }
            }
            else
            {
                MessageBox.Show("Bitte generiere zuerst ein Passwort um es zu speichern!");
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(1);
        }
        private void copyPassword1(object sender, RoutedEventArgs e)
        {
            if(eingeloggt == false)
            {
                MessageBox.Show("Bitte melde dich dafür zuerst an!");

            }
            else
            {
                eingeloggt = true;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) ;
                string decrypt = Encrypt.DecryptString(File.ReadAllText(path + "\\Manager\\passwordManager1.txt"), File.ReadAllText(path + "\\Manager\\trowssap.txt"));
                Clipboard.SetText(decrypt);
                MessageBox.Show("Passwort für " + pw1.Text +   " wurde in deine Zwischenablage gespeichert.");
            }
        }
        private void copyPassword2(object sender, RoutedEventArgs e)
        {
            if (eingeloggt == false)
            {
                MessageBox.Show("Bitte melde dich dafür zuerst an!");
            }
            else
            {
                eingeloggt = true;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string decrypt = Encrypt.DecryptString(File.ReadAllText(path + "\\Manager\\passwordManager2.txt"), File.ReadAllText(path + "\\Manager\\trowssap.txt"));
                Clipboard.SetText(decrypt);
                MessageBox.Show("Passwort für " + pw2.Text + " wurde in deine Zwischenablage gespeichert.");
            }
        }
        private void copyPassword3(object sender, RoutedEventArgs e)
        {
            if (eingeloggt == false)
            {
                MessageBox.Show("Bitte melde dich dafür zuerst an!");

            }
            else
            {
                eingeloggt = true;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string decrypt = Encrypt.DecryptString(File.ReadAllText(path + "\\Manager\\passwordManager3.txt"), File.ReadAllText(path + "\\Manager\\trowssap.txt"));
                Clipboard.SetText(decrypt);
                MessageBox.Show("Passwort für " + pw3.Text + " wurde in deine Zwischenablage gespeichert.");
            }
        }
        private void copyPassword4(object sender, RoutedEventArgs e)
        {
            if (eingeloggt == false)
            {
                MessageBox.Show("Bitte melde dich dafür zuerst an!");
            }
            else
            {
                eingeloggt = true;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string decrypt = Encrypt.DecryptString(File.ReadAllText(path + "\\Manager\\passwordManager4.txt"), File.ReadAllText(path + "\\Manager\\trowssap.txt"));
                Clipboard.SetText(decrypt);
                MessageBox.Show("Passwort für " + pw4.Text + " wurde in deine Zwischenablage gespeichert.");
            }
        }
        private void copyPassword5(object sender, RoutedEventArgs e)
        {
            if (eingeloggt == false)
            {
                MessageBox.Show("Bitte melde dich dafür zuerst an!");
            }
            else
            {
                eingeloggt = true;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string decrypt = Encrypt.DecryptString(File.ReadAllText(path + "\\Manager\\passwordManager5.txt"), File.ReadAllText(path + "\\Manager\\trowssap.txt"));
                Clipboard.SetText(decrypt);
                MessageBox.Show("Passwort für " + pw5.Text + " wurde in deine Zwischenablage gespeichert.");
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            try
            {
                string savedPassword = Encrypt.DecryptString(File.ReadAllText(path + "\\Manager\\trowssap.txt"), PasswordBox.Text);
                string enteredPassword = PasswordBox.Text;
                if (savedPassword == enteredPassword)
                {
                    eingeloggt = true;
                    loggedIn.Visibility = Visibility.Visible;
                    loggedOut.Visibility = Visibility.Hidden;
                    PasswordBox.Visibility = Visibility.Hidden;
                    checkBox.Visibility = Visibility.Hidden;
                }
            }
            catch
            {
                MessageBox.Show("Falsches Passwort!");
            }
        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.Text = "";
        }
        private void delete1(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            File.Delete(path + "\\passwordManager1.txt");
            pw1.Visibility = Visibility.Hidden;
            de1.Visibility = Visibility.Hidden;
            copy1.Visibility = Visibility.Hidden;
            pw1.Text = "Passwort 1";
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name1.txt";
            File.Delete(path1);
        }
        private void delete2(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            File.Delete(path + "\\passwordManager2.txt");
            copy2.Visibility = Visibility.Hidden;
            de2.Visibility = Visibility.Hidden;
            pw2.Visibility = Visibility.Hidden;
            pw2.Text = "Passwort 2";
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name2.txt";
            File.Delete(path1);
        }
        private void delete3(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            File.Delete(path + "\\passwordManager3.txt");
            copy3.Visibility = Visibility.Hidden;
            de3.Visibility = Visibility.Hidden;
            pw3.Visibility = Visibility.Hidden;
            pw3.Text = "Passwort 3";
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name3.txt";
            File.Delete(path1);
        }
        private void delete4(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            File.Delete(path + "\\passwordManager4.txt");
            copy4.Visibility = Visibility.Hidden;
            de4.Visibility = Visibility.Hidden;
            pw4.Visibility = Visibility.Hidden;
            pw4.Text = "Passwort 4";
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name4.txt";
            File.Delete(path1);
        }
        private void delete5(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            File.Delete(path + "\\passwordManager5.txt");
            copy5.Visibility = Visibility.Hidden;
            de5.Visibility = Visibility.Hidden;
            pw5.Visibility = Visibility.Hidden;
            pw5.Text = "Passwort 5";
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name5.txt";
            File.Delete(path1);
        }
        private void pw1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pw1.Text != "Passwort 1")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name1.txt";
                File.WriteAllText(path, pw1.Text);
            }
        }
        private void pw2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pw2.Text != "Passwort 2")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name2.txt";
                File.WriteAllText(path, pw2.Text);
            }
        }
        private void pw3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pw3.Text != "Passwort 3")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name3.txt";
                File.WriteAllText(path, pw3.Text);
            }
        }
        private void pw4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pw4.Text != "Passwort 4")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name4.txt";
                File.WriteAllText(path, pw4.Text);
            }
        }
        private void pw5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (pw5.Text != "Passwort 5")
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\name5.txt";
                File.WriteAllText(path, pw5.Text);
            }
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path4Explorer = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            Process.Start("explorer.exe", path4Explorer);
        }
    }
}