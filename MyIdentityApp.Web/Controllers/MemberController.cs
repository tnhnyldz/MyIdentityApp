using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyIdentityApp.Web.Extensions;
using MyIdentityApp.Web.Models;
using MyIdentityApp.Web.ViewModels;

namespace MyIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<MemberController> _logger;

		public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<MemberController> logger)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_logger = logger;
		}
		public async Task<IActionResult> Index()
		{
			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

			var userViewModel = new UserViewModel
			{
				Email = currentUser!.Email,
				UserName = currentUser.UserName,
				PhoneNumber = currentUser.PhoneNumber,
			};

			return View(userViewModel);
		}
		public async Task<IActionResult> PasswordChange()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

			var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

			if (!checkOldPassword)
			{
				ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
				return View();
			}

			var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

			if (!resultChangePassword.Succeeded)
			{
				ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
				return View();
			}

			await _userManager.UpdateSecurityStampAsync(currentUser);
			await _signInManager.SignOutAsync();
			await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

			TempData["SucseedMessage"] = $"Şifreniz yenildendi yeni şifreniz: {request.PasswordNew}";


			return View();
		}
		public async Task LogOut()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
