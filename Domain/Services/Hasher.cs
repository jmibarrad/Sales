using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class Hasher: ICrypter
    {
 
            public string Encrypt(string password)
            {
                var hashlite = new SHA256Managed();
                byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(password, "URFMODE"));
                byte[] hashedBytes = hashlite.ComputeHash(textWithSaltBytes);
                hashlite.Clear();
                return Convert.ToBase64String(hashedBytes);
            }
        
    }
}
