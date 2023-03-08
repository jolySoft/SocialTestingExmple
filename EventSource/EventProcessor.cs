namespace EventSource;

public class EventProcessor
{
    public void Process(ShippingEvent shippingEvent)
    {
        shippingEvent.Process();
    }
}