namespace ShipTracker.Server.Models
{
    public class AddShipDto
    {
        public required string Name { get; set; }
        public required double MaximumSpeed { get; set; }
    }
}