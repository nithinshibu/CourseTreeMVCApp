namespace CourseTreeMVCApp.Models
{
    public class GroupedCategoryItemsByCategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        //The IGrouping<int, CategoryItemDetailsModel> property represents a collection of CategoryItemDetailsModel objects grouped by an integer key. The IGrouping<TKey, TElement> interface is part of LINQ and allows you to access both the key and the group of items associated with that key. In this case, the key is an int, and the grouped items are instances of CategoryItemDetailsModel (this is our View Model). This type is typically used in scenarios where you want to organize and process data that has been grouped by a specific property.

        public IGrouping<int, CategoryItemDetailsModel> Items { get; set; }
    }
}
