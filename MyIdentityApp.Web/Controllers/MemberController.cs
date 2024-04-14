using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
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
		private readonly IFileProvider _fileProvider;

		public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<MemberController> logger, IFileProvider fileProvider)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_logger = logger;
			_fileProvider = fileProvider;
		}
		public async Task<IActionResult> Index()
		{
			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

			var userViewModel = new UserViewModel
			{
				Email = currentUser!.Email,
				UserName = currentUser.UserName,
				PhoneNumber = currentUser.PhoneNumber,
				PictureUrl=currentUser.Picture,
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

			var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

			var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

			if (!checkOldPassword)
			{
				ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
				return View();
			}

			var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld, request.PasswordNew);

			if (!resultChangePassword.Succeeded)
			{
				ModelState.AddModelErrorList(resultChangePassword.Errors);
				return View();
			}

			await _userManager.UpdateSecurityStampAsync(currentUser);
			await _signInManager.SignOutAsync();
			await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

			TempData["SucseedMessage"] = $"Şifreniz yenildendi yeni şifreniz: {request.PasswordNew}";

			return View();
		}

		public async Task<IActionResult> UserEdit()
		{
			//get all selectıons from enum
			ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(Gender)));

			var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;

			var userEditViewModel = new UserEditViewModel
			{
				UserName = currentUser.UserName!,
				Email = currentUser.Email!,
				Phone = currentUser.PhoneNumber!,
				BirthDate = currentUser.BirthDate,
				City = currentUser.City,
				Gender = currentUser.Gender,
			};
			return View(userEditViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> UserEdit(UserEditViewModel request)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			ViewBag.GenderList = new SelectList(Enum.GetNames(typeof(Gender)));

			var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

			currentUser.UserName = request.UserName;
			currentUser.Email = request.Email;
			currentUser.PhoneNumber = request.Phone;
			currentUser.BirthDate = request.BirthDate;
			currentUser.City = request.City;
			currentUser.Gender = request.Gender;

			if (request.Picture is not null && request.Picture.Length > 0)
			{
				var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
				var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}"; //.png
																												  //create picture path
				var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);
				using var stream = new FileStream(newPicturePath, FileMode.Create);
				await request.Picture.CopyToAsync(stream);
				currentUser.Picture = randomFileName;
			}

			var updateResult = await _userManager.UpdateAsync(currentUser);
			if (!updateResult.Succeeded)
			{
				ModelState.AddModelErrorList(updateResult.Errors);
				return View();
			}

			await _userManager.UpdateSecurityStampAsync(currentUser);
			await _signInManager.SignOutAsync();
			await _signInManager.SignInAsync(currentUser, true);

			TempData["SucseedMessage"] = $"Üyelik bilgileriniz güncellendi.";

			var userEditViewModel = new UserEditViewModel
			{
				UserName = currentUser.UserName!,
				Email = currentUser.Email!,
				Phone = currentUser.PhoneNumber!,
				BirthDate = currentUser.BirthDate,
				City = currentUser.City,
				Gender = currentUser.Gender,
			};

			return View(userEditViewModel);
		}
		public async Task LogOut()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
