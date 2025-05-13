using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PhotoBooth_App.Service
{
    public static class AppConfig
    {
        public static string GetApiUrl()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonPath = Path.Combine(baseDir, "appsettings.json");
                string json = File.ReadAllText(jsonPath);
                var config = JObject.Parse(json);
                return config["ApiSettings"]?["BaseUrl"]?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đọc cấu hình API: " + ex.Message);
            }
        }
        public static string GetBoothId()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string jsonPath = Path.Combine(baseDir, "config.json");
                string json = File.ReadAllText(jsonPath);
                var config = JObject.Parse(json);
                return config["boothId"]?.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi đọc BoothId từ config.json: " + ex.Message);
            }
        }
    }
}
