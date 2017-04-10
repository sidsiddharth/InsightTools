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
using Microsoft.Extensions.Options;
using AppTools.Model;

namespace AppTools.Data
{
    public class UserRepository : IUserRepository
    {
        //private readonly UserContext _context;
        public IOptions<AppKeyConfig> appKeys;
        public AppKeyConfig AppConfigs { get; }
        
        public UserRepository(IOptions<AppKeyConfig> appKeys)
        {
            //_context = context;
            //Add(new AppTools.Model.User { FirstName = "Sid", UserName="shazarika", LastName="Hazarika" });
            AppConfigs = appKeys.Value;
        }

        public void Add(string userName, AppTools.Model.User user)
        {
            //_context.Users.Add(user);
            //_context.SaveChanges();
            LDAPAccess ldapAccess = new LDAPAccess(AppConfigs);
            ldapAccess.CreateLDAPUser(userName, user);
        }

        public AppTools.Model.User Find(string userName)
        {
            //return _context.Users.FirstOrDefault(t => t.UserName == userName);
            LDAPAccess ldapAccess = new LDAPAccess(AppConfigs);
            return ldapAccess.GetLDAPUser(userName);
        }

        public IEnumerable<AppTools.Model.User> GetAll(string searchString)
        {
            //return _context.Users.ToList();
            LDAPAccess ldapAccess = new LDAPAccess(AppConfigs);
            return  ldapAccess.GetLDAPUsers(searchString);
        }

        public void Remove(string userName)
        {
            //var entity = _context.Users.First(t => t.UserName == userName);
            //_context.Users.Remove(entity);
            //_context.SaveChanges();
            LDAPAccess ldapAccess = new LDAPAccess(AppConfigs);
            ldapAccess.GetLDAPUser(userName);
        }

        public void Update(string userName, AppTools.Model.User user)
        {
            //_context.Users.Update(user);
            //_context.SaveChanges();
            LDAPAccess ldapAccess = new LDAPAccess(AppConfigs);
            ldapAccess.UpdateLDAPUser(userName, user);
        }
    }
}
