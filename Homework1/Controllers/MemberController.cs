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
        // 抓取資料庫資料，確認姓名是否存在
        public IActionResult CheckAccountName(string name)
        {
            var isExist = _dbContext.Members.Any(x => x.Name == name);
            var errorMsg = isExist ? "此姓名已存在" : "";

            return Json(isExist);
        }
    }
}
