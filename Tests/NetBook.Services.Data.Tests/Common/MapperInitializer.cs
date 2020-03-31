namespace NetBook.Services.Data.Tests.Common
{
    using System.Reflection;

    using NetBook.Data.Models;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(StudentServiceModel).GetTypeInfo().Assembly,
                typeof(Student).GetTypeInfo().Assembly);
        }
    }
}
