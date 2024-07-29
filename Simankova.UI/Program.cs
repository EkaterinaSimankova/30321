using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Simankova.UI.Data;
using Simankova.UI.Models;
using System.Security.Claims;
using Serilog;
using Simankova.Api.Services;
using Simankova.UI.Services;
using ICategoryService = Simankova.UI.Interfaces.ICategoryService;
using IProductService = Simankova.UI.Interfaces.IProductService;
using Microsoft.AspNetCore.Builder;

namespace Simankova.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval:
                    RollingInterval.Day)
                .CreateLogger();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p =>
                p.RequireClaim(ClaimTypes.Role, "admin"));
            });
            builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

            builder.Services.AddHttpClient<IProductService, ApiProductService>(opt
                => opt.BaseAddress = new Uri("https://localhost:7002/api/products/"));
            builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt
                => opt.BaseAddress = new
                    Uri("https://localhost:7002/api/categories/"));
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p =>
                    p.RequireClaim(ClaimTypes.Role, "admin"));
            });

            builder.Services.AddRazorPages();

            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();

            await DbInit.SetupIdentityAdmin(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMiddleware<FileLoggerMiddleware>();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
