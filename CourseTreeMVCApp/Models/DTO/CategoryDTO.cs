using System.ComponentModel.DataAnnotations;

namespace CourseTreeMVCApp.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Thumbnail Image Path")]
        public string ThumbnailImagePath { get; set; }
    }
}
