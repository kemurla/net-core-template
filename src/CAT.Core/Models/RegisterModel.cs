using System;

using CAT.Core.Entities;

namespace CAT.Core.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Maps the model to an actual User entity.(Can be replaced with AutoMapper if needed.)
        /// </summary>
        /// <returns></returns>
        public User ToUserEntity()
        {
            return new User
            {
                Id        = Guid.NewGuid().ToString(),
                FirstName = FirstName,
                LastName  = LastName,
                Email     = Email,
                UserName  = Email
            };
        }
    }
}
