using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CourseTreeMVCApp.Entities;

namespace CourseTreeMVCApp.Data
{
    public class ApplicationUser:IdentityUser
    {
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string Address1 { get; set; }
        [StringLength(250)]
        public string Address2 { get; set; }
        [StringLength(50)]
        public string PostCode { get; set; }

        //The Id column which is a GUID in aspnetusers table will be stored in UserCategory table as UserId.

        [ForeignKey("UserId")]
        public virtual ICollection<UserCategory> UserCategory { get; set; }
    }
}
