namespace EventSourceTests;

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