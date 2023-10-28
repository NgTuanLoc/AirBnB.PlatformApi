namespace Core.Models.Database
{
    public class RoomModel
    {
        public string Name { get; set; } = "RoomName";
        public string HomeType { get; set; } = "HomeType";
        public string RoomType { get; set; } = "RoomType";
        public int TotalOccupancy { get; set; }
        public int TotalBedrooms { get; set; }
        public int TotalBathrooms { get; set; }
        public string Summary { get; set; } = "Summary";
        public string Address { get; set; } = "Address";
        public bool HasTV { get; set; }
        public bool HasKitchen { get; set; }
        public bool HasAirCon { get; set; }
        public bool HasHeating { get; set; }
        public bool HasInternet { get; set; }
        public float Price { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public ICollection<string> ImagePath { get; set; } = default!;
        public string? LocationName { get; set; }
    }
    public class LocationModel
    {
        public string Name { get; set; } = "LocationName";
        public string Province { get; set; } = "LocationProvince";
        public string Country { get; set; } = "LocationCountry";
        public string ImagePath { get; set; } = "LocationImagePath";
    }
}