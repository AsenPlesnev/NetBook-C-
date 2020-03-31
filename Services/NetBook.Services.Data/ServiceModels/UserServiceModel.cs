using NetBook.Data.Models;
using NetBook.Services.Mapping;

namespace NetBook.Services.Data.ServiceModels
{
    public class UserServiceModel : IMapFrom<NetBookUser>, IMapTo<NetBookUser>
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
