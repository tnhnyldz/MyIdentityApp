using System.ComponentModel.DataAnnotations;

namespace MyIdentityApp.Web.ViewModels
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]	
		[Required(ErrorMessage = "Şifre alanı boş olamaz.")]
		[Display(Name = "Yeni şifre :")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Girilen şifreler eşleşmemektedir.")]
		[Required(ErrorMessage = "Şifre tekrarı alanı boş olamaz.")]
		[Display(Name = "Yeni şifre tekrar :")]
		public string PasswordConfirm { get; set; }
	}
}
