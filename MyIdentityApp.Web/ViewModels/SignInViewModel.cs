using System.ComponentModel.DataAnnotations;

namespace MyIdentityApp.Web.ViewModels
{
	//req yapıldıgında bodydekı data SignInViewModele maplenir
	public class SignInViewModel
	{
		public SignInViewModel()
		{

		}
		public SignInViewModel(string email, string password)
		{
			Email = email;
			Password = password;
		}

		[EmailAddress(ErrorMessage = "Email formatı yanlış.")]
		[Required(ErrorMessage = "Email adı alanı boş olamaz.")]
		[Display(Name = "Email :")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Şifre alanı boş olamaz.")]
		[Display(Name = "Şifre :")]
		public string Password { get; set; }

		[Display(Name = "Beni hatırla ")]
		public bool RememberMe { get; set; }
    }
}
