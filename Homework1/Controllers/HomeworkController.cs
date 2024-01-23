using Homework1.Models.ViewModels;
using Homework1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controllers
{
    public class HomeworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // 顯示所有城市的簡介
        public IActionResult Homework1()
        {
            return View();
        }
    }
}
