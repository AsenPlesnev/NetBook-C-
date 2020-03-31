namespace NetBook.Web.ViewModels.Remark
{
    using AutoMapper;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class RemarkAllViewModel : IMapFrom<RemarkServiceModel>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string StudentFullName { get; set; }

        public string SubjectName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RemarkServiceModel, RemarkAllViewModel>()
                .ForMember(
                    destination => destination.StudentFullName,
                    opts => opts.MapFrom(origin =>
                        origin.Student.FullName))
                .ForMember(
                    destination => destination.SubjectName,
                    opts => opts.MapFrom(origin =>
                        origin.Subject.Subject.Name));
        }
    }
}
