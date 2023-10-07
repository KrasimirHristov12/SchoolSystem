namespace SchoolSystem.Web.ViewModels.Classes
{
    using SchoolSystem.Data.Models;
    using SchoolSystem.Services.Mapping;

    public class ClassViewModel : IMapFrom<SchoolClass>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
