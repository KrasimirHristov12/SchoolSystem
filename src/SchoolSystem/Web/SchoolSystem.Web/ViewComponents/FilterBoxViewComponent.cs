namespace SchoolSystem.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Web.ViewModels.Grades;

    public class FilterBoxViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(FilterBaseViewModel filterElements, string type)
        {
            this.ViewBag.Type = type;
            return this.View(filterElements);
        }
    }
}
