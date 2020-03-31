namespace NetBook.Web.InputModels.Absence
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class AbsenceCreateInputModel : IMapTo<AbsenceServiceModel>
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string StudentId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string ClassId { get; set; }
    }
}
