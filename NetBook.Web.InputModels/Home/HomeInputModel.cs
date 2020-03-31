namespace NetBook.Web.InputModels.Home
{
    using System.ComponentModel.DataAnnotations;
    using NetBook.Common;

    public class HomeInputModel
    {
        [Required(ErrorMessage = GlobalConstants.PinMissingErrorMessage)]
        [RegularExpression(GlobalConstants.PinRegex, ErrorMessage = GlobalConstants.PinRegexErrorMessage)]
        [MaxLength(GlobalConstants.PinLength)]
        public string PIN { get; set; }
    }
}
