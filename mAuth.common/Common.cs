using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;
namespace mAuth.Common
{
    public class CommonLogics
    {

        public enum MembershipPasswordFormat
        {
            //
            // Summary:
            //     Passwords are not encrypted.
            Clear = 0,
            //
            // Summary:
            //     Passwords are encrypted one-way using the SHA1 hashing algorithm.
            Hashed = 1,
            //
            // Summary:
            //     Passwords are encrypted using the encryption settings determined by the machineKey
            //     Element (ASP.NET Settings Schema) element configuration.
            Encrypted = 2
        }

        public virtual string EncodePassword(string password, string passwordSalt, MembershipPasswordFormat passwordFormat)
        {
            string salt = string.Empty;
            if (string.IsNullOrEmpty(passwordSalt))
            {
                salt = CreateSalt(Convert.ToInt32(5));
            }
            //System.Security.Cryptography.RNGCryptoServiceProvider.Create().ToString();

            if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                return password;

            byte[] bIn = Encoding.Unicode.GetBytes(password);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

            return Convert.ToBase64String(bRet);
        }

        public virtual string CreateSalt(int size)
        {
            //Generate a cryptographic random number using the cryptographic service provider
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] buffer = new byte[size];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public virtual MembershipPasswordFormat ConvertIntToMembershipPasswordFormat(System.Int32 passwordFormat)
        {
            MembershipPasswordFormat format = MembershipPasswordFormat.Hashed;
            switch (passwordFormat)
            {
                case 1:
                    format = MembershipPasswordFormat.Hashed;
                    break;
                case 2:
                    format = MembershipPasswordFormat.Encrypted;
                    break;
                case 0:
                    format = MembershipPasswordFormat.Clear;
                    break;
            }
            return format;
        }


    }
}
