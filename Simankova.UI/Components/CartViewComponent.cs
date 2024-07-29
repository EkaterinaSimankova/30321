using Microsoft.AspNetCore.Mvc;
using Simankova.Domain.Models;
using Simankova.UI.Extensions;
namespace Simankova.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }
    }
}
