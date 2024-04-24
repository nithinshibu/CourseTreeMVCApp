using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseTreeMVCApp.Entities
{
    public class Content
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [Display(Name = "HTML Content")]
        public string HTMLContent { get; set; }

        [Display(Name = "Video Link")]
        public string VideoLink { get; set; }

        //The content entity is having a one to one relationship with category item entity
        //Here we are establishing the referential integrity between the content entity and the category item entity
        public CategoryItem CategoryItem { get; set; }

        [NotMapped]
        public int CatItemId { get; set; }
        //Note: This property cannot be 
        //named CategoryItemId because this would
        //interfere with future migrations
        //It has been named like this
        //so as not to conflict with EF Core naming conventions

        [NotMapped]
        public int CategoryId { get; set; }
    }
}
