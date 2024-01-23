using Homework1.Models;
using Homework1.Models.ViewModels;
using Homework1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _dbContext;

        public ApiController(MyDBContext dbDontext)

        {

			_dbContext = dbDontext;

        }
        public IActionResult Index()
        {
            
            return Content("你好","text/html",System.Text.Encoding.UTF8 );
        }

        public IActionResult Cities()
        {
           var citise = _dbContext.Addresses.Select(x => x.City).Distinct();
            return Json(citise);
        }
        
        public IActionResult Avatar(int id=1)
        {
            Member? member = _dbContext.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                return File(img, "image/jpeg");
            }
            return NotFound();
        }
        public IActionResult First()
        {
			return View();
		}
 
    }
}
