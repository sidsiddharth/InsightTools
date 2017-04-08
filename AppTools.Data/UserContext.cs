using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppTools.Model;
//using System.DirectoryServices;
//using System.DirectoryServices.Protocols;
//using CommonMethods;
using System.Net;
using Novell.Directory.Ldap;
using System.Collections;

namespace AppTools.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<AppTools.Model.User> Users { get; set; }
    }
}
