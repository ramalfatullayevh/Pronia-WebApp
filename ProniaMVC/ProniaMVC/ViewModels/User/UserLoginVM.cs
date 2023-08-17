using System.ComponentModel.DataAnnotations;

namespace ProniaMVC.ViewModels
{
    public class UserLoginVM
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
