using System.Text.Json.Serialization;

namespace Simankova.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } // id продукта
        public string Name { get; set; } // название продукта
        public string Description { get; set; } // описание продукта
        public int Price { get; set; } // стоимость продукта
        public string? Image { get; set; } // путь к файлу изображения
                                         
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
