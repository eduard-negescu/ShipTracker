namespace ShipTracker.Server.Models
{
    public class AddPortDto
    {
        public required string Name { get; set; }
        public required int CountryId { get; set; }
    }
}
