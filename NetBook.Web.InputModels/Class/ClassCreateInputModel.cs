namespace NetBook.Web.InputModels.Class
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class ClassCreateInputModel : IMapTo<ClassServiceModel>
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public int ClassNumber { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public ClassLetter ClassLetter { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.SchoolYearRegex, ErrorMessage = GlobalConstants.SchoolYearRegexError)]
        public int SchoolYearStart { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.SchoolYearRegex, ErrorMessage = GlobalConstants.SchoolYearRegexError)]
        public int SchoolYearEnd { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string ClassTeacherId { get; set; }
    }
}
