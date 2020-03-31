namespace NetBook.Services.Data.Tests.Service
{
    using System.Linq;
    using System.Threading.Tasks;

    using NetBook.Data;
    using NetBook.Data.Models;
    using NetBook.Services.Data.School;
    using NetBook.Services.Data.Tests.Common;
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;
    using Xunit;

    public class SchoolServiceTests
    {
        private ISchoolService schoolService;

        public SchoolServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        private School GetDummyData()
        {
            return new School
            {
                Name = "TestName",
                Town = "Test",
                Municipality = "Test",
                Region = "Test",
                Area = "Test",
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetSchool_ShouldGetSuccessfullySchool()
        {
            string errorMessagePrefix = "School GetSchool() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.schoolService = new SchoolService(context);

            var actualResult = await this.schoolService.GetSchoolAsync();
            var expectedResult = context.School.First().To<SchoolServiceModel>();

            Assert.True(actualResult.Name == expectedResult.Name, errorMessagePrefix + " " + "School Name is not returned properly.");
            Assert.True(actualResult.Town == expectedResult.Town, errorMessagePrefix + " " + "School Town is not returned properly.");
            Assert.True(actualResult.Municipality == expectedResult.Municipality, errorMessagePrefix + " " + "School Municipality is not returned properly.");
            Assert.True(actualResult.Region == expectedResult.Region, errorMessagePrefix + " " + "School Region is not returned properly.");
            Assert.True(actualResult.Area == expectedResult.Area, errorMessagePrefix + " " + "School Area is not returned properly.");
        }

        [Fact]
        public async Task UpdateSchool_ShouldUpdateSchoolSuccessfully()
        {
            string errorMessagePrefix = "School UpdateAsync() does not work properly.";

            var context = NetBookDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            this.schoolService = new SchoolService(context);

            var testSchool = new SchoolServiceModel
            {
                Name = "TestName1",
                Town = "Test1",
                Municipality = "Test1",
                Region = "Test1",
                Area = "Test1",
            };

            var actualResult = await this.schoolService.UpdateAsync(testSchool);

            Assert.True(actualResult, errorMessagePrefix);

            var updatedSchool = context.School.First();

            Assert.True(updatedSchool.Name == testSchool.Name, errorMessagePrefix + " " + "School Name is not set properly.");
            Assert.True(updatedSchool.Town == testSchool.Town, errorMessagePrefix + " " + "School Town is not set properly.");
            Assert.True(updatedSchool.Municipality == testSchool.Municipality, errorMessagePrefix + " " + "School Municipality is not set properly.");
            Assert.True(updatedSchool.Region == testSchool.Region, errorMessagePrefix + " " + "School Region is not set properly.");
            Assert.True(updatedSchool.Area == testSchool.Area, errorMessagePrefix + " " + "School Area is not set properly.");
        }
    }
}
