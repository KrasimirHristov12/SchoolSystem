namespace SchoolSystem.Web.WebModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using SchoolSystem.Common;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;
    using SchoolSystem.Web.ViewModels.Subjects;

    public class SubjectsInputModel
    {
        [Display(Name = GlobalConstants.Subject.SubjectNameDisplay)]
        [RequiredWithErrorMessage]
        [Remote(action: "ValidateSubjectUniquenessToTeacherList", controller: "Subjects")]
        public int? SubjectId { get; set; }

        public IEnumerable<SubjectsViewModel> Subjects { get; set; }
    }
}
