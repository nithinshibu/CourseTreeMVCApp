namespace CourseTreeMVCApp.Interfaces
{
    //The main use of this interface in the Generic method written inside the Convert Extensions class
    public interface IPrimaryProperties
    {
        int Id { get; set; }
        string Title { get; set; }
    }
}
