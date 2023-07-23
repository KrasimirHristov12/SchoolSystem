namespace SchoolSystem.Web.ViewComponents
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Web.ViewModels.Questions;

    public class QuestionsFormViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<TakeQuestionsViewModel> model)
        {
            return this.View(model);
        }
    }
}
