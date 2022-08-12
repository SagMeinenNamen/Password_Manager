using Newtonsoft.Json;
using Password_Manager.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows;

namespace Password_Manager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (string.IsNullOrWhiteSpace(Settings.Default.SavedPasswords))
            {
                Settings.Default.SavedPasswords = JsonConvert.SerializeObject(new Dictionary<string, string>());
                Settings.Default.Save();
            }
            if (string.IsNullOrWhiteSpace((string)Settings.Default["MasterPassword"]))
            {
                SetPasswordWindow setPasswordWindow = new SetPasswordWindow();
                setPasswordWindow.ShowDialog();
            }
        }
    }
}
