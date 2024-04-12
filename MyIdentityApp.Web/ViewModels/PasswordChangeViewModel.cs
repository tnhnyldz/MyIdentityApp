using System.ComponentModel.DataAnnotations;

namespace MyIdentityApp.Web.ViewModels
{
	public class PasswordChangeViewModel
	{
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı boş olamaz.")]
		[Display(Name = "Eski şifre :")]
		public string PasswordOld { get; set; }= null!;

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Yeni şifre alanı boş olamaz.")]
		[Display(Name = "Yeni şifre :")]
		[MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")]
		public string PasswordNew { get; set; } = null!;

		[DataType(DataType.Password)]
		[Compare(nameof(PasswordNew), ErrorMessage = "Girilen şifreler eşleşmemektedir.")]
		[Required(ErrorMessage = "Yeni şifre tekrarı alanı boş olamaz.")]
		[Display(Name = "Yeni şifre tekrar :")]
		[MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")]
		public string PasswordNewConfirm { get; set; } = null!;
	}
}
