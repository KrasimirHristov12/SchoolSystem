namespace SchoolSystem.Web.ViewModels.Questions
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using SchoolSystem.Common;
    using SchoolSystem.Data.Models;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Services.Mapping;
    using SchoolSystem.Web.Infrastructure.ValidationAttributes;
    using SchoolSystem.Web.ViewModels.Answers;

    public class QuestionsInputModel : IMapFrom<Question>, IHaveCustomMappings, IMapObject
    {
        [RequiredWithErrorMessage]
        [Display(Name = GlobalConstants.Question.TitleDisplay)]
        public string Title { get; set; }

        [RequiredWithErrorMessage]
        public QuestionType? QuestionType { get; set; }

        [Display(Name = GlobalConstants.Question.PointsDisplay)]
        [RequiredWithErrorMessage]
        [Range(typeof(int), GlobalConstants.Question.MinimumPointsAsString, GlobalConstants.Question.MaximumPointsAsString, ErrorMessage = GlobalConstants.ErrorMessage.PointsErrorMessage)]
        public int? Points { get; set; }

        [RequiredWithErrorMessage]
        public string FirstAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsFirstAnswerCorrect { get; set; }

        [RequiredWithErrorMessage]
        public string SecondAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsSecondAnswerCorrect { get; set; }

        [RequiredWithErrorMessage]
        public string ThirdAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsThirdAnswerCorrect { get; set; }

        [RequiredWithErrorMessage]
        public string FourthAnswerContent { get; set; }

        [CheckAtLeastOneAttribute]
        public bool IsFourthAnswerCorrect { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Question, QuestionsInputModel>()
                .ReverseMap();
        }
    }
}