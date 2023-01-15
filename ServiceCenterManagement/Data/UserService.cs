using ServiceCenterManagement.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ServiceCenterManagement.Data
{

    //A service that contains methods for the addition,deletion,updation, retrieval of users to and from user json file, and for merge sort
    public class UserService {

		public const string SeedUserName = "admin";
		public const string SeedFullName = "adminadmin";
		public const string SeedContact = "98378392010";
		public const string SeedEmail = "admin@admin.com";
		public const string SeedPassword = "admin";

        public static string userFilePath = UtilService.getAppUsersFilePath();
        public static string directoryPath = UtilService.getAppDirectoryPath();


        public static List<UserModel> Register(Guid userId, string fullName, string userName, string email, string contact, string password, UserRole role, Guid createdBy)
		{
			var users = GetUsers();
			var existingUser = users.Any(x => x.UserName.ToLower() == userName.ToLower());
            var existingEmail = users.Any(x => x.Email.ToLower() == email.ToLower());
            var adminCount = users.Where(x => x.Role == UserRole.Admin).Count();

            string phoneRegex = "^[0-9]+$";
            Match m = Regex.Match(contact,phoneRegex);

            if(String.IsNullOrEmpty(fullName) || String.IsNullOrEmpty(userName))
            {
                throw new Exception($"Username and full name cannot be empty");

            }

            if (!m.Success) {
                throw new Exception($"Contact must be diigts from 0-9");

            }

            if (contact.Length !=10)
            {
                throw new Exception($"Contact must be 10 digits");
            }
            if (existingUser)
			{
				throw new Exception($"Username : {userName} already exists.");
			}
            if (existingEmail)
            {
                throw new Exception($"Email: {email} already exists");
            }
			if (role == UserRole.Admin && adminCount >= 2)
			{
              throw new Exception($"This system already contains 2 admins.");
			}
            if(password.Length < 5)
            {
                throw new Exception($"Password must be greater than 5 letters");

            }

            var user = new UserModel()
			{
				UserName = userName,
				FullName = fullName,
				ContactNo = contact,
				Email = email,
				Role = role,
				Password = UtilService.GenerateHash(password),
                CreatedBy = createdBy
               
			};
			users.Add(user);
			SaveAll(users);

            return users;

		}


        //Creating a seed user 
		public static void SeedUsers()
		{
			  var users = GetUsers().FirstOrDefault(x => x.UserName == SeedUserName);
               if (users == null)
               {
				Register(Guid.Empty, SeedFullName, SeedUserName, SeedEmail, SeedContact, SeedPassword, UserRole.Admin, Guid.Empty);

			}

		}
		public static List<UserModel> GetUsers()

        {

            if (!File.Exists(userFilePath))
            {
                return new List<UserModel>();
            }

            var jsonData = File.ReadAllText(userFilePath);

            if (jsonData.Length == 0)
            {
                return new List<UserModel>();

            }
            else
            {
                return JsonSerializer.Deserialize<List<UserModel>>(jsonData);
            }

        }

        public static UserModel GetUserById(Guid id)
        {
            var users = GetUsers();
            return users.FirstOrDefault(x => x.Id == id);
        }

        //Method for paassword change
        public static UserModel ChangePassword(Guid id, string currentPassword, string newPassword)
        {
            if (currentPassword == newPassword)
            {
                throw new Exception("New password must be different from current password.");
            }

            List<UserModel> users = GetUsers();
            UserModel user = users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            bool passwordIsValid = UtilService.VerifyHash(currentPassword, user.Password);

            if (!passwordIsValid)
            {
                throw new Exception("Incorrect current password.");
            }

            user.Password = UtilService.GenerateHash(newPassword);
            SaveAll(users);

            return user;
        }

        public static void SaveAll(List<UserModel> users)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var jsonData = JsonSerializer.Serialize(users);
            File.WriteAllText(userFilePath, jsonData);
        }

        public static List<UserModel> Delete(Guid id)
        {
            var users = GetUsers();
            var user = users.FirstOrDefault(x => x.Id == id);
            if(user == null)
            {
                throw new Exception("User doesnt exists");
            }
            users.Remove(user);
            SaveAll(users);
            return users;
        }
 
        //method for login
        public static UserModel Login(string username, string password)
        {
            var loginErrorMessage = "Invalid username or password.";

            var users = GetUsers();

            var user = users.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
               throw new Exception(loginErrorMessage);
               
            }

            bool isValidPassword = UtilService.VerifyHash(password, user.Password);

            if (!isValidPassword)
            {
                throw new Exception(loginErrorMessage);
            }

            return user;
        }
    }
}
