namespace NetBook.Services.Models
{
    using NetBook.Data.Models;
    using NetBook.Services.Mapping;

    public class UserServiceModel : IMapFrom<NetBookUser>, IMapTo<NetBookUser>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsClassTeacher { get; set; }

        public bool IsTeacher { get; set; }

        public string ClassId { get; set; }

        public ClassServiceModel Class { get; set; }

        // public string RoleName { get; set; }
    }
}
