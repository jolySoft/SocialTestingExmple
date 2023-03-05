using Shouldly;

namespace EventSourceTests;

public class ShippingTest
{
    [Fact]
    public void CanCreateNewShip()
    {
        var ship = new Ship("Ever Given");

        ship.Name.ShouldBe("Ever Given");
    }
}

public class Ship
{
    public string Name { get; }

    public Ship(string name)
    {
        Name = name;

    }
}