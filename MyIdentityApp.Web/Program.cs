using Microsoft.EntityFrameworkCore;
using MyIdentityApp.Web.Extensions;
using MyIdentityApp.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//adding context
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

//ýdentity configs
builder.Services.AddIdentityWithExt();



builder.Services.ConfigureApplicationCookie(opt =>
{
	var cookieBuilder = new CookieBuilder();

	cookieBuilder.Name = "UdemyAppCookie";
	opt.LoginPath = new PathString("/Home/SignIn");

	opt.Cookie = cookieBuilder; 
	opt.ExpireTimeSpan=TimeSpan.FromDays(60);

	//at day 61 the cookie will expire
	opt.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");




app.Run();
