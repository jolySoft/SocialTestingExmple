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

    [Fact]
    public void ArrivalEventBerthsShip()
    {
        var berthInFelixstowe = new ArrivalEvent(new DateTime(2023, 03, 05, 10, 24, 10), _felixstowe, _everGiven);
        var eventProcessor = new EventProcessor();

        eventProcessor.Process(berthInFelixstowe);

        _everGiven.Location.ShouldBe(ShipLocation.InPort);
        _everGiven.ShipsLog.ShouldContain(berthInFelixstowe);
        _felixstowe.Berths.ShouldContain(_everGiven);
    }
}

public class EventProcessor
{
    public void Process(ArrivalEvent arrivalEvent)
    {
        arrivalEvent.Process();
    }
}

public class ArrivalEvent
{
    private readonly DateTime _occurrence;
    private readonly Port _port;
    private readonly Ship _ship;

    public ArrivalEvent(DateTime occurrence, Port port, Ship ship)
    {
        _occurrence = occurrence;
        _port = port;
        _ship = ship;
    }

    public void Process()
    {
        _port.Berth(_ship);
        _ship.Log(this);
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
    public List<ArrivalEvent> ShipsLog { get; }

    private Ship(string name)
    {
        Name = name;
        Ports = new List<Port>();
        ShipsLog = new List<ArrivalEvent>();

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

    public void Log(ArrivalEvent arrivalEvent)
    {
        ShipsLog.Add(arrivalEvent);
    }
}