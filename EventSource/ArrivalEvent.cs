namespace EventSource;

public class ArrivalEvent : ShippingEvent
{
    public ArrivalEvent(DateTime occurrence, Port port, Ship ship) : base(occurrence, port, ship) { }

    public override void Process()
    {
        Port.Berth(Ship);
        Ship.Log(this);
    }
}