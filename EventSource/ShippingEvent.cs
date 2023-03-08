namespace EventSource;

public abstract class ShippingEvent
{
    public DateTime Occurrence { get; }
    public Port Port { get; }
    public Ship Ship { get; }

    protected ShippingEvent(DateTime occurrence, Port port, Ship ship)
    {
        Occurrence = occurrence;
        Port = port;
        Ship = ship;
    }

    public abstract void Process();
}