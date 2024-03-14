using MyIdentityApp.Web.CustomValidations;
using MyIdentityApp.Web.Localizations;
using MyIdentityApp.Web.Models;

namespace MyIdentityApp.Web.Extensions
{
	public static class StartupExtensions
	{
		public static void AddIdentityWithExt(this IServiceCollection services)
		{
			//adding identity
			services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789-._";

				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = false;
				options.Password.RequireDigit = false;

				//user lockout on failure settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
				options.Lockout.MaxFailedAccessAttempts = 7;

			}).AddPasswordValidator<PasswordValidator>()
			.AddUserValidator<UserValidator>()
			.AddErrorDescriber<LocalizationIdentityErrorDescriber>()
			.AddEntityFrameworkStores<AppDbContext>();
		}
	}
}
