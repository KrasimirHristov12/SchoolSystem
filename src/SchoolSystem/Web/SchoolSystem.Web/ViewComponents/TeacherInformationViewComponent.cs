﻿namespace SchoolSystem.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Services.Data.Teachers;
    using SchoolSystem.Web.ViewModels.Teachers;

    public class TeacherInformationViewComponent : ViewComponent
    {
        private readonly ITeacherService teacherService;

        public TeacherInformationViewComponent(ITeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? teacherId)
        {
            var teacherInfo = this.teacherService.GetTeacherInformation<TeacherInformationViewModel>((int)teacherId);
            return this.View(teacherInfo);
        }
    }
}
