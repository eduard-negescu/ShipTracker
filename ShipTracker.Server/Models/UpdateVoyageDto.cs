namespace ShipTracker.Server.Models
{
    public class UpdateVoyageDto
    {
        public string? Name { get; set; }
        public DateOnly? VoyageDate { get; set; }
        public DateOnly? VoyageStart { get; set; }
        public DateOnly? VoyageEnd { get; set; }
        public int? DeparturePortId { get; set; }
        public int? ArrivalPortId { get; set; }
        public int? ShipId { get; set; }
    }
}
