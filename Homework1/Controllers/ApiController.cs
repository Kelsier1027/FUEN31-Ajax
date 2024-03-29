﻿using Fuen31Site.Models.DTO;
using Homework1.Models;
using Homework1.Models.ViewModels;
using Homework1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

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

			// 產生 salt 方法
			member.Salt = Guid.NewGuid().ToString();

			// 產生 hash 方法
			member.Password = HashPassword(member.Password, member.Salt);

			string HashPassword(string? password, string salt)
			{
				// 透過 SHA256 產生 Hash 值
				using var sha256 = SHA256.Create();
				var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
				return Convert.ToBase64String(bytes);
			}


			//新增
			_dbContext.Members.Add(member);
            _dbContext.SaveChanges();




            return Content("新增成功");

            // return Content($"Hello {_user.Name}, {_user.Age}歲了,電子郵件是{_user.Email}");
            //return Content($"{_user.Avatar?.FileName}-{_user.Avatar?.Length}-{_user.Avatar?.ContentType}");
        }
        [HttpPost]
        public IActionResult Spots([FromBody] SearchDTO _search)
        {
            //根據分類編號讀取景點資料
            var spots = _search.categoryId == 0 ? _dbContext.SpotImagesSpots : _dbContext.SpotImagesSpots.Where(s => s.CategoryId == _search.categoryId);//如果categoryId=0就讀取全部資料 //如果categoryId不等於0就讀取指定分類的資料 

            //根據關鍵字搜尋
            if (!string.IsNullOrEmpty(_search.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(_search.keyword) || s.SpotDescription.Contains(_search.keyword));
            }

            //排序
            switch (_search.sortBy)
            {
                case "spotTitle":
                    spots = _search.sortType == "asc" ? spots.OrderBy(s => s.SpotTitle) : spots.OrderByDescending
                        (s => s.SpotTitle);
                    break;
                case "categoryId":
                    spots = _search.sortType == "asc" ? spots.OrderBy(s => s.CategoryId) : spots.OrderByDescending
                       (s => s.CategoryId);
                    break;
                default:
                    spots = _search.sortType == "asc" ? spots.OrderBy(s => s.SpotId) : spots.OrderByDescending
                       (s => s.SpotId);
                    break;
            }

            //分頁
            int TotalCount = spots.Count(); //搜尋出來的資料總共有幾筆
            int pageSize = _search.pageSize ?? 9; //每頁多少筆資料
            int TotalPages = (int)Math.Ceiling((decimal)TotalCount / pageSize); //計算出總共有幾頁
            int page = _search.Page ?? 1; //第幾頁

            //取出分頁資料
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);

            //設計要回傳的資料格式
            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalPages = TotalPages;
            spotsPaging.SpotsReslut = spots.ToList();

            return Json(spotsPaging);
        }

        public IActionResult CheckName(string name)
        {
			var member = _dbContext.Members.FirstOrDefault(m => m.Name == name);
			if (member != null)
            {
				return Json(true);
			}
			return Json(false);
		}

        public IActionResult SpotsTitle(string keyword)
        {
            var spots = _dbContext.Spots.Where(s => s.SpotTitle.Contains(keyword)).Select(s => s.SpotTitle).Take(8);
            return Json(spots);
        }

        // 取得景點分類對應的 categoryId
        public IActionResult SpotCategories()
        {
			var categories = _dbContext.Categories.Select(c => new { c.CategoryId, c.CategoryName });
			return Json(categories);
		}
       
	}
}
