namespace NetBook.Web.InputModels.Grade
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using NetBook.Common;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class GradeCreateInputModel : IMapTo<GradeServiceModel>, IHaveCustomMappings
    {
        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        [RegularExpression(GlobalConstants.GradeValueRegex, ErrorMessage = GlobalConstants.GradeValueRegexError)]
        public string GradeValue { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string SubjectId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string StudentId { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public Term Term { get; set; }

        [Required(ErrorMessage = GlobalConstants.RequiredFieldError)]
        public string IsTermGrade { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<GradeCreateInputModel, GradeServiceModel>()
                .ForMember(
                    destination => destination.IsTermGrade,
                    opts => opts.MapFrom(origin =>
                        origin.IsTermGrade == "1"));
        }
    }
}
