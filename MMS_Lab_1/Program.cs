using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMS_Lab_1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!File.Exists("Config.ini"))
            {
                FileStream file = new FileStream("Config.ini", FileMode.Create);
                String defaultSettings = "[Stack]\r\nStackCapacity = 10";
                file.Write(Encoding.Default.GetBytes(defaultSettings), 0, defaultSettings.Length);
                file.Close();
            }
            Application.Run(new frmMain());
        }
    }
}
