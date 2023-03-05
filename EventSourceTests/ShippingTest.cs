using Shouldly;
using Xunit;

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
        _everGiven.CurrentPort.ShouldBe(_felixstowe);
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
