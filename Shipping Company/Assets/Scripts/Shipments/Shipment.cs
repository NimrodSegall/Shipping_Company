public class Shipment
{
    public string type;
    public float volume;
    public float weight;
    public int amount;
    public Cargo unit;

    public Waypoint from;
    public Waypoint to;

    public Shipment(string type, float volume, float weight, int amount, Waypoint from, Waypoint to)
    {
        unit = new Cargo(type, volume, weight);
        this.type = type;
        this.volume = volume * amount;
        this.weight = weight * amount;
        this.amount = amount;
        this.from = from;
        this.to = to;
    }
}
