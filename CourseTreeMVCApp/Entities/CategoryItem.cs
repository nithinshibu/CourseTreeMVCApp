using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseTreeMVCApp.Entities
{
    public class CategoryItem
    {
        private DateTime _releaseDate = DateTime.MinValue;

        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        public string Description { get; set; }

        //this represents the foreign key field from the category entity 
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please select a valid item from the '{0}' dropdown list")]
        [Display(Name = "Media Type")]
        public int MediaTypeId { get; set; }

        // To make sure this doesn't affect the migration we can add the Not Mapped attribute , hence this will be ignored
        [NotMapped]
        public virtual ICollection<SelectListItem> MediaTypes { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Release Date")]
        public DateTime DateTimeItemReleased
        {
            get
            {
                return (_releaseDate == DateTime.MinValue) ? DateTime.Now : _releaseDate;
            }
            set
            {
                _releaseDate = value;
            }

        }

        [NotMapped]
        public int ContentId { get; set; }
    }
}
