using CourseTreeMVCApp.Data.DbContext;
using CourseTreeMVCApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseTreeMVCApp.Data
{
    //This class is mainly created because of DRY principles (Don't Repeat Yourself)

    //We are going to use the built-in dependency injection system to inject an object derived from the data functions class within any class
    //that needs to reuse the data related functionality that will reside within the data functions class.

    //We can make the functionality within our data functions class available to other classes by using the dependency injection system built into .net core

    public class DataFunctions : IDataFunctions
    {
        private readonly ApplicationDbContext _context;

        public DataFunctions(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, List<UserCategory> userCategoryItemsToAdd)
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    _context.RemoveRange(userCategoryItemsToDelete);
                    await _context.SaveChangesAsync();
                    if (userCategoryItemsToAdd != null)
                    {
                        _context.AddRange(userCategoryItemsToAdd);
                        await _context.SaveChangesAsync();
                    }
                    //This line finalizes the transaction. It means "apply all the scheduled changes to the database." If this line is reached without errors, the deletions and additions are saved permanently.
                    await dbContextTransaction.CommitAsync();

                }
                catch (Exception)
                {
                    //If any error occurs during the changes, the catch block is executed. Instead of committing the transaction, it disposes of it. This effectively cancels all the changes made during the transaction, ensuring the database remains unchanged in case of an error.
                    await dbContextTransaction.DisposeAsync();
                }

            }
        }
    }
}
