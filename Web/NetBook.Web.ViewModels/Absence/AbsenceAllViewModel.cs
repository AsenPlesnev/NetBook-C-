namespace NetBook.Web.ViewModels.Absence
{
    using AutoMapper;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class AbsenceAllViewModel : IMapFrom<AbsenceServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string SubjectName { get; set; }

        public string StudentFullName { get; set; }

        public string ClassName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AbsenceServiceModel, AbsenceAllViewModel>()
                .ForMember(
                    destination => destination.SubjectName,
                    opts => opts.MapFrom(origin =>
                        origin.Subject.Subject.Name))
                .ForMember(
                    destination => destination.ClassName,
                    opts => opts.MapFrom(origin =>
                        $"{origin.Student.Class.ClassNumber} {origin.Student.Class.ClassLetter}"))
                .ForMember(
                    destination => destination.StudentFullName,
                    opts => opts.MapFrom(origin =>
                        origin.Student.FullName));
        }
    }
}
