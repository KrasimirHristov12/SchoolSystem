namespace SchoolSystem.Web.ViewModels.Subjects
{
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;

    public class SubjectViewModel : IMapFrom<Subject>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
