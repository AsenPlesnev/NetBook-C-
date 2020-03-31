namespace NetBook.Services.Data.ServiceModels
{
    using NetBook.Data.Models;
    using NetBook.Services.Mapping;

    public class SubjectServiceModel : IMapFrom<Subject>, IMapTo<Subject>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
