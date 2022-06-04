using System.ComponentModel.DataAnnotations;

namespace BlogTutorial.ViewModel
{
    public class LoginVM
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
