using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace MMS_Lab_1
{
    public class Config
    {
        public int StackCapacity { get; set; }

        public Config()
        {

        }

        public void LoadConfig(String filePath)
        {
            FileIniDataParser parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            try
            {
                StackCapacity = Convert.ToInt32(data["Stack"]["StackCapacity"]);
            }
            catch (Exception e)
            {
                StackCapacity = 10;
            }
        }
    }
}
