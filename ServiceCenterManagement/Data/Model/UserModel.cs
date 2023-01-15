using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterManagement.Data.Model
{
    public class UserModel
    {
        //A class that holds the properties of User model
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
       
        public string ContactNo { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }





    }
}
