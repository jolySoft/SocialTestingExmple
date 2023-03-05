using Shouldly;

namespace EventSourceTests;

public class ShippingTest
{
    private readonly Ship _everGiven;
    private readonly Port _felixstowe;

    public ShippingTest()
    {
        _everGiven = Ship.New("Ever Given");
        _felixstowe = Port.New("felixstowe");
    }

    [Fact]
    public void CanCreateNewShip()
    {
        _everGiven.Name.ShouldBe("Ever Given");
    }

    [Fact]
    public void CanCreatePort()
    {
        _felixstowe.Name.ShouldBe("felixstowe");
    }

    [Fact]
    public void BerthShipInPort()
    {
        _felixstowe.Berth(_everGiven);

        _everGiven.Location.ShouldBe(ShipLocation.InPort);
        _everGiven.Ports[0].ShouldBe(_felixstowe);
        _felixstowe.Berths.ShouldContain(_everGiven);
    }
}

public enum ShipLocation
{
    InPort
}

public class Port
{
    public string Name { get; }

    public List<Ship> Berths { get; }

    private Port(string name)
    {
        Name = name;
        Berths = new List<Ship>();
    }

    public void Berth(Ship ship)
    {
        Berths.Add(ship);
        ship.Berth(this);
    }

    public static Port New(string name)
    {
        return new Port(name);
    }
}

public class Ship
{
    public string Name { get; }
    public ShipLocation Location { get; private set; }
    public List<Port> Ports { get; }

    private Ship(string name)
    {
        Name = name;
        Ports = new List<Port>();

    }

    public static Ship New(string name)
    {
        return new Ship(name);
    }

    public void Berth(Port port)
    {
        Location = ShipLocation.InPort;
        Ports.Add(port);
    }
}