using Homework1.Models;
using Homework1.Models.ViewModels;
using Homework1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homework1.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _dbContext;
        private readonly IWebHostEnvironment _host;
        public ApiController(MyDBContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            _host = host;
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

        public IActionResult Address()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Member member, IFormFile Avatar)
        {
            string fileName = "empty.jpg";
            if (Avatar != null)
            {
                fileName = Avatar.FileName;
            }

            //取得檔案上傳的實際路徑
            string uploadPath = Path.Combine(_host.WebRootPath, "uploads", fileName);
            //檔案上傳
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                Avatar?.CopyTo(fileStream);
            }

            //轉成二進位
            byte[]? imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                Avatar?.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }

            member.FileName = fileName;
            member.FileData = imgByte;

            //新增
            _dbContext.Members.Add(member);
            _dbContext.SaveChanges();




            return Content("新增成功");

            // return Content($"Hello {_user.Name}, {_user.Age}歲了,電子郵件是{_user.Email}");
            //return Content($"{_user.Avatar?.FileName}-{_user.Avatar?.Length}-{_user.Avatar?.ContentType}");
        }
    }
}
