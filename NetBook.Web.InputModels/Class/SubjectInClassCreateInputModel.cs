namespace NetBook.Web.InputModels.Class
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class SubjectInClassCreateInputModel : IMapTo<ClassSubjectServiceModel>
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.WorkloadRegex, ErrorMessage = GlobalConstants.WorkloadRegexError)]
        public int WorkLoad { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string ClassId { get; set; }
    }
}
