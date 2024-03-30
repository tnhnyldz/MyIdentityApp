using System.ComponentModel.DataAnnotations;

namespace MyIdentityApp.Web.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[EmailAddress(ErrorMessage = "Email formatı yanlış.")]
		[Required(ErrorMessage = "Email adı alanı boş olamaz.")]
		[Display(Name = "Email :")]
		public string Email { get; set; }
	}
}
