using Homework1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controllers
{
    public class MemberController : Controller
    {
        private readonly MyDBContext _dbContext;

        public MemberController(MyDBContext dbDontext)
        {
            _dbContext = dbDontext;
        }

        public IActionResult Index()
        {
            return View();
        }

        // 註冊頁面
        public IActionResult Register()
        {
            return View();
        }

    }
}
