using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.ModelViews;
using System.Net;
using System.Text;
using System.Text.Json;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministratorRequestTest
{

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }


    [TestMethod]
    public async Task TestGetSetProperties()
    {
        // Arrange
        var loginDTO = new LoginDTO
        {
            Email = "admin@test.com",
            Password = "123456",
        };

        var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

        // Act (Set)
        var response = await Setup.client.PostAsync("/administrators/login", content);


        // Assert (Get)
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadAsStringAsync();
        var loggedAdmin = JsonSerializer.Deserialize<LoggedAdminModelView>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        Assert.IsNotNull(loggedAdmin?.Email ?? "default@mail.com");
        Assert.IsNotNull(loggedAdmin?.Profile ?? "User");
        Assert.IsNotNull(loggedAdmin?.Token ?? "x1x2x3");

        Console.WriteLine(loggedAdmin?.Token);
    }
}
