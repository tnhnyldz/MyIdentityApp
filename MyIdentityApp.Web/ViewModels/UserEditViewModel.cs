using MyIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace MyIdentityApp.Web.ViewModels
{
	public class UserEditViewModel
	{
		[Required(ErrorMessage = "Kullanıcı adı alanı boş olamaz.")]
		[Display(Name = "Kullanıcı Adı:")]
		public string UserName { get; set; }

		[EmailAddress(ErrorMessage = "Email formatı yanlış.")]
		[Required(ErrorMessage = "Email alanı boş olamaz.")]
		[Display(Name = "Email:")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Telefon alanı boş olamaz.")]
		[Display(Name = "Telefon:")]
		public string Phone { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Doğum Tarihi:")]
		public DateTime? BirthDate { get; set; }

		[Display(Name = "Şehir:")]
		public string? City { get; set; }

		[Display(Name = "Profil Resmi:")]
		public IFormFile? Picture { get; set; }

		[Display(Name = "Cinsiyet:")]
		public Gender? Gender { get; set; }

	}
}
