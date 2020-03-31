namespace NetBook.Web.ViewModels.Grade
{
    using AutoMapper;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class GradeAllViewModel : IMapFrom<GradeServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string GradeValue { get; set; }

        public string SubjectName { get; set; }

        public string StudentName { get; set; }

        public Term Term { get; set; }

        public bool IsTermGrade { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<GradeServiceModel, GradeAllViewModel>()
                .ForMember(
                    destination => destination.SubjectName,
                    opts => opts.MapFrom(origin => origin.Subject.Subject.Name))
                .ForMember(
                    destination => destination.StudentName,
                    opts => opts.MapFrom(origin => origin.Student.FullName));
        }
    }
}
