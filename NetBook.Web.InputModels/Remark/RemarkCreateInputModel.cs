namespace NetBook.Web.InputModels.Remark
{
    using System.ComponentModel.DataAnnotations;
    using NetBook.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class RemarkCreateInputModel : IMapTo<RemarkServiceModel>
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.RemarkTextRegex, ErrorMessage = GlobalConstants.RemarkTextRegexError)]
        [MaxLength(GlobalConstants.RemarkTextLength)]
        public string Text { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string StudentId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string SubjectId { get; set; }
    }
}
