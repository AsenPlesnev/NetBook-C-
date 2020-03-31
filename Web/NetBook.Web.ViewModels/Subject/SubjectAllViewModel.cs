namespace NetBook.Web.ViewModels.Subject
{
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class SubjectAllViewModel : IMapFrom<SubjectServiceModel>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
