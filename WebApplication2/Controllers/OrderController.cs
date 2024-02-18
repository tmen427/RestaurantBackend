using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Formats.Asn1;
using System.Reflection.Metadata.Ecma335;
using WebApplication2.Models;
using WebApplication2.Repository;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {

        private ToDoContext _toDoContext;
        private readonly ILogger<AuthController> _logger;
        IConfiguration _configuration;
        IRepo<CartItems> _cartItemsRepo;
        IRepo<User> _userRepo;  
        //public OrderController(ToDoContext toDoContext, ILogger<AuthController> logger, IConfiguration configuration)
        //{
        //    _toDoContext = toDoContext;
        //    _logger = logger;
        //    _configuration = configuration;
        //}

        public OrderController(ToDoContext toDoContext, IRepo<CartItems> carItemrepo, IRepo<User> userRepo)
        {
            _toDoContext = toDoContext; 
            _cartItemsRepo = carItemrepo;
            _userRepo = userRepo; 
        }

        [HttpPost]
        public async Task<CartItems> PostInformationAsync(CartItems cartItems)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var cartItems1 = await _cartItemsRepo.PostItemsAsync(cartItems);
                    return cartItems1; 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }
            return null;
        }

        [HttpGet("CartItems")]

        public async  Task<List<CartItems>> GetCartItems()
        {
      
            try
            {
                return  await _cartItemsRepo.CartItemsAsync(); 
           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }

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

            try
            {
                await _toDoContext.OrderInformation.AddAsync(orders);
                await _toDoContext.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);

            }
        }

        [HttpPost("BookingInformation")]
        public async Task<string> BookingInformation(BookingInformation bookingInformation)
        {
            try
            {
                await _toDoContext.BookingInformation.AddAsync(bookingInformation);
                await _toDoContext.SaveChangesAsync();
                return "the post was succesful";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);

            }
        }

        [HttpGet("UserOrders")]
        public async Task<List<CartItems>> GetInformationAsync(string NameOnCard)
        {

            var query = (from x in _toDoContext.CartItems where x.OrderInformationNameonCard == NameOnCard select x);
            return await query.ToListAsync();
        }

        [HttpPost("ContactInformation")]
        public async Task<Contact> PostContactAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _toDoContext.Contacts.AddAsync(contact);
                    await _toDoContext.SaveChangesAsync();
                    return contact;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.StackTrace);

                }

            }

            return null;
        }

        [HttpGet("Health")]
        public string Healthy(string p)
        {
            return _cartItemsRepo.ReturnString(p);
        }


    }
}
