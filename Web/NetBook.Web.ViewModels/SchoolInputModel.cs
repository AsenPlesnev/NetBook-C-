using NetBook.Common;

namespace NetBook.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class SchoolInputModel
    {
        [Required(ErrorMessage = GlobalConstants.PhoneRegexError)]
        [RegularExpression(GlobalConstants.SchoolNameRegex)]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = GlobalConstants.PhoneRegexError)]
        [RegularExpression(GlobalConstants.SchoolNameRegex, ErrorMessage = GlobalConstants.SchoolNameRegexError)]
        public string Town { get; set; }

        [Required(ErrorMessage = GlobalConstants.PhoneRegexError)]
        [RegularExpression(GlobalConstants.SchoolNameRegex, ErrorMessage = GlobalConstants.SchoolNameRegexError)]
        public string Municipality { get; set; }

        [Required(ErrorMessage = GlobalConstants.PhoneRegexError)]
        [RegularExpression(GlobalConstants.SchoolNameRegex, ErrorMessage = GlobalConstants.SchoolNameRegexError)]
        public string Region { get; set; }

        [Required(ErrorMessage = GlobalConstants.PhoneRegexError)]
        [RegularExpression(GlobalConstants.SchoolNameRegex, ErrorMessage = GlobalConstants.SchoolNameRegexError)]
        public string Area { get; set; }

    }
}
