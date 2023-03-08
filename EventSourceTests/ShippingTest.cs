using EventSource;
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

    [Fact]
    public void DepartureEventPutsShipToSea()
    {
        _everGiven.Berth(_felixstowe);
        var setToSea = new DepartureEvent(new DateTime(2023, 03, 08, 07, 34, 20), _felixstowe, _everGiven);
        var eventProcessor = new EventProcessor();

        eventProcessor.Process(setToSea);

        _everGiven.CurrentPort.ShouldBeNull();
        _everGiven.Location.ShouldBe(ShipLocation.AtSea);
        _everGiven.ShipsLog.ShouldContain(setToSea);
        _felixstowe.Berths.ShouldNotContain(_everGiven);
    }

    [Fact]
    public void JourneyTimeIsCalculatedFromDepartureEvent()
    {
        var eventProcessor = new EventProcessor();
        var arrivesFelixstowe = new ArrivalEvent(new DateTime(01, 01, 2033, 03, 00, 00), _felixstowe, _everGiven);
        eventProcessor.Process(arrivesFelixstowe);
        var departsFelixstowe = new DepartureEvent(new DateTime(03, 01, 2023, 03, 00, 00), _felixstowe, _everGiven);
        var newark = Port.New("Newark");
        var arrivesNewark = new ArrivalEvent(new DateTime(13, 01, 2023, 03, 00, 00), newark, _everGiven);

        TimeSpan journeyTime = _everGiven.ShipsLog.GetJourneyTime(_felixstowe, newark);

        var expected = new TimeSpan(10, 0, 0, 0);

        journeyTime.ShouldBe(expected);
    }
}