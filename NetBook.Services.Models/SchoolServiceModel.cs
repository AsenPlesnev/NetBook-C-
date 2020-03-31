namespace NetBook.Services.Models
{
    using NetBook.Data.Models;
    using NetBook.Services.Mapping;

    public class SchoolServiceModel : IMapFrom<School>, IMapTo<School>
    {
        public string Name { get; set; }

        public string Town { get; set; }

        public string Municipality { get; set; }

        public string Region { get; set; }

        public string Area { get; set; }
    }
}
