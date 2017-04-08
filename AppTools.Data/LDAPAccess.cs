using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppTools.Model;
using System.Net;
using Novell.Directory.Ldap;
using System.Collections;

namespace AppTools.Data
{
    public class LDAPAccess
    {
        private static string[] propertiesToQuery = { "uid", "givenName", "sn", "email", "partnerreference", "telephoneNumber", "hasprovidedinformation", "cancreatecase", "company", "cn", "pwdAccountLockedTime", "pwdChangedTime", "pwdPolicySubentry", "pwdReset" };
        private static string dc = environment == "production-japan" ? "dc=co,dc=jp" : "dc=com";
        private static string baseDN = "ou=groups,dc=catalystapps," + dc;
        private const string environment = "development";
        public AppKeyConfig AppConfigs { get; }

        public LDAPAccess(AppKeyConfig appConfigsValues)
        {
            AppConfigs = appConfigsValues;
        }

        public LdapConnection OpenLDAPAdminConnection()
        {
            LdapConnection ldapConn = null;
            //string methodName = "OpenLDAPAdminConnection";
            try
            {
                ldapConn = new LdapConnection();
                ldapConn.Connect(AppConfigs.LDAPServer, AppConfigs.LDAPServerPort);
                ldapConn.Bind(AppConfigs.LDAPUser, AppConfigs.LDAPUserPass);
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

        public List<AppTools.Model.User> GetLDAPUsers(string searchString)
        {
            string filter = "(&(objectClass=crsUser)";
            AppTools.Model.User user = null;
            LdapConnection ldapConn = null;
            LdapEntry nextEntry = null;
            LdapSearchResults lsc = null;
            List<AppTools.Model.User> lstUser = null;
            try
            {
                using (ldapConn = OpenLDAPAdminConnection())
                {
                    if (searchString != null && searchString.Trim() != "")
                        filter += "(uid=*" + searchString + "*))";
                    else
                        filter += ")";
                    lsc = ldapConn.Search(baseDN, LdapConnection.SCOPE_SUB, filter, propertiesToQuery, false);
                    lstUser = new List<Model.User>();
                    while (lsc.hasMore())
                    {
                        try
                        {
                            nextEntry = lsc.next();
                            user = MapLDAPEntryToUser(nextEntry);
                            lstUser.Add(user);
                        }
                        catch (LdapException ex)
                        {
                            throw ex;
                        }
                    }
                }
                return lstUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppTools.Model.User GetLDAPUser(string userName)
        {
            string baseDN = "ou=groups,dc=catalystapps,";
            baseDN += environment == "production-japan" ? "dc=co,dc=jp" : "dc=com";
            string filter = "(&(objectClass=crsUser)(uid=" + userName + "))";
            AppTools.Model.User user = null;
            LdapConnection ldapConn = null;
            LdapEntry nextEntry = null;
            LdapSearchResults lsc = null;
            try
            {
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

        public void CreateLDAPUser(string userName, User user)
        {
            string filter = "(&(objectClass=crsUser)(uid=" + userName + "))";
            LdapConnection ldapConn = null;
            LdapEntry entry = null;
            string userDN = "";
            string policy = "";
            try
            {
                using (ldapConn = OpenLDAPAdminConnection())
                {
                    // DN of the entry to be added
                    userDN = "cn=" + userName + ",ou=" + user.Partner + ",ou=groups,dc=catalystapps,";
                    userDN += environment == "production-japan" ? "dc=co,dc=jp" : "dc=com";
                    policy = "cn=" + user.Partner.ToLower() + ",ou=Policies,dc=catalystapps,";
                    policy += (environment == "production-japan") ? "dc=co,dc=jp" : "dc=com";
                    user.pwdPolicySubentry = policy;
                    user.HasProvidedInfo = true;
                    user.CaseUserTwoFA = false;
                    user.UserUserTwoFA = false;
                    user.IsAutomationUser = false;
                    entry = MapUserToLDAPEntry(userDN, user);
                    user.pwdReset = false;
                    ldapConn.Add(entry);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLDAPUser(string userName, User user)
        {
            string filter = "(&(objectClass=crsUser)(uid=" + userName + "))";
            LdapConnection ldapConn = null;
            ArrayList modList = null;
            LdapAttribute attribute = null;
            string userDN = "";
            try
            {
                using (ldapConn = OpenLDAPAdminConnection())
                {
                    //Get user DN
                    userDN = GetUserDN(userName);
                    if (userDN != "")
                    {
                        modList = new ArrayList();
                        attribute = new LdapAttribute("givenName", user.FirstName);
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        attribute = new LdapAttribute("sn", user.LastName);
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        attribute = new LdapAttribute("email", user.Email);
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        attribute = new LdapAttribute("partnerreference", user.Partner);
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        attribute = new LdapAttribute("telephoneNumber", user.Telephone);
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        attribute = new LdapAttribute("cancreatecase", user.CanCreateCase.ToString().ToUpper());
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        attribute = new LdapAttribute("company", user.Company);
                        modList.Add(new LdapModification(LdapModification.REPLACE, attribute));

                        LdapModification[] mods = new LdapModification[modList.Count];
                        Type mtype = Type.GetType("Novell.Directory.LdapModification");
                        mods = (LdapModification[])modList.ToArray(typeof(LdapModification));

                        //Modify the entry in the directory
                        ldapConn.Modify(userDN, mods);
                    }
                    else
                        throw new Exception("user doesn't exist");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetUserDN(string userName)
        {
            string filter = "(&(objectClass=crsUser)(uid=" + userName + "))";
            string DN = "";
            LdapConnection ldapConn = null;
            LdapEntry nextEntry = null;
            LdapSearchResults lsc = null;
            try
            {
                using (ldapConn = OpenLDAPAdminConnection())
                {
                    lsc = ldapConn.Search(baseDN, LdapConnection.SCOPE_SUB, filter, propertiesToQuery, false);
                    while (lsc.hasMore())
                    {
                        try
                        {
                            nextEntry = lsc.next();
                            DN = nextEntry.DN;
                        }
                        catch (LdapException ex)
                        {
                            throw ex;
                        }
                    }
                }
                return DN;
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
                //user.Password = entry.getAttribute("userPassword").StringValue;
                user.FirstName = (entry.getAttribute("givenName") == null) ? "" : entry.getAttribute("givenName").StringValue;
                user.LastName = (entry.getAttribute("sn") == null) ? "" : entry.getAttribute("sn").StringValue;
                user.Email = (entry.getAttribute("email") == null) ? "" : entry.getAttribute("email").StringValue;
                user.Partner = (entry.getAttribute("partnerreference") == null) ? "" : entry.getAttribute("partnerreference").StringValue;
                user.Telephone = (entry.getAttribute("telephoneNumber") == null) ? "" : entry.getAttribute("telephoneNumber").StringValue;
                user.HasProvidedInfo = (entry.getAttribute("hasprovidedinformation") == null) ? false : Convert.ToBoolean(entry.getAttribute("hasprovidedinformation").StringValue);
                user.CanCreateCase = (entry.getAttribute("cancreatecase") == null) ? false : Convert.ToBoolean(entry.getAttribute("cancreatecase").StringValue);
                user.Company = (entry.getAttribute("company") == null) ? "" : entry.getAttribute("company").StringValue;
                user.UserType = (entry.getAttribute("usertype") == null) ? 1 : Convert.ToInt16(entry.getAttribute("usertype").StringValue);
                user.CN = (entry.getAttribute("cn") == null) ? "" : entry.getAttribute("cn").StringValue;
                user.IsAutomationUser = (entry.getAttribute("partnerreference") == null || entry.getAttribute("partnerreference").ToString() == "Catalyst Automation Users" || entry.getAttribute("partnerreference").ToString() == "automation") ? true : false;
                user.CaseUserTwoFA = (entry.getAttribute("caseusertwofa") == null) ? false : Convert.ToBoolean(entry.getAttribute("caseusertwofa").StringValue);
                user.UserUserTwoFA = (entry.getAttribute("userusertwofa") == null) ? false : Convert.ToBoolean(entry.getAttribute("userusertwofa").StringValue);
                user.ObjectClass = (entry.getAttribute("objectClass") == null) ? "" : entry.getAttribute("objectClass").StringValue;
                user.Policy = (entry.getAttribute("pwdPolicySubentry") == null) ? "" : entry.getAttribute("pwdPolicySubentry").StringValue;
                user.pwdAccountLockedTime = (entry.getAttribute("pwdAccountLockedTime") == null) ? DateTime.MinValue : DateTime.ParseExact(entry.getAttribute("pwdAccountLockedTime").StringValue.Replace("Z", ""), "yyyyMMddHHmmss", null);
                user.pwdChangedTime = (entry.getAttribute("pwdChangedTime") == null) ? DateTime.MinValue : DateTime.ParseExact(entry.getAttribute("pwdChangedTime").StringValue.Replace("Z", ""), "yyyyMMddHHmmss", null);
                user.pwdPolicySubentry = (entry.getAttribute("pwdPolicySubentry") == null) ? "" : entry.getAttribute("pwdPolicySubentry").StringValue;
                user.pwdReset = (entry.getAttribute("pwdReset") == null) ? false : Convert.ToBoolean(entry.getAttribute("pwdReset").StringValue);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LdapEntry MapUserToLDAPEntry(string userDN, User user)
        {
            LdapEntry entry = null;
            LdapAttributeSet attributeSet = null;
            try
            {
                attributeSet = new LdapAttributeSet();
                attributeSet.Add(new LdapAttribute("objectClass", "crsUser"));
                attributeSet.Add(new LdapAttribute("cn", user.UserName));
                attributeSet.Add(new LdapAttribute("uid", user.UserName));
                attributeSet.Add(new LdapAttribute("userPassword", user.Password));
                attributeSet.Add(new LdapAttribute("givenName", user.FirstName));
                attributeSet.Add(new LdapAttribute("sn", user.LastName));
                attributeSet.Add(new LdapAttribute("email", user.Email));
                attributeSet.Add(new LdapAttribute("partnerreference", user.Partner));
                attributeSet.Add(new LdapAttribute("telephoneNumber", user.Telephone));
                attributeSet.Add(new LdapAttribute("hasprovidedinformation", user.HasProvidedInfo.ToString().ToUpper()));
                attributeSet.Add(new LdapAttribute("cancreatecase", user.CanCreateCase.ToString().ToUpper()));
                attributeSet.Add(new LdapAttribute("company", user.Company));
                attributeSet.Add(new LdapAttribute("usertype", user.UserType.ToString()));
                attributeSet.Add(new LdapAttribute("caseusertwofa", user.CaseUserTwoFA.ToString().ToUpper()));
                attributeSet.Add(new LdapAttribute("userusertwofa", user.UserUserTwoFA.ToString().ToUpper()));
                attributeSet.Add(new LdapAttribute("pwdPolicySubentry", (user.IsAutomationUser == true) ? "" : user.pwdPolicySubentry));
                attributeSet.Add(new LdapAttribute("pwdReset", (user.pwdReset == false) ? "FALSE" : user.pwdReset.ToString().ToUpper()));
                entry = new LdapEntry(userDN, attributeSet);
                return entry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
