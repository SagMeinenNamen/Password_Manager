using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // StartupUri="MainWindow.xaml"
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string pathForPW = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Manager";
            Directory.CreateDirectory(pathForPW);
            if (!File.Exists(pathForPW + "\\trowssap.txt"))
            {
                Window1 passwortEingeben = new Window1();
                passwortEingeben.ShowDialog();
                //passwortEingeben.Close();

            }

           


        }

    }
}
