using Homework1.Models.ViewModels;
using System.Text.Json.Serialization;

namespace Homework1.Services
{
    public class CityService
    {
        // 從 travel.js 抓取資料
        public string GetCities()
        {
            // 從資料夾中抓取 travel.js
            var json = File.ReadAllText("wwwroot/js/travel.js");

            var cities = new List<CitiesVm>();
            return json;
        }
    }
}
