using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Formats.Asn1;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private ToDoContext _toDoContext;
        public OrderController(ToDoContext toDoContext)
        {
            _toDoContext = toDoContext;    
        }

        [HttpPost]
        public async Task<CartItems> PostInformationAsync(CartItems cartItems)
        {
     
            await _toDoContext.CartItems.AddAsync(cartItems);
            await _toDoContext.SaveChangesAsync();
            return cartItems;
        }

        [HttpPost("PaymentInformation")]
        public async Task<OrderInformationDTO> PostOrderInformation(OrderInformationDTO order)
        {
            OrderInformation orders = new OrderInformation();
            orders.Credit = order.Credit;
            orders.NameonCard = order.NameonCard;
            orders.CreditCardNumber = order.CreditCardNumber;
            orders.Expiration = order.Expiration;
            orders.CVV = order.CVV;
            orders.UserName = order.UserName;

            await _toDoContext.OrderInformation.AddAsync(orders); 
            await _toDoContext.SaveChangesAsync();
            return order;
        }

        [HttpGet("UserOrders")]
        public async Task<List<CartItems>> GetInformationAsync(string NameOnCard)
        {
            //orderInformationId is the id represents  person in order...
            //query the cartItems, but no the OrderInformation....
            var query = (from x in _toDoContext.CartItems where x.OrderInformationNameonCard == NameOnCard select x); 
           return await query.ToListAsync();
        }

        [HttpPost("ContactInformation")]
        public async Task<Contact> PostContactAsync(Contact contact)
        {


            await _toDoContext.Contacts.AddAsync(contact);
            await _toDoContext.SaveChangesAsync();
            return contact;
        }

        [HttpGet("Health")]
        public string Healthy()
        {
            return "just a health check"; 
        }

    
    }
}
