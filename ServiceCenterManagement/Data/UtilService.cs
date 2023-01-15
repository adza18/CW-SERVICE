using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterManagement.Data
{
    //A class that contains utility methods that is shared among different classes
    public class UtilService
    {
        private const string _separator = ":";

        //generating hash for password
        public static string GenerateHash(string passwordInput)
        {
            var saltSize = 16;
            var iterations = 100000;
            var keySize = 32;
            HashAlgorithmName passwordAlgorithm = HashAlgorithmName.SHA256;
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(passwordInput, salt, iterations, passwordAlgorithm, keySize);

            return string.Join(_separator, Convert.ToHexString(hash), Convert.ToHexString(salt), iterations, passwordAlgorithm);

        }

        //Verifying hash to match password
        public static bool VerifyHash(string inputPassword, string hashString)
        {
            var segments = hashString.Split(_separator);
            var hash = Convert.FromHexString(segments[0]);
            var salt = Convert.FromHexString(segments[1]);
            var iterations = int.Parse(segments[2]);
            var algorithm = new HashAlgorithmName(segments[3]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(
                inputPassword,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        //Getting app directory path
        public static string getAppDirectoryPath()
        {
            return @"D:\college\application development\service\ServiceCenterManagement\ServiceCenterManagement\wwwroot\FileData";
        }
        public static string getAppUsersFilePath()
        {
            return Path.Combine(getAppDirectoryPath(), "users.json");
        }
        public static string getRecordFilePath()
        {
            return Path.Combine(getAppDirectoryPath(), "records.json");
        }
        public static string getItemFilePath()
        {
            return Path.Combine(getAppDirectoryPath(), "items.json");
        }
    }
}
