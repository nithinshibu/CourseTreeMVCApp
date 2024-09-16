using CourseTreeMVCApp.Entities;

namespace CourseTreeMVCApp.Models
{
    public class CategoriesToUserModel
    {
        //Id in aspnetusers table is GUID
        public string UserId { get; set; }

        //A list of courses saved to the system will be stored in the Categories property.

        public ICollection<Category> Categories { get; set; }
        //This view model will store the information for the view that will display a list of checkboxes to the registered user
        //Each checkbox item denotes a course currently saved to the system. 

        //A list of courses currently associated with the logged on user will be stored in CategoriesSelected property.
        public ICollection<Category> CategoriesSelected { get; set; }   
    }
}
