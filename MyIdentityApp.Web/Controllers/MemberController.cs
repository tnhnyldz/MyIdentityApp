using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdentityApp.Web.Models;

namespace MyIdentityApp.Web.Controllers
{
	[Authorize]
	public class MemberController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ILogger<MemberController> _logger;

		public MemberController(SignInManager<AppUser> signInManager, ILogger<MemberController> logger)
		{
			_signInManager = signInManager;
			_logger = logger;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task LogOut()
		{
			await _signInManager.SignOutAsync();
		}
	}
}
