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
        void Add(string userName, User user);
        IEnumerable<User> GetAll(string searchString);
        User Find(string userName);
        void Remove(string userName);
        void Update(string userName, User user);
    }
}
