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
        /* In your Content class, you have a navigation property CategoryItem of type CategoryItem. Since this is a one-to-one relationship, Entity Framework Core will look for a foreign key property based on naming conventions. By default, EF Core will look for a property named [NavigationPropertyName]Id to use as the foreign key.
           In your case, the navigation property is CategoryItem, so EF Core looks for a property named CategoryItemId as the foreign key. That's why you see the CategoryItemId column created in your Content table when you migrate the database. 
        */
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
