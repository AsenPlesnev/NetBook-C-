namespace NetBook.Web.ViewModels.Grade
{
    using AutoMapper;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class StudentGradesAllViewModel : IMapFrom<ClassSubjectServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string SubjectName { get; set; }

        public int Workload { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ClassSubjectServiceModel, StudentGradesAllViewModel>()
                .ForMember(
                    destination => destination.SubjectName,
                    opts => opts.MapFrom(origin =>
                        origin.Subject.Name))
                .ForMember(
                    destination => destination.Workload,
                    opts => opts.MapFrom(origin => origin.Workload));
        }
    }
}
