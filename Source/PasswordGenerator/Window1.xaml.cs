using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Title = "Erstelle ein Passwort";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(pw1.Text.Length > 7)
            {
                if(pw1.Text == pw2.Text)
                {
                    string pathForPW = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager\\";
                    string trowssap = pw1.Text;
                    File.WriteAllText(pathForPW + "\\trowssap.txt" , Encrypt.EncryptString((trowssap), trowssap));
                    MessageBox.Show("Du hast dein neues Passwort erfolgreich gesetzt!");
                    //MainWindow mw = new MainWindow();
                    //mw.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Die Passwörter stimmen nicht überein.");
                }
            }
            else
            {
                MessageBox.Show("Das Passwort ist zu kurz. Es muss mindestens aus 8 Zeichen bestehen.");
            }
        }


    }
}
