namespace Contracts;
public class RoomMessageModel
{
    public string Name { get; set; }
    public string HomeType { get; set; }
    public string RoomType { get; set; }
    public int TotalOccupancy { get; set; }
    public int TotalBedrooms { get; set; }
    public int TotalBathrooms { get; set; }
    public string Summary { get; set; }
    public string Address { get; set; }
    public bool HasTV { get; set; }
    public bool HasKitchen { get; set; }
    public bool HasAirCon { get; set; }
    public bool HasHeating { get; set; }
    public bool HasInternet { get; set; }
    public float Price { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public ICollection<string> ImagePath { get; set; }
    public string Email { get; set; }
#nullable enable
    public string? LocationName { get; set; }
}