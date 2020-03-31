namespace NetBook.Web.ViewModels.Student
{
    using AutoMapper;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class StudentAllViewModel : IMapFrom<StudentServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string PIN { get; set; }

        public string ClassName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<StudentServiceModel, StudentAllViewModel>().ForMember(
                destination => destination.ClassName,
                opts => opts.MapFrom(origin => $"{origin.Class.ClassNumber} {origin.Class.ClassLetter}"));
        }
    }
}