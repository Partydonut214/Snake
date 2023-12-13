using System;
using System.IO;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace Snake
{
    /// <summary>
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : Window
    {
        //Default JSON Data
        string jsonData = @"{
   squaresPerSecond : ""10"",

   rowsCount : ""15"",
   colsCount : ""15"",
   
   colors: [
   {""name"": ""background"", ""value"": ""#211E2B""},
   {""name"": ""gridBackground"", ""value"": ""#312C40""},
   {""name"": ""textColor"", ""value"": ""#EBEBEB""},
   {""name"": ""overlayColor"", ""value"": ""#7f000000""}
   ]
}";
        string jsonFile = @$"C:/Users/{Environment.UserName}/Documents/Snake/snakeConfig.json";

        private GameSettings LoadSettings(string file, string backupFile)
        {
            GameSettings settings = new GameSettings();
            if (File.Exists(file))
            {
                settings = JsonConvert.DeserializeObject<GameSettings>(file);
            }
            else
            {
                //CreatePath(file);
                settings = JsonConvert.DeserializeObject<GameSettings>(backupFile);
            }
            return settings;
        }
        
        private void CreatePath(string file)
        {
            Directory.CreateDirectory(file);
            File.Create(file);
            TextWriter tw = new StreamWriter(file);
            tw.WriteLine("{\r\n   squaresPerSecond : \"10\",\r\n\r\n   rowsCount : \"15\",\r\n   colsCount : \"15\",\r\n   \r\n   colors: [\r\n   {\"name\": \"background\", \"value\": \"#211E2B\"},\r\n   {\"name\": \"gridBackground\", \"value\": \"#312C40\"},\r\n   {\"name\": \"textColor\", \"value\": \"#EBEBEB\"},\r\n   {\"name\": \"overlayColor\", \"value\": \"#7f000000\"}\r\n   ]\r\n}");
            tw.Close();
        }

        public Config()
        {
            GameSettings gameSettings = LoadSettings(jsonFile, jsonData);
            InitializeComponent();
        }

        private void configInput_SQ_s_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
