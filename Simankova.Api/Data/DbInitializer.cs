using Simankova.Domain.Entities;

namespace Simankova.Api.Data;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Categories.ToList().Count == 0)
        {
            var categories = new List<Category>
            {
                new Category {Id=0, Name="Помады", NormalizedName="lipstick"},
                new Category {Id=0, Name="Пудры", NormalizedName="powder"},
                new Category {Id=0, Name="Тушь для ресниц", NormalizedName="mascara"}
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        if (context.Products.ToList().Count == 0)
        {
            var categories = context.Categories.ToList();

            var products = new List<Product>
            {
                new Product
                {
                    Id = 0, Name = "Помада",
                    Description = "Натуральный цвет, бархатная текстура",
                    Price = 20, Image = "Images/lipstick1.jpg",
                    CategoryId = categories.Find(c => c.NormalizedName.Equals("lipstick")).Id
                },

                new Product
                {
                    Id = 0, Name = "Помада",
                    Description = "Классический красный оттенок, сатиновый финиш",
                    Price = 25, Image = "Images/lipstick2.jpg",
                    CategoryId = categories.Find(c => c.NormalizedName.Equals("lipstick")).Id
                },

                new Product
                {
                    Id = 0, Name = "Помада",
                    Description = "Насыщенный коралловый цвет, увлажнение и питание",
                    Price = 28, Image = "Images/lipstick3.jpg",
                    CategoryId = categories.Find(c => c.NormalizedName.Equals("lipstick")).Id
                },

                new Product
                {
                    Id = 0, Name = "Помада",
                    Description = "Яркая фуксия, комфортная гелевая текстура",
                    Price = 25, Image = "Images/lipstick4.jpg",
                    CategoryId = categories.Find(c => c.NormalizedName.Equals("lipstick")).Id
                },

                new Product
                {
                    Id = 0, Name = "Пудра",
                    Description = "Для бледной кожи с холодным оттенком",
                    Price = 35, Image = "Images/powder1.jpg",
                    CategoryId = categories.Find(c => c.NormalizedName.Equals("powder")).Id
                },

                new Product
                {
                    Id = 0, Name = "Тушь для ресниц",
                    Description = "Черная тушь с эффектом панорамного объема",
                    Price = 25, Image = "Images/mascara1.jpg",
                    CategoryId = categories.Find(c => c.NormalizedName.Equals("mascara")).Id
                },
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        
    }
}