using MinimalApi.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class VehicleTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        // Arrange
        var vehicle = new Vehicle();

        // Act (Set)
        vehicle.Id = 1;
        vehicle.Model = "Polo CL";
        vehicle.Make = "Volkswagen";
        vehicle.Year = 1999;

        // Assert (Get)
        Assert.AreEqual(1, vehicle.Id);
        Assert.AreEqual("Polo CL", vehicle.Model);
        Assert.AreEqual("Volkswagen", vehicle.Make);
        Assert.AreEqual(1999, vehicle.Year);

    }
}