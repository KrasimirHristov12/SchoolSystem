namespace SchoolSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SchoolSystem.Data.Common.Models;

    public class GradingScale : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        public Guid QuizId { get; set; }

        public int MinimumPointForPoor { get; set; }

        public int MaximumPointsForPoor { get; set; }

        public int MinimumPointsForFair { get; set; }

        public int MaximumPointsForFair { get; set; }

        public int MinimumPointsForGood { get; set; }

        public int MaximumPointsForGood { get; set; }

        public int MinimumPointsForVeryGood { get; set; }

        public int MaximumPointsForVeryGood { get; set; }

        public int MinimumPointsForExcellent { get; set; }

        public int MaximumPointsForExcellent { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
