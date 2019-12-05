using System.Collections.Generic;
using UnityEngine;

//public Shipment(string type, float volume, float weight, int amount, Waypoint from, Waypoint to)

public class Job
{
    public Shipment shipment;

    public static string[] types = { "hamburgers", "steel rods", "bricks", "electronic tablets", "top hats" };
    public static Waypoint[] from = { new Waypoint() };
    public static Waypoint[] to = { new Waypoint() };

    public int deadline;

    public Job()
    {
        this.deadline = Random.Range(0, 23);
        shipment = new Shipment(
            types[Random.Range(0, types.Length)],
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0, 100),
            from[Random.Range(0, from.Length)],
            to[Random.Range(0, to.Length)]
            );
    }

    public string Format()
    {
        string jobFormatted = "Deliver " + shipment.amount + " units of " + shipment.type + " from " + shipment.from.placeName + " to " +
            shipment.to.placeName + " before " + Utilities.FormatTime(deadline);
        return jobFormatted;
    }


}
