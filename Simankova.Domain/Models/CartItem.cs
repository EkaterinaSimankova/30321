using Simankova.Domain.Entities;

namespace Simankova.Domain.Models;

public class CartItem
{
    public Product Product { get; set; }

    public int Amount { get; set; }
}

public class Cart
{

    public int Id { get; set; }
    /// <summary>
    /// Список объектов в корзине
    /// key - идентификатор объекта
    /// </summary>
    public Dictionary<int, CartItem> CartItems { get; set; } = new();

    public virtual void AddToCart(Product product)
    {
        if (CartItems.TryGetValue(product.Id, out var item))
        {
            item.Amount++;
        }
        else
        {
            CartItems.Add(product.Id, new CartItem
            {
                Product = product,
                Amount = 1
            });
        };
    }

    public virtual void RemoveItems(int id)
    {
        CartItems.Remove(id);
    }
    /// <summary>
    /// Очистить корзину
    /// </summary>
    public virtual void ClearAll()
    {
        CartItems.Clear();
    }


    public int TotalPrice => CartItems.Sum(x => x.Value.Product.Price * x.Value.Amount);

    public int Count => CartItems.Sum(x => x.Value.Amount);
}