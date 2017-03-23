using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AppTools.Model
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
            Add(new User { FirstName = "Sid", UserName="shazarika", LastName="Hazarika" });
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User Find(string userName)
        {
            return _context.Users.FirstOrDefault(t => t.UserName == userName);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public void Remove(string userName)
        {
            var entity = _context.Users.First(t => t.UserName == userName);
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
