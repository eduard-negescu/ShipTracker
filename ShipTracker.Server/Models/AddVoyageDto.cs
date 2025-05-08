namespace ShipTracker.Server.Models
{
    public class AddVoyageDto
    {
        public required string Name { get; set; }
        public DateTime VoyageDate { get; set; }
        public DateTime VoyageStart { get; set; }
        public DateTime VoyageEnd { get; set; }
        public required int DeparturePortId { get; set; }
        public required int ArrivalPortId { get; set; }
        public required int ShipId { get; set; }
    }
}
