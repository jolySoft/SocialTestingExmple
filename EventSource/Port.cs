namespace EventSourceTests;

public class Port
{
    public string Name { get; }

    public List<Ship> Berths { get; }

    private Port(string name)
    {
        Name = name;
        Berths = new List<Ship>();
    }

    public void Berth(Ship ship)
    {
        Berths.Add(ship);
        ship.Berth(this);
    }

    public static Port? New(string name)
    {
        return new Port(name);
    }

    public void CastOff(Ship ship)
    {
        Berths.Remove(ship);
    }
}