using System.Linq;

namespace EventSource;

public class ShipsLog : List<ShippingEvent>
{
    private Ship _ship;

    public ShipsLog(Ship ship)
    {
        _ship = ship;
    }

    public TimeSpan GetJourneyTime(Port departure, Port destination)
    {
        var setSail = this.OfType<DepartureEvent>().First(e => e.Port == departure && e.Ship == _ship);
        var docked = this.OfType<ArrivalEvent>().First(e => e.Port == destination && e.Ship == _ship);
        
        return docked.Occurrence - setSail.Occurrence;
    }
}