using System.Linq;

namespace EventSource;

public class ShipsLog : List<ShippingEvent>
{
    public TimeSpan GetJourneyTime(Port departure, Port destination, Ship ship)
    {
        var setSail = this.OfType<DepartureEvent>().First(e => e.Port == departure && e.Ship == ship);
        var docked = this.OfType<ArrivalEvent>().First(e => e.Port == destination && e.Ship == ship);
        
        return docked.Occurrence - setSail.Occurrence;
    }
}