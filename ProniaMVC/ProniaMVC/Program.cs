using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.Abstractions;
using ProniaMVC.Areas.Admin.Controllers;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.Services;

namespace ProniaMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
            });

            builder.Services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(15);
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Lockout.AllowedForNewUsers = true;
                opt.User.RequireUniqueEmail = true;

            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}