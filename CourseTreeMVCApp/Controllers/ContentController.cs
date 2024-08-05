using CourseTreeMVCApp.Data.DbContext;
using CourseTreeMVCApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseTreeMVCApp.Controllers
{
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * Only items that have content are displayed in the collapsable list in the home index.
         * The content entity has a one to one relationship with the category item entity .
         * So a category item will have either an item of content or will not have any related content. 
         * If the category item doesnot contain any content this category item will not be made available in the collapsable list in Home/index view.
         */


        public async Task<IActionResult> Index(int categoryItemId)
        {
            Content? content = await (from item in _context.Content
                                     where item.CategoryItem.Id == categoryItemId
                                     select new Content
                                     {
                                         Title = item.Title,
                                         VideoLink = item.VideoLink,
                                         HTMLContent = item.HTMLContent
                                     }).FirstOrDefaultAsync();


            return View(content);
        }
    }
}
