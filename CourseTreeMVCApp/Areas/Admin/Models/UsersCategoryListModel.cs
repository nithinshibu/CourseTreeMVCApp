namespace CourseTreeMVCApp.Areas.Admin.Models
{
    public class UsersCategoryListModel
    {
        //This denotes the relevant category
        public int CategoryId { get; set; }

        //This will contain a collection of all the registered users saved to the system 
        public ICollection<UserModel> Users { get; set; }

        //This will contain the collection of users that have been saved to the system for a particular category
        public ICollection<UserModel> UsersSelected { get; set; }
    }
}
