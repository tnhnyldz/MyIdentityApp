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

		[EmailAddress(ErrorMessage ="Email formatı yanlış.")]
		[Required(ErrorMessage = "Email alanı boş olamaz.")]
		[Display(Name = "Email :")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Telefon alanı boş olamaz.")]
		[Display(Name = "Telefon :")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Şifre alanı boş olamaz.")]
		[Display(Name = "Şifre :")]
		public string Password { get; set; }

		[Compare(nameof(Password),ErrorMessage ="Girilen şifreler eşleşmemektedir.")]
		[Required(ErrorMessage = "Şifre tekrarı alanı boş olamaz.")]
		[Display(Name = "Şifre tekrar :")]
		public string PasswordConfirm { get; set; }
	}
}
