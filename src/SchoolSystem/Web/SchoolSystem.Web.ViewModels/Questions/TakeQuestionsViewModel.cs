﻿namespace SchoolSystem.Web.ViewModels.Questions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using SchoolSystem.Common;
    using SchoolSystem.Data.Models.Enums;
    using SchoolSystem.Web.ViewModels.Answers;

    [Bind(Exclude = $"{nameof(Title)},{nameof(Type)},{nameof(Points)},{nameof(FirstAnswerContent)},{nameof(SecondAnswerContent)},${nameof(ThirdAnswerContent)},{nameof(FourthAnswerContent)}")]
    public class TakeQuestionsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public QuestionType Type { get; set; }

        public string FirstAnswerContent { get; set; }

        public bool IsFirstAnswerChecked { get; set; }

        public string SecondAnswerContent { get; set; }

        public bool IsSecondAnswerChecked { get; set; }

        public string ThirdAnswerContent { get; set; }

        public bool IsThirdAnswerChecked { get; set; }

        public string FourthAnswerContent { get; set; }

        public bool IsFourthAnswerChecked { get; set; }

        public int Points { get; set; }
    }
}
