using CourseTreeMVCApp.Data;
using CourseTreeMVCApp.Data.DbContext;
using CourseTreeMVCApp.Entities;
using CourseTreeMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseTreeMVCApp.Controllers
{
    [Authorize]
    public class CategoriesToUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataFunctions _dataFunctions;

        public CategoriesToUserController(ApplicationDbContext context,UserManager<ApplicationUser> userManager,IDataFunctions dataFunctions)
        {
            _context = context;
            _userManager = userManager;
            _dataFunctions = dataFunctions;
        }
        public async Task<IActionResult> Index()
        {
            CategoriesToUserModel categoriesToUserModel = new CategoriesToUserModel();
            var userId = _userManager.GetUserAsync(User).Result?.Id;

            categoriesToUserModel.Categories = await GetCategoriesThatHaveContent();
            categoriesToUserModel.CategoriesSelected = await GetCategoriesCurrentlySavedForUser(userId);

            categoriesToUserModel.UserId = userId;  

            return View(categoriesToUserModel);
        }

        //Creating the index post action method
        //The parameter for this action method will contain a list of category ids 
        //these ids denote the checked item displayed on the relevant index razor view
        //pertaining to courses that are associated with the relevant user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] categoriesSelected)
        {
            var userId = _userManager.GetUserAsync(User).Result?.Id;

            List<UserCategory> userCategoriesToDelete = await GetCategoriesToDeleteForUser(userId);

            //now to project the relevant user id and categories passed in as argument to this method into a list of user category items

            List<UserCategory> userCategoriesToAdd = GetCategoriesToAddForUser(categoriesSelected, userId);

            //now we need to write the ef core code to delete the relevant rows from the user category database table and then add the relevant rows to the user category database table

            await _dataFunctions.UpdateUserCategoryEntityAsync(userCategoriesToDelete, userCategoriesToAdd);

            //Once the user category table has been appropriately updated , we want the user redirected to the Home/Index view

            return RedirectToAction("Index","Home");


        }

        //For retreiving and transforming relevant data 

        //In order to make sure only those categories saved to the system where one of the relevant
        //categories related category items have associated  content are returned from our linq query
        //we will have to join the data retrieved from the category entity with data retreived from the 
        //category item entity and then we are joining the Content entity so that
        //the list returned from this private method will contain a list of categories where each of the relevant categories
        //has atleast one category item that contains content

        private async Task<List<Category>> GetCategoriesThatHaveContent()
        {
            //If a category in the list as more than one category item that contains content,
            //the relevant category data will be repeated i.e for each related category item
            // and that's why we are using Distinct

            var categoriesThatHaveContent = await (from category in _context.Category
                                                   join categoryItem in _context.CategoryItem
                                                   on category.Id equals categoryItem.CategoryId
                                                   join content in _context.Content
                                                   on categoryItem.Id equals content.CategoryItem.Id
                                                   select new Category
                                                   {
                                                       Id = category.Id,
                                                       Title = category.Title,
                                                       Description = category.Description,
                                                   }).Distinct().ToListAsync();

            return categoriesThatHaveContent;   
        }

        //Method for retreiving a list of categories that are associated with the relevant logged on user from the database 

        private async Task<List<Category>> GetCategoriesCurrentlySavedForUser(string userId)
        {

            var categoriesCurrentlySavedForUser = await (from userCategory in _context.UserCategory
                                                         where userCategory.UserId == userId
                                                         select new Category
                                                         {
                                                             Id=userCategory.CategoryId,
                                                         }).ToListAsync();

            return categoriesCurrentlySavedForUser;

        }

        //Method returns a list of user category items that needs to be deleted from the user category table

        private async Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId)
        {
            //linq query to return all categories currently associated with the relevant user
            //and these items will be deleted from the user category table
            var categoriesToDelete = await (from userCat in _context.UserCategory
                                            where userCat.UserId == userId
                                            select new UserCategory
                                            {
                                                Id=userCat.Id,
                                                CategoryId=userCat.CategoryId,
                                                UserId=userId
                                            }).ToListAsync();
            return categoriesToDelete;  
        }

        //Method responsible for transforming data that will be passed into the post index action method which will be an array of CategoryIds into a list of user category items.

        private List<UserCategory> GetCategoriesToAddForUser(string[] categoriesSelected,string userId)
        {
            //We are performing this projection operation so that the data is in a form where we can later use
            //the efcore AddRangeAsync() method to add the relevant rows to the user category database table.

            //So ef core's RemoveRangeAsync() method will be used to remove the relevant rows from the user category table and
            //ef core's AddRangeAsync() method will be used to add the relevant rows to the user category database table.

            var categoriesToAdd = (from categoryId in categoriesSelected
                                   select new UserCategory
                                   {
                                       UserId=userId,
                                       CategoryId= int.Parse(categoryId)
                                   }).ToList();

            return categoriesToAdd;

        }

    }
}
