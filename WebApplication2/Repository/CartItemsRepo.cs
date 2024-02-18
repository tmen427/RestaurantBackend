using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class CartItemsRepo : IRepo<CartItems>
    {
        private ToDoContext _context;
        public CartItemsRepo(ToDoContext context)
        {
            _context = context;
        }

        public string ReturnString(string x)
        {
            if (x == "cookie") { throw new NotImplementedException(); }
            return "bro I am from the class class" + x;
        }

        public async Task<List<CartItems>> CartItemsAsync()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            return cartItems; 
        }

        public async Task<CartItems> PostItemsAsync(CartItems cartItems)
        {
            var carItems = await _context.CartItems.AddAsync(cartItems);
            await _context.SaveChangesAsync();
            return cartItems; 
        }

    }
}
