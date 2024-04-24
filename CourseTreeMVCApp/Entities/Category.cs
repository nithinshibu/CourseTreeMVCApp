using CourseTreeMVCApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTreeMVCApp.Entities
{
    public class Category : IPrimaryProperties
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Thumbnail Image Path")]
        public string ThumbnailImagePath { get; set; }

        //to enforce referential integrity between the category entity and category item entity
        //In category item table there will be a foreign key called CategoryId.
        //The category item class contains a property named category id so by passing the text category id to the 
        //foreign key attribute we are declaratively establishing the relationship between the category entity and category item entity. 

        [ForeignKey("CategoryId")]
        public virtual ICollection<CategoryItem> CategoryItems { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ICollection<UserCategory> UserCategory { get; set; }
    }
}
