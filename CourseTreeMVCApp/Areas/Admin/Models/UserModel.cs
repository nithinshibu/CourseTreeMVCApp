namespace CourseTreeMVCApp.Areas.Admin.Models
{
    //Here we are creating a view model based on AspNetUsers entity
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
