namespace EventSourceTests;

public class Ship
{
    public string Name { get; }
    public ShipLocation Location { get; private set; }
    public Port? CurrentPort { get; private set; }
    public List<object> ShipsLog { get; }

    private Ship(string name)
    {
        Name = name;
        ShipsLog = new List<object>();
    }

    public static Ship New(string name)
    {
        return new Ship(name);
    }

    public void Berth(Port? port)
    {
        Location = ShipLocation.InPort;
        CurrentPort = port;
    }

    public void Log(ArrivalEvent arrivalEvent)
    {
        ShipsLog.Add(arrivalEvent);
    }

    public void SetSail(DepartureEvent departureEvent)
    {
        Location = ShipLocation.AtSea;
        CurrentPort = null;
        ShipsLog.Add(departureEvent);
    }
}