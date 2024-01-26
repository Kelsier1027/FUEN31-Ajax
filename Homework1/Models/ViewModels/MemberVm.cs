#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Homework1.Models.ViewModels
{
	public class MemberVm
	{
		[Display(Name = "姓名:")]
		[Required(ErrorMessage = "姓名為必填項目")]
		[StringLength(30, ErrorMessage = "姓名長度不能超過100個字符")]
		public string Name { get; set; }

		[Display(Name = "電子郵件:")]
		[Required(ErrorMessage = "電子郵件為必填項目")]
		[EmailAddress(ErrorMessage = "無效的電子郵件地址")]
		public string Email { get; set; }

		[Display(Name = "年紀:")]
		[Range(0, 150, ErrorMessage = "年齡必須在0至150之間")]
		public int? Age { get; set; }

		[Display(Name = "密碼:")]
		[Required(ErrorMessage = "密碼為必填項目")]
		[StringLength(100, MinimumLength = 6, ErrorMessage = "密碼至少需要6個字符")]
		public string Password { get; set; }

		[Display(Name = "確認密碼:")]
		[Compare("Password", ErrorMessage = "密碼與確認密碼不一致")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "頭像:")]
		public IFormFile? Avatar { get; set; }

	}
}
