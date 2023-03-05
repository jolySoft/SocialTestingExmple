using Shouldly;

namespace EventSourceTests;

public class ShippingTest
{
    [Fact]
    public void CanCreateNewShip()
    {
        var ship = Ship.New("Ever Given");

        ship.Name.ShouldBe("Ever Given");
    }

    [Fact]
    public void CanCreatePort()
    {
        var felixstowe = Port.New("felixstowe");

        felixstowe.Name.ShouldBe("felixstowe");
    }

    [Fact]
    public void BerthShipInPort()
    {
        var everGiven = Ship.New("Ever Given");
        var felixstowe = Port.New("felixstowe");

        felixstowe.Berth(everGiven);

        everGiven.Location.ShouldBe(ShipLocation.InPort);
        everGiven.Ports[0].ShouldBe(felixstowe);
        felixstowe.Berths.ShouldContain(everGiven);
    }
}

public class Port
{
    public string Name { get; }

    public List<Ship> Berths { get; }

    private Port(string name)
    {
        Name = name;
    }

    public static Port New(string name)
    {
        return new Port(name);
    }
}

public class Ship
{
    public string Name { get; }

    private Ship(string name)
    {
        Name = name;

    }

    public static Ship New(string name)
    {
        return new Ship(name);
    }
}