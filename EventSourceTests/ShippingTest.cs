using EventSource;
using Shouldly;
using Xunit;

namespace EventSourceTests;

public class ShippingTest
{
    private readonly Ship _everGiven;
    private readonly Port _felixstowe;
    private readonly EventProcessor _eventProcessor;
    private readonly Port _newark;
    private readonly Port _portoDeSantos;
    private readonly Port _mosselBay;

    public ShippingTest()
    {
        _everGiven = Ship.New("Ever Given");

        _felixstowe = Port.New("felixstowe");
        _newark = Port.New("Newark");
        _portoDeSantos = Port.New("Porto de Santos");
        _mosselBay = Port.New("Mossel Bay");

        _eventProcessor = new EventProcessor();
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

        _eventProcessor.Process(berthInFelixstowe);

        _everGiven.Location.ShouldBe(ShipLocation.InPort);
        _everGiven.ShipsLog.ShouldContain(berthInFelixstowe);
        _felixstowe.Berths.ShouldContain(_everGiven);
    }

    [Fact]
    public void DepartureEventPutsShipToSea()
    {
        _everGiven.Berth(_felixstowe);
        var setToSea = new DepartureEvent(new DateTime(2023, 03, 08, 07, 34, 20), _felixstowe, _everGiven);
        
        _eventProcessor.Process(setToSea);

        _everGiven.CurrentPort.ShouldBeNull();
        _everGiven.Location.ShouldBe(ShipLocation.AtSea);
        _everGiven.ShipsLog.ShouldContain(setToSea);
        _felixstowe.Berths.ShouldNotContain(_everGiven);
    }

    [Fact]
    public void JourneyTimeIsCalculatedFromDepartureEvent()
    {
        BuildShipsLogFromFelixstoweToMosselBay();

        var journeyTime = _everGiven.ShipsLog.GetJourneyTime(_felixstowe, _newark);

        var expected = new TimeSpan(10, 0, 0, 0);
        journeyTime.ShouldBe(expected);
    }

    [Fact]
    public void JourneyTimeFromFelixstoweToPortoDeSantos()
    {
        BuildShipsLogFromFelixstoweToMosselBay();

        var journeyTime = _everGiven.ShipsLog.GetJourneyTime(_felixstowe, _portoDeSantos);

        var expected = new TimeSpan(20, 17, 30, 00);
        journeyTime.ShouldBe(expected);
    }

    private void BuildShipsLogFromFelixstoweToMosselBay()
    {
        _eventProcessor.Process(new ArrivalEvent(new DateTime(2023, 01, 01, 12, 45, 00), _felixstowe, _everGiven));
        _eventProcessor.Process(new DepartureEvent(new DateTime(2023, 01, 03, 03, 00, 00), _felixstowe, _everGiven));
        _eventProcessor.Process(new ArrivalEvent(new DateTime(2023, 01, 13, 03, 00, 00), _newark, _everGiven));
        _eventProcessor.Process(new DepartureEvent(new DateTime(2023, 01, 15, 14, 30, 00), _newark, _everGiven));
        _eventProcessor.Process(new ArrivalEvent(new DateTime(2023, 01, 23, 20, 30, 00), _portoDeSantos, _everGiven));
        _eventProcessor.Process(new DepartureEvent(new DateTime(2023, 01, 26, 06, 05, 00), _portoDeSantos, _everGiven));
        _eventProcessor.Process(new ArrivalEvent(new DateTime(2023, 02, 04, 10, 35, 00), _mosselBay, _everGiven));
        _eventProcessor.Process(new DepartureEvent(new DateTime(2023, 02, 05, 16, 50, 00), _mosselBay, _everGiven));
    }
}