using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;

namespace Test.Domain.Entities;

[TestClass]
public class AdministratorServiceTest
{
    private DataBaseContext CreateTestContext()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DataBaseContext(configuration);
    }

    [TestMethod]
    public void TestGetSetProperties()
    {
        // Arrange
        var context = CreateTestContext();
        context.Database.ExecuteSqlRaw("truncate table administrators");

        var admin = new Administrator();
        admin.Email = "test@test.com";
        admin.Password = "123456";
        admin.Profie = "Admin";

        
        var adminServices = new AdministratorService(context);

        // Act (Set)
        adminServices.Add(admin);


        // Assert (Get)
        Assert.AreEqual(1, adminServices.GetAdministrators(1).Count());
        
    }
}
