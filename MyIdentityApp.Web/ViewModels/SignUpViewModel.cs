using System.ComponentModel.DataAnnotations;

namespace MyIdentityApp.Web.ViewModels
{
	public class SignUpViewModel
	{
		public SignUpViewModel()
		{

		}
		public SignUpViewModel(string userName, string email, string phone, string password, string passwordConfirm)
		{
			UserName = userName;
			Email = email;
			Phone = phone;
			Password = password;
			PasswordConfirm = passwordConfirm;
		}
		[Required(ErrorMessage = "Kullanıcı adı alanı boş olamaz.")]
		[Display(Name = "Kullanıcı Adı :")]
		public string UserName { get; set; }

		[EmailAddress(ErrorMessage = "Email formatı yanlış.")]
		[Required(ErrorMessage = "Email alanı boş olamaz.")]
		[Display(Name = "Email :")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Telefon alanı boş olamaz.")]
		[Display(Name = "Telefon :")]
		public string Phone { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı boş olamaz.")]
		[Display(Name = "Şifre :")]
		[MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Girilen şifreler eşleşmemektedir.")]
		[Required(ErrorMessage = "Şifre tekrarı alanı boş olamaz.")]
		[Display(Name = "Şifre tekrar :")]
		[MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")]
		public string PasswordConfirm { get; set; }
	}
}
