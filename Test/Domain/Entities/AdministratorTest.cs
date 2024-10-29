using MinimalApi.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class AdministratorTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        // Arrange
        var admin = new Administrator();

        // Act (Set)
        admin.Id = 1;
        admin.Email = "admin@test.com";
        admin.Password = "123456";
        admin.Profie = "Admin";

        // Assert (Get)
        Assert.AreEqual(1, admin.Id);
        Assert.AreEqual("admin@test.com", admin.Email);
        Assert.AreEqual("123456", admin.Password);
        Assert.AreEqual("Admin", admin.Profie);
    }
}