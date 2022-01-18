using kairosApp.Domain.Services;
using kairosApp.Extensions;
using kairosApp.Models.Support;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace kairosApp.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        string path = "LDAP://192.168.253.3";
        DirectorySearcher directorySearcher;
        public string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry(path, "carfesal","EngyCfsm1011");
            try {
                directorySearcher = new DirectorySearcher(de);
            }
            catch(DirectoryServicesCOMException exception) { 
                return exception.Message.ToString();
            }
            

            return "LDAPS://"; //+ de.Properties["defaultNamingContext"][0].ToString();
        }

        public void GetAdditionalUserInfo()
        {
            SearchResultCollection results;
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.253.3", "csiusrpw", "T3st*12$");
            var contador = 0;
            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");

            // Email Address
            ds.PropertiesToLoad.Add("mail");

            // First Name
            ds.PropertiesToLoad.Add("givenname");

            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");

            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");

            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");

            ds.PropertiesToLoad.Add("physicalDeliveryOfficeName");

            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                if (sr.Properties["physicalDeliveryOfficeName"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad physicalDeliveryOfficeName: " + sr.Properties["physicalDeliveryOfficeName"][0].ToString());

                if (sr.Properties["name"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad name: " + sr.Properties["name"][0].ToString());

                // If not filled in, then you will get an error
                if (sr.Properties["mail"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad mail: " + sr.Properties["mail"][0].ToString());

                if (sr.Properties["givenname"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad givenname: " + sr.Properties["givenname"][0].ToString());

                if (sr.Properties["sn"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad surname: " + sr.Properties["sn"][0].ToString());

                if (sr.Properties["userPrincipalName"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad userPrincipalName: " + sr.Properties["userPrincipalName"][0].ToString());

                if (sr.Properties["distinguishedName"].Count > 0)
                    Debug.WriteLine("Imprimiendo propiedad distinguishedName: " + sr.Properties["distinguishedName"][0].ToString());
                contador++;
            }

            Debug.WriteLine("Numero de Usuarios: "+contador);
        }

        /**
         *
         */
        public void GetAUser(string userName)
        {
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.253.3", "csiusrpw", "T3st*12$");
            SearchResult sr;

            // Build User Searcher
            ds = BuildUserSearcher(de);
            // Set the filter to look for a specific user
            ds.Filter = "(&(objectCategory=User)(objectClass=person)(name=" + userName + "))";

            sr = ds.FindOne();

            if (sr != null)
            {
                Debug.WriteLine(sr.GetPropertyValue("name"));
                Debug.WriteLine(sr.GetPropertyValue("mail"));
                Debug.WriteLine(sr.GetPropertyValue("givenname"));
                Debug.WriteLine(sr.GetPropertyValue("sn"));
                Debug.WriteLine(sr.GetPropertyValue("userPrincipalName"));
                Debug.WriteLine(sr.GetPropertyValue("distinguishedName"));
            }
        }

        public DirectorySearcher BuildUserSearcher(DirectoryEntry de)
        {
            DirectorySearcher ds = null;

            ds = new DirectorySearcher(de);

            // Full Name
            ds.PropertiesToLoad.Add("name");

            // Email Address
            ds.PropertiesToLoad.Add("mail");

            // First Name
            ds.PropertiesToLoad.Add("givenname");

            // Last Name (Surname)
            ds.PropertiesToLoad.Add("sn");

            // Login Name
            ds.PropertiesToLoad.Add("userPrincipalName");

            // Distinguished Name
            ds.PropertiesToLoad.Add("distinguishedName");

            return ds;
        }

        public string Login(string userName, string password)
        {
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://192.168.253.3", userName, password))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    //Buscamos por la propiedad SamAccountName
                    searcher.Filter = "(samaccountname=" + userName + ")";
                    //Buscamos el usuario con la cuenta indicada
                    SearchResult result = null;
                    try
                    {
                        result = searcher.FindOne();
                    }
                    catch (DirectoryServicesCOMException e)
                    {
                        Debug.WriteLine(e.Message);
                        return null;
                    }

                    if (result != null)
                    {
                        string un = "";
                        //Comporbamos las propiedades del usuario
                        Debug.WriteLine(result.ToString);
                        ResultPropertyCollection fields = result.Properties;
                        foreach (String ldapField in fields.PropertyNames)
                        {
                            Debug.WriteLine("Propiedad Active Directory: " + ldapField);
                            foreach (Object myCollection in fields[ldapField])
                            {
                                Debug.WriteLine("Valor de la propiedad:" + myCollection.ToString());
                                if (ldapField == "userPrincipalName")
                                    un = myCollection.ToString().ToLower();
                            }
                        }
                        return result.ToString();

                    }
                    else
                        return null;
                }
            }
        }

        

        public bool ChangePassword(string userName, string password, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool ResetPassword(string userName, string password)
        {
            /*PrincipalContext context = new PrincipalContext(ContextType.Domain, "espol.edu.ec", "DC=espol,DC=edu,DC=ec", "buscador", "T3st*12$");
            UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
            if(user == null)
            {
                return false;
            }
            //Enable Account if it is disabled
            user.Enabled = true;
            //Reset User Password
            user.SetPassword(password);
            //Force user to change password at next logon
            //user.ExpirePasswordNow();
            user.Save();
            return true;*/

            DirectoryEntry domainEntry = new DirectoryEntry("LDAP://192.168.253.3:636/CN=sugfimcp,OU=Users,OU=FIMCP,DC=espol,DC=edu,DC=e", "csiusrpw", "T3st*12$");
            DirectorySearcher dirSearcher = new DirectorySearcher(domainEntry);
            domainEntry.Invoke("SetPassword", new object[] { "carlitos" });
            domainEntry.Properties["LockOutTime"].Value = 0; //unlock account
            domainEntry.CommitChanges();
            domainEntry.Close();
            return true;
            /*string filter = string.Format("(SAMAccountName={0})", userName);
            dirSearcher.Filter = filter;
            SearchResult result = dirSearcher.FindOne();
            if (result != null)
            {
                DirectoryEntry userEntry = result.GetDirectoryEntry();

                //Enable Account if it is disabled
                userEntry.Properties["userAccountControl"].Value = 0x200;
                //Reset User Password
                userEntry.Invoke("SetPassword", new object[] { password });
                //Force user to change password at next logon
                userEntry.Properties["pwdlastset"][0] = 0;
                userEntry.CommitChanges();
                userEntry.Close();
                return true;
            }
            else
            {
                return false;
            }*/
        }

        public bool CreateUser(ADCreateUser user)
        {
            /*// Creating the PrincipalContext
            PrincipalContext principalContext = null;
            try
            {
                principalContext = new PrincipalContext(ContextType.Domain, "192.168.253.3", "OU=Users,DC=espol,DC=edu,DC=ec", "buscador", "T3st*12$");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to create PrincipalContext. Exception: " + e);
                return false;
            }

            // Check if user object already exists in the store
            UserPrincipal usr = UserPrincipal.FindByIdentity(principalContext, user.Username);
            if (usr != null)
            {
                Debug.WriteLine(user.Username + " already exists. Please use a different User Logon Name.");
                return false;
            }

            // Create the new UserPrincipal object
            UserPrincipal userPrincipal = new UserPrincipal(principalContext);*/
            /*
            if (user.Persona.Apellidos != null && user.Persona.Apellidos.Length > 0)
                userPrincipal.Surname = user.Persona.Apellidos;

            if (user.Persona.Nombres != null && user.Persona.Nombres.Length > 0)
                userPrincipal.GivenName = user.Persona.Nombres;

            if (user.Persona.Identificacion != null && user.Persona.Identificacion.Length > 0)
                userPrincipal.EmployeeId = user.Persona.Identificacion;

            if (user.Username != null && user.Username.Length > 0)
                userPrincipal.EmailAddress = user.Username+"@espol.edu.ec";

            if (user.Persona.Telefono != null && user.Persona.Telefono.Length > 0)
                userPrincipal.VoiceTelephoneNumber = user.Persona.Telefono;
            
            if (user.Username != null && user.Username.Length > 0)
                userPrincipal.SamAccountName = user.Username;
            
            var pwdOfNewlyCreatedUser = "abcde@@12345!~";
            userPrincipal.SetPassword(pwdOfNewlyCreatedUser);

            userPrincipal.Enabled = true;
            //userPrincipal.ExpirePasswordNow();

            try
            {
                //userPrincipal.Save();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception creating user object. " + e);
                return false;
            }*/
            DirectoryEntry ouEntry = new DirectoryEntry("espol.edu.ec"/*, "buscador", "T3st*12$"*/);
            //ouEntry.Path = "LDAP://OU=Users,DC=espol,DC=edu,DC=ec";
            //ouEntry.AuthenticationType = AuthenticationTypes.Secure;
            try
            {
                DirectoryEntry childEntry = ouEntry.Children.Add("CN="+user.Username, "user");
                childEntry.CommitChanges();
                ouEntry.CommitChanges();
                childEntry.Invoke("SetPassword", "Holiwis");
                childEntry.Invoke("Put", new object[] { "userAccountControl", "512" });
                childEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }

}
