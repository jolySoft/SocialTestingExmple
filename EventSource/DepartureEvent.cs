namespace EventSource;

public class DepartureEvent : ShippingEvent
{
    public DepartureEvent(DateTime occurrence, Port port, Ship ship) : base(occurrence, port, ship) { }

    public override void Process()
    {
        Ship.SetSail(this);
        Port.CastOff(Ship);
    }
}