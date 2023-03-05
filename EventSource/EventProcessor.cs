namespace EventSourceTests;

public class EventProcessor
{
    public void Process(ArrivalEvent arrivalEvent)
    {
        arrivalEvent.Process();
    }
}