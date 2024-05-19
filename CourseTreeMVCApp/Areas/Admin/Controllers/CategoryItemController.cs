using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseTreeMVCApp.Data.DbContext;
using CourseTreeMVCApp.Entities;
using CourseTreeMVCApp.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace CourseTreeMVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CategoryItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CategoryItem
        public async Task<IActionResult> Index(int categoryId)
        {
            /*
             Here, first we are joining the category item entity with the content entity.
            The content entity contains a foreign key from the category item entity which will be used for joining the entities.
            We can then create a group for the content items related to each category item.
            Then we can select from the group (contentcatgroup) whcih we have created into an object (subContent)
            And also we are calling the DefaultIfEmpty() method on each content group 
            Because if a category item doesnot contain content then the subContent object will be set to NULL
            So this provides us with an easy way to tell if a category item contains content or doesnot contain content.   
            

            We are using a ternary operator to assign the content id property in the returned category item object to Zero if the content id 
            for that particular category item has not been added to the system yet.

            If the content for that particular category item is present then we will assign the content for that category item
            */
            List<CategoryItem> list = await (from catItem in _context.CategoryItem
                                             join contentItem in _context.Content on catItem.Id equals contentItem.CategoryItem.Id
                                             into contentcatgroup
                                             from subContent in contentcatgroup.DefaultIfEmpty()
                                             where catItem.CategoryId == categoryId
                                             select new CategoryItem
                                             {
                                                 Id = catItem.Id,
                                                 Title = catItem.Title,
                                                 Description = catItem.Description,
                                                 DateTimeItemReleased = catItem.DateTimeItemReleased,
                                                 MediaTypeId = catItem.MediaTypeId,
                                                 CategoryId = categoryId,
                                                 ContentId = (subContent != null) ? subContent.Id : 0
                                             }).ToListAsync();


            ViewBag.CategoryId = categoryId;


            return _context.CategoryItem != null ?
                        View(list) :
                        Problem("Entity set 'ApplicationDbContext.CategoryItem'  is null.");
        }

        // GET: Admin/CategoryItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryItem == null)
            {
                return NotFound();
            }

            var categoryItem = await _context.CategoryItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = categoryItem.CategoryId;

            return View(categoryItem);
        }

        // GET: Admin/CategoryItem/Create
        public async Task<IActionResult> Create(int categoryId)
        {
            List<MediaType> mediaTypes = await _context.MediaType.ToListAsync();
            CategoryItem categoryItem = new CategoryItem()
            {
                CategoryId = categoryId,
                MediaTypes = mediaTypes.ConvertToSelectList(0)
            };
            return View(categoryItem);
        }

        // POST: Admin/CategoryItem/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CategoryId,MediaTypeId,DateTimeItemReleased")] CategoryItem categoryItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { categoryId = categoryItem.CategoryId });
            }
            List<MediaType> mediaTypes = await _context.MediaType.ToListAsync();
            categoryItem.MediaTypes = mediaTypes.ConvertToSelectList(categoryItem.MediaTypeId);
            return View(categoryItem);
        }

        // GET: Admin/CategoryItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryItem == null)
            {
                return NotFound();
            }

            List<MediaType> mediaTypes = await _context.MediaType.ToListAsync();



            var categoryItem = await _context.CategoryItem.FindAsync(id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            categoryItem.MediaTypes = mediaTypes.ConvertToSelectList(categoryItem.MediaTypeId);

            ViewBag.CategoryId = categoryItem.CategoryId;

            return View(categoryItem);
        }

        // POST: Admin/CategoryItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CategoryId,MediaTypeId,DateTimeItemReleased")] CategoryItem categoryItem)
        {
            if (id != categoryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryItemExists(categoryItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { categoryId = categoryItem.CategoryId });
            }
            return View(categoryItem);
        }

        // GET: Admin/CategoryItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryItem == null)
            {
                return NotFound();
            }

            var categoryItem = await _context.CategoryItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryItem.CategoryId;
            return View(categoryItem);
        }

        // POST: Admin/CategoryItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CategoryItem'  is null.");
            }
            var categoryItem = await _context.CategoryItem.FindAsync(id);
            if (categoryItem != null)
            {
                _context.CategoryItem.Remove(categoryItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId = categoryItem?.CategoryId });
        }

        private bool CategoryItemExists(int id)
        {
            return (_context.CategoryItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
