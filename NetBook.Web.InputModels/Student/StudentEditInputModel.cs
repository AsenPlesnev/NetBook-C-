namespace NetBook.Web.InputModels.Student
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class StudentEditInputModel : IMapTo<StudentServiceModel>, IMapFrom<StudentServiceModel>
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string Id { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.FullNameRegex, ErrorMessage = GlobalConstants.FullNameRegexError)]
        [MaxLength(GlobalConstants.FullNameLength)]
        public string FullName { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.PinRegex, ErrorMessage = GlobalConstants.PinRegexError)]
        [MaxLength(GlobalConstants.PinLength)]
        public string PIN { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.CitizenshipRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
        [MaxLength(GlobalConstants.CitizenshipLength)]
        public string Citizenship { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.PinRegex, ErrorMessage = GlobalConstants.PhoneRegexError)]
        [MaxLength(GlobalConstants.PhoneLength)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.CitizenshipRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
        [MaxLength(GlobalConstants.CitizenshipLength)]
        public string Town { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.CitizenshipRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
        [MaxLength(GlobalConstants.CitizenshipLength)]
        public string Municipality { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.CitizenshipRegex, ErrorMessage = GlobalConstants.CitizenshipRegexError)]
        [MaxLength(GlobalConstants.CitizenshipLength)]
        public string Region { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string ClassId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.AddressRegex, ErrorMessage = GlobalConstants.AddressRegexError)]
        [MaxLength(GlobalConstants.AddressLength)]
        public string Address { get; set; }
    }
}
