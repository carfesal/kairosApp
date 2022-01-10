using kairosApp.Domain.Services;
using kairosApp.Extensions;
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
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());

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

            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            foreach (SearchResult sr in results)
            {
                if (sr.Properties["name"].Count > 0)
                    Debug.WriteLine(sr.Properties["name"][0].ToString());

                // If not filled in, then you will get an error
                if (sr.Properties["mail"].Count > 0)
                    Debug.WriteLine(sr.Properties["mail"][0].ToString());

                if (sr.Properties["givenname"].Count > 0)
                    Debug.WriteLine(sr.Properties["givenname"][0].ToString());

                if (sr.Properties["sn"].Count > 0)
                    Debug.WriteLine(sr.Properties["sn"][0].ToString());

                if (sr.Properties["userPrincipalName"].Count > 0)
                    Debug.WriteLine(sr.Properties["userPrincipalName"][0].ToString());

                if (sr.Properties["distinguishedName"].Count > 0)
                    Debug.WriteLine(sr.Properties["distinguishedName"][0].ToString());
            }
        }

        /**
         *
         */
        public void GetAUser(string userName)
        {
            DirectorySearcher ds = null;
            DirectoryEntry de = new DirectoryEntry(GetCurrentDomainPath());
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

        public string login(string userName, string password)
        {
            using (DirectoryEntry entry = new DirectoryEntry(path, userName, password))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry))
                {
                    //Buscamos por la propiedad SamAccountName
                    searcher.Filter = "(samaccountname=" + userName + ")";
                    //Buscamos el usuario con la cuenta indicada
                    var result = searcher.FindOne();
                    if (result != null)
                    {
                        string role = "";
                        //Comporbamos las propiedades del usuario
                        ResultPropertyCollection fields = result.Properties;
                        foreach (String ldapField in fields.PropertyNames)
                        {
                            foreach (Object myCollection in fields[ldapField])
                            {
                                if (ldapField == "employeetype")
                                    role = myCollection.ToString().ToLower();
                            }
                        }
                        /*
                        //Añadimos los claims Usuario y Rol para tenerlos disponibles en la Cookie
                        //Podríamos obtenerlos de una base de datos.
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, credentials.Username),
                            new Claim(ClaimTypes.Role, role)
                        };

                        //Creamos el principal
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        //Generamos la cookie. SignInAsync es un método de extensión del contexto.
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                        */
                        //Redirigimos a la Home
                        return result.ToString();

                    }
                    else
                        return null;
                }
            }
        }

        public string Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool ResetPassword(string userName, string password)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, path);
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
            return true;
        }
    }

}
