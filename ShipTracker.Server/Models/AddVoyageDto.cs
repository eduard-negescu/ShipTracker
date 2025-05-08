namespace ShipTracker.Server.Models
{
    public class AddVoyageDto
    {
        public required string Name { get; set; }
        public DateOnly VoyageDate { get; set; }
        public DateOnly VoyageStart { get; set; }
        public DateOnly VoyageEnd { get; set; }
        public required int DeparturePortId { get; set; }
        public required int ArrivalPortId { get; set; }
        public required int ShipId { get; set; }
    }
}
