using CourseTreeMVCApp.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseTreeMVCApp.Extensions
{
    public static class ConvertExtensions
    {
        public static List<SelectListItem> ConvertToSelectList<T>(this IEnumerable<T> collection, int selectedValue) where T : IPrimaryProperties
        {
            return (from item in collection
                    select new SelectListItem
                    {
                        Text = item.Title,
                        Value = item.Id.ToString(),
                        Selected = (item.Id == selectedValue)
                    }).ToList();
        }
    }
}
