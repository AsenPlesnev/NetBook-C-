namespace NetBook.Web.InputModels.Subject
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class SubjectCreateInputModel : IMapTo<SubjectServiceModel>, IMapFrom<SubjectServiceModel>
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.SubjectNameRegex, ErrorMessage = GlobalConstants.SubjectNameRegexError)]
        [MaxLength(GlobalConstants.SubjectNameLength)]
        public string Name { get; set; }
    }
}
