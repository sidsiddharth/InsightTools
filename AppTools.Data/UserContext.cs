using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppTools.Model;
//using System.DirectoryServices;
//using System.DirectoryServices.Protocols;
using CommonMethods;
using System.Net;
using Novell.Directory.Ldap;

namespace AppTools.Data
{
    public class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<AppTools.Model.User> Users { get; set; }
    }

    public class LDAPAccess
    {
        private static string[] propertiesToQuery = { "uid", "givenName", "sn", "email", "partnerreference", "telephoneNumber", "hasprovidedinformation","cancreatecase","company","cn","pwdAccountLockedTime", "pwdChangedTime", "pwdPolicySubentry", "pwdReset" };
        public LdapConnection OpenLDAPAdminConnection()
        {
            //LdapDirectoryIdentifier ldapDirIdentifier = null;
            //NetworkCredential networkCreds = null;
            LdapConnection ldapConn = null;
            //string methodName = "OpenLDAPAdminConnection";
            try
            {
                //ldapDirIdentifier = new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(CommonMethods.AppConfigSettings.AccessAppConfigDict("LDAPServer"), 389);
                //networkCreds = new NetworkCredential(Common.Base64Decode(AppConfigSettings.AccessAppConfigDict("LDAPUser")).ToString(), Common.Base64Decode(AppConfigSettings.AccessAppConfigDict("LDAPPass")).ToString());
                //ldap = new Novell.Directory.Ldap.LdapConnection(ldapDirIdentifier, networkCreds);
                //ldap.SessionOptions.SecureSocketLayer = false;
                //ldap.SessionOptions.ProtocolVersion = 3;
                //ldap.AuthType = AuthType.Basic;
                //ldap.Bind();
                //ldapDirIdentifier = null;
                //networkCreds = null;
                //return ldap;
                ldapConn = new LdapConnection();
                ldapConn.Connect("ml-auth01-dev.caseshare.com", 389);
                ldapConn.Bind("cn=User Manager,dc=catalystapps,dc=com", "R4arPpfx_f");
                return ldapConn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                

            }
        }

        public AppTools.Model.User GetLDAPUser(string userName)
        {
            string baseDN = "ou=groups,dc=catalystapps,";
            baseDN += AppConfigSettings.ServerEnvironment == "production-japan" ? "dc=co,dc=jp" : "dc=com";
            string filter = "(&(objectClass=crsUser)(uid=" + userName + "))";
            AppTools.Model.User user = null;
            LdapConnection ldapConn = null;
            LDAPAccess _ldapaccess = null;
            LdapEntry nextEntry = null;
            LdapSearchResults lsc = null;
            try
            {
                _ldapaccess = new LDAPAccess();
                using (ldapConn = OpenLDAPAdminConnection())
                {
                    lsc = ldapConn.Search(baseDN, LdapConnection.SCOPE_SUB, filter, propertiesToQuery, false);
                    while (lsc.hasMore())
                    {
                        try
                        {
                            nextEntry = lsc.next();
                            user = MapLDAPEntryToUser(nextEntry);
                        }
                        catch (LdapException ex)
                        {
                            throw ex;
                        }
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AppTools.Model.User MapLDAPEntryToUser(LdapEntry entry)
        {
            AppTools.Model.User user = null;
            try
            {
                user = new AppTools.Model.User();
                user.UserName = entry.getAttribute("uid").StringValue;
                user.FirstName = (entry.getAttribute("givenName") == null) ? "" : entry.getAttribute("givenName").StringValue;
                user.LastName = (entry.getAttribute("sn") == null) ? "" : entry.getAttribute("sn").StringValue;
                user.Email = (entry.getAttribute("email") == null) ? "" : entry.getAttribute("email").StringValue;
                user.Partner = (entry.getAttribute("partnerreference") == null) ? "" : entry.getAttribute("partnerreference").StringValue;
                user.Telephone = (entry.getAttribute("telephoneNumber") == null) ? "" : entry.getAttribute("telephoneNumber").StringValue;
                user.HasProvidedInfo = (entry.getAttribute("hasprovidedinformation") == null) ? false : Convert.ToBoolean(entry.getAttribute("hasprovidedinformation").StringValue);
                user.CanCreateCase = (entry.getAttribute("cancreatecase") == null) ? false : Convert.ToBoolean(entry.getAttribute("cancreatecase").StringValue);
                user.Company = (entry.getAttribute("company") == null) ? "" : entry.getAttribute("company").StringValue;
                user.CN = (entry.getAttribute("cn") == null) ? "" : entry.getAttribute("cn").StringValue;
                user.pwdAccountLockedTime = (entry.getAttribute("pwdAccountLockedTime") == null) ? DateTime.MinValue : DateTime.ParseExact(entry.getAttribute("pwdAccountLockedTime").StringValue.Replace("Z", ""), "yyyyMMddHHmmss", null);
                user.pwdChangedTime = (entry.getAttribute("pwdChangedTime") == null) ? DateTime.MinValue : DateTime.ParseExact(entry.getAttribute("pwdChangedTime").StringValue.Replace("Z", ""), "yyyyMMddHHmmss", null);
                user.pwdPolicySubentry = (entry.getAttribute("pwdPolicySubentry") == null) ? "" : entry.getAttribute("pwdPolicySubentry").StringValue;
                user.pwdReset = (entry.getAttribute("pwdReset") == null) ? false : Convert.ToBoolean(entry.getAttribute("pwdReset").StringValue);
                return user;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

}
