using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using SHA256EncryptCore;

namespace entities.Domain
{
    internal class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public static string Password = AESEncrypter.EncryptString(Password, "sgwrg");
        public User() { }

    }

}
