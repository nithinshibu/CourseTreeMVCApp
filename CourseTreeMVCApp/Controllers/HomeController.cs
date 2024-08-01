using CourseTreeMVCApp.Data;
using CourseTreeMVCApp.Data.DbContext;
using CourseTreeMVCApp.Models;
using CourseTreeMVCApp.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourseTreeMVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //Only the categories that have Category Items with associated content with which logged on user has been matched will be passed
            ////into the relevant index view and subsequently displayed to the logged on user.
            ///

            IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels = null;
            IEnumerable<GroupedCategoryItemsByCategoryModel> groupedCategoryItemsByCategoryModels = null;

            CategoryDetailsModel categoryDetailsModel = new CategoryDetailsModel();

            //Checking whether the User has logged on to the system
            //Here we are capturing the currently logged in user's Id , this Id value will be the primary key value for the logged in user
            //which is stored in the AspNetUsers identity table
            if (_signInManager.IsSignedIn(User))
            {
                //we can capture the user Id by using the user manager object

                var user = await _userManager.GetUserAsync(User);

                if(user != null)
                {
                    categoryItemDetailsModels = await GetCategoryItemDetailsForUser(user.Id);
                    //We want the data to be grouped by Category Id so that the data can be shown in the Index view in accordance with our requirement.
                    //We want to display the user the data as a collapsable list

                    groupedCategoryItemsByCategoryModels = GetGroupedCategoryItemsByCategory(categoryItemDetailsModels);

                    categoryDetailsModel.GroupedCategoryItemsByCategoryModels = groupedCategoryItemsByCategoryModels;

                }



            }


            return View(categoryDetailsModel);
        }

        private IEnumerable<GroupedCategoryItemsByCategoryModel> GetGroupedCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels)
        {

            return (from item in categoryItemDetailsModels
                    group item by item.CategoryId into g
                    select new GroupedCategoryItemsByCategoryModel
                    {
                        Id=g.Key,
                        Title= g.Select(c=>c.CategoryTitle).FirstOrDefault(),
                        Items = g
                    });

        }

        //A category may contain category items that do not yet have associated content.
        //We want to bring back those category items which have a associated content using LINQ
        //Category Item entity has a one-to-one relationship with content entity
        //Which means a category item either has one item of content or doesnot have an item of content

        private async Task<IEnumerable<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId)
        {
            /*
                Here we want to join the CategoryItem table to the Category Table and the Content table because we only want to return
                category and category item data if the category item has related content. Then we have to join the user category table so that 
                we only retreive the data relevant to the logged on user.
                We only want the Category data returned that has been matched to the relevant user within the Category table in DB.
                We are also joining the MediaType Table because we want to get the Media Type Image Path so that we can display an image to each category item
                and MediaType indicates the Type of Content associated with the relevant category Item.
            */


            return await (from catItem in _context.CategoryItem
                          join category in _context.Category
                          on catItem.CategoryId equals category.Id
                          join content in _context.Content
                          on catItem.Id equals content.CategoryItem.Id
                          join userCat in _context.UserCategory
                          on category.Id equals userCat.CategoryId
                          join mediaType in _context.MediaType
                          on catItem.MediaTypeId equals mediaType.Id
                          where userCat.UserId == userId
                          select new CategoryItemDetailsModel
                          {
                              CategoryId = category.Id,
                              CategoryTitle=category.Title,
                              CategoryItemId= catItem.Id,
                              CategoryItemTitle=catItem.Title,
                              CategoryItemDescription=catItem.Description,
                              MediaImagePath=mediaType.ThumbnailImagePath
                          }).ToListAsync();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
