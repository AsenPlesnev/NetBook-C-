namespace NetBook.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    using NetBook.Common;

    public class IndexInputModel
    {
        [Required(ErrorMessage = GlobalConstants.PinMissingErrorMessage)]
        [RegularExpression(GlobalConstants.PinRegex, ErrorMessage = GlobalConstants.PinRegexErrorMessage)]
        [MinLength(GlobalConstants.PhoneLength, ErrorMessage = GlobalConstants.PhoneRegexError)]
        [StringLength(GlobalConstants.PhoneLength)]
        public string PIN { get; set; }
    }
}
