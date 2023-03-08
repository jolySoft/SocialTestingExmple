namespace EventSourceTests;

public class DepartureEvent
{
    private readonly DateTime _occurrence;
    private readonly Port _port;
    private readonly Ship _ship;

    public DepartureEvent(DateTime occurrence, Port port, Ship ship)
    {
        _occurrence = occurrence;
        _port = port;
        _ship = ship;
    }

    public void Process()
    {
        _ship.SetSail(this);
        _port.CastOff(_ship);
    }
}