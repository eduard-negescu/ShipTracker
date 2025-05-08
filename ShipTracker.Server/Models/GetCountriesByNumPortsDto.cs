namespace ShipTracker.Server.Models
{
    public class GetCountriesByNumPortsDto
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public int NumberOfPorts { get; set; }
    }
}
