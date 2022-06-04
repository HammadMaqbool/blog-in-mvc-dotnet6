using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogTutorial.ViewModel
{
    public class PostVM
    {
        [Required(ErrorMessage = "Please Enter Title")]
        public string Title { get; set; }


        [Required(ErrorMessage ="Please provide subtitle")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$", ErrorMessage = "Please provide Valid email")]
        public string SubTitle { get; set; }






        public string Content { get; set; }
        public string Date { get; set; }

        [DisplayName("Cover image for blog post")]
        public IFormFile Image { get; set; }
        public string Slug { get; set; }
    }
}
