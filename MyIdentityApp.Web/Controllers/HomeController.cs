using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyIdentityApp.Web.Models;
using MyIdentityApp.Web.ViewModels;
using System.Diagnostics;
using MyIdentityApp.Web.Extensions;
using MyIdentityApp.Web.Services;
using NuGet.Common;

namespace MyIdentityApp.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IEmailService _emailService;

		public HomeController(IEmailService emailService, ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
			_emailService = emailService;
		}

		[HttpGet]
		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			//will go validator classes from now
			var identityResult = await _userManager.CreateAsync(
				new AppUser
				{
					UserName = request.UserName,
					Email = request.Email,
					PhoneNumber = request.Phone,
				}, request.PasswordConfirm
				);

			if (identityResult.Succeeded)
			{
				TempData["SucseedMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleştirilmiştir.";
				return RedirectToAction(nameof(HomeController.SignUp));
			}

			ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

			return View();
		}

		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel request)
		{
			var hasUser = await _userManager.FindByEmailAsync(request.Email);
			if (hasUser is null)
			{
				ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");
				return View();
			}

			string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
			var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

			await _emailService.SendResetPasswordEmail(passwordResetLink!, request.Email);

			TempData["SucseedMessage"] = "Şifre yenileme link email adresinize gönderilmiştir.";

			return RedirectToAction(nameof(ForgotPassword));
		}
		[HttpGet]
		public IActionResult ResetPassword(string userId, string token)
		{
			TempData["userId"] = userId;
			TempData["token"] = token;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
		{
			var userId = TempData["userId"];
			var token = TempData["token"];

			if (userId is null || token is null)
			{
				throw new Exception("Bir hata meydana geldi");
			}

			var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);

			if (hasUser is null)
			{
				ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır");
				return View();
			}

			IdentityResult result = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, request.Password);

			if (result.Succeeded)
			{
				TempData["SucseedMessage"] = $"Şifreniz yenildendi yeni şifreniz: {request.Password}";
			}
			else
			{
				ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
				return View();
			}
			return View();
		}

		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null) //model comes with requests body, returnUrl comes with request url
		{
			//returnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index", "Home");
			returnUrl ??= Url.Action("Index", "Home"); //one of url class static methods

			var hasUser = await _userManager.FindByEmailAsync(model.Email);

			if (hasUser is null)
			{
				ModelState.AddModelError(string.Empty, "Email veya Şifre yanlış.");
				return View();
			}

			var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);

			if (signInResult.Succeeded)
			{
				return Redirect(returnUrl!);
			}
			if (signInResult.IsLockedOut)
			{
				ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca deneme yapamazsınız." });
				return View();
			}

			ModelState.AddModelErrorList(new List<string>() { "Email veya şifre yanlış.", $"Başarısız giriş sayısı({await _userManager.GetAccessFailedCountAsync(hasUser)})" });

			return View();
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
