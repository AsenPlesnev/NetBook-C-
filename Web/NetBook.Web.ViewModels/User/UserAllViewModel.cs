namespace NetBook.Web.ViewModels.User
{
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class UserAllViewModel : IMapFrom<UserServiceModel>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsClassTeacher { get; set; }

        public string RoleName { get; set; }
    }
}
