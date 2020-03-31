namespace NetBook.Web.InputModels.Class
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class SubjectInClassEditInputModel : IMapTo<ClassSubjectServiceModel>, IMapFrom<ClassSubjectServiceModel>
    {
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string ClassId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.WorkloadRegex, ErrorMessage = GlobalConstants.WorkloadRegexError)]
        public int Workload { get; set; }
    }
}
