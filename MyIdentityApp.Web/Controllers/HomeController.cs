using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyIdentityApp.Web.Models;
using MyIdentityApp.Web.ViewModels;
using System.Diagnostics;
using MyIdentityApp.Web.Extensions;

namespace MyIdentityApp.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
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

		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null) //model comes with requests body, returnUrl comes with request url
		{
			//returnUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : Url.Action("Index", "Home");
			returnUrl = returnUrl ?? Url.Action("Index", "Home"); //one of url class static methods

			var hasUser = await _userManager.FindByEmailAsync(model.Email);

			if (hasUser is null)
			{
				ModelState.AddModelError(string.Empty, "Email veya Şifre yanlış.");
				return View();
			}

			var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, false);

			if (signInResult.Succeeded)
			{
				return Redirect(returnUrl);
			}

			ModelState.AddModelErrorList(new List<string>() { "Email veya şifre yanlış" });

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
