namespace NetBook.Services.Models
{
    using NetBook.Data.Models;
    using NetBook.Data.Models.Enums;
    using NetBook.Services.Mapping;
    public class ClassServiceModel : IMapFrom<Class>, IMapTo<Class>
    {
        public string Id { get; set; }

        public int ClassNumber { get; set; }

        public ClassLetter ClassLetter { get; set; }

        public int SchoolYearStart { get; set; }

        public int SchoolYearEnd { get; set; }

        public string ClassTeacherId { get; set; }

        public UserServiceModel ClassTeacher { get; set; }
    }
}
