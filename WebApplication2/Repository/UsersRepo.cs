using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Repository
{
    public class UsersRepo : IRepo<User>
    {
        private readonly ToDoContext _context;
        public UsersRepo(ToDoContext context)
        {

            _context = context;

        }


        public async Task<List<User>> CartItemsAsync()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }


        public string ReturnString(string x)
        {
            throw new NotImplementedException();
        }
        public Task<User> PostItemsAsync(User t)
        {
            throw new NotImplementedException();
        }

    }
}
