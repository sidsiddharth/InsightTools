using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using AppTools.Model;
using System.Runtime;

namespace AppTools.Data
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
