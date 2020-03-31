namespace NetBook.Web.ViewModels.Class
{
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class SubjectInClassAllViewModel : IMapFrom<ClassSubjectServiceModel>
    {
        public string Id { get; set; }

        public string ClassId { get; set; }

        public string SubjectName { get; set; }

        public int Workload { get; set; }
    }
}
