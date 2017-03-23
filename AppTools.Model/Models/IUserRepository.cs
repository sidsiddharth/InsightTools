using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTools.Model
{
    public interface IUserRepository
    {
        void Add(User user);
        IEnumerable<User> GetAll();
        User Find(string userName);
        void Remove(string userName);
        void Update(User user);
    }
}
