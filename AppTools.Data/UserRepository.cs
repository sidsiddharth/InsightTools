using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using AppTools.Model;
//using CommonMethods;
//using System.DirectoryServices;
//using System.DirectoryServices.ActiveDirectory;
//using System.DirectoryServices.Protocols;
using System.Xml;
using System.Xml.Linq;
using Novell.Directory.Ldap;

namespace AppTools.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        //private readonly LDAPAccess _ldapaccess;

        public UserRepository(UserContext context)
        {
            _context = context;
            Add(new AppTools.Model.User { FirstName = "Sid", UserName="shazarika", LastName="Hazarika" });
        }

        public void Add(AppTools.Model.User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public AppTools.Model.User Find(string userName)
        {
            //return _context.Users.FirstOrDefault(t => t.UserName == userName);
            LDAPAccess ldapAccess = new LDAPAccess();
            return ldapAccess.GetLDAPUser(userName);
        }

        public IEnumerable<AppTools.Model.User> GetAll()
        {
            //return _context.Users.ToList();
            LDAPAccess ldapAccess = new LDAPAccess();
            return  ldapAccess.GetLDAPUsers();
        }

        public void Remove(string userName)
        {
            var entity = _context.Users.First(t => t.UserName == userName);
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(AppTools.Model.User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
