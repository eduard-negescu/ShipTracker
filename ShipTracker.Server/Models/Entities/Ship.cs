namespace ShipTracker.Server.Models.Entities
{
    public class Ship
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double MaximumSpeed { get; set; }
    }
}
