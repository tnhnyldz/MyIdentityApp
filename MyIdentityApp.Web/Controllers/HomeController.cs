﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyIdentityApp.Web.Models;
using MyIdentityApp.Web.ViewModels;
using System.Diagnostics;

namespace MyIdentityApp.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> _userManager;

		public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
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

			foreach (IdentityError item in identityResult.Errors)
			{
				ModelState.AddModelError(string.Empty, item.Description);
			}

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
