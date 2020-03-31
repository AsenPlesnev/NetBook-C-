namespace NetBook.Web.ViewModels.Class
{
    using AutoMapper;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class ClassAllViewModel : IMapFrom<ClassServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string SchoolYear { get; set; }

        public string ClassTeacherFullName { get; set; }

        public string ClassName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ClassServiceModel, ClassAllViewModel>()
                .ForMember(
                    destination => destination.SchoolYear,
                    opts => opts.MapFrom(origin =>
                        $"{origin.SchoolYearStart.ToString()}/{origin.SchoolYearEnd.ToString()}"))
                .ForMember(
                    destination => destination.ClassName,
                    opts => opts.MapFrom(origin => $"{origin.ClassNumber} {origin.ClassLetter}"));
        }
    }
}
