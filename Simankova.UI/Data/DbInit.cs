using Microsoft.AspNetCore.Identity;
using Simankova.UI.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Claims;

namespace Simankova.UI.Data
{
    public class DbInit
    {
        public static async Task SetupIdentityAdmin(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var userManager = scope
                .ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();
            var user = await userManager.FindByEmailAsync("admin@gmail.com");
            if (user == null)
            {
                user = new AppUser();
                await userManager.SetEmailAsync(user, "admin@gmail.com");
                await userManager.SetUserNameAsync(user, user.Email);
                user.EmailConfirmed = true;
                user.Avatar = GetImage();
                user.MimeType = "image/png";
                await userManager.CreateAsync(user, "123456");
                var claim = new Claim(ClaimTypes.Role, "admin");
                await userManager.AddClaimAsync(user, claim);
            }
        }

        private static byte[] GetImage()
        {
            using Bitmap bitmap = new Bitmap(2, 2);
            // Set the colors of the pixels
            bitmap.SetPixel(0, 0, Color.Red);
            bitmap.SetPixel(1, 0, Color.Green);
            bitmap.SetPixel(0, 1, Color.Blue);
            bitmap.SetPixel(1, 1, Color.Black);

            // Save the bitmap to a memory stream in PNG format
            using MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            byte[] pngBytes = memoryStream.ToArray();

            // Write to file (optional)
            return pngBytes;
            // Output to console for demonstration
        }
    }
}