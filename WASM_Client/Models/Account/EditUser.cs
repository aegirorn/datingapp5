using System.ComponentModel.DataAnnotations;

namespace WASM_Client.Models.Account
{
    public class EditUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [MinLength(6, ErrorMessage = "The Password field must be a minimum of 6 characters")]
        public string Password { get; set; }

        public EditUser() { }

        public EditUser(UserModel user)
        {
            Username = user.username;
        }
    }
}