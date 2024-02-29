using Microsoft.AspNetCore.Identity;

namespace MyIdentityApp.Web.Localizations
{
	public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
	{
		public override IdentityError DuplicateUserName(string userName)
		{
			//return base.DuplicateUserName(userName);
			return new IdentityError
			{
				Code = "DuplicateUserName",
				Description = $"{userName} kullanıcı adı daha önce başka bir kullanıcı tarafından alınmıştır."
			};
		}
		public override IdentityError DuplicateEmail(string email)
		{
			//return base.DuplicateEmail(email);
			return new IdentityError
			{
				Code = "DuplicateEmail",
				Description = $"{email} mail adresine ait başka bir hesap vardır."
			};
		}
		public override IdentityError PasswordTooShort(int length)
		{
			//return base.PasswordTooShort(length);
			return new IdentityError
			{
				Code = "PasswordTooShort",
				Description = "Şifre en az 6 karakterden oluşmalıdır."
			};
		}
	}
}
