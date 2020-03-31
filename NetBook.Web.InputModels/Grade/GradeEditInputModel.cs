namespace NetBook.Web.InputModels.Grade
{
    using System.ComponentModel.DataAnnotations;
    using NetBook.Common;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class GradeEditInputModel : IMapTo<GradeServiceModel>, IMapFrom<GradeServiceModel>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.GradeValueRegex, ErrorMessage = GlobalConstants.GradeValueRegexError)]
        public string GradeValue { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string StudentId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public Term Term { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public bool IsTermGrade { get; set; }
    }
}
