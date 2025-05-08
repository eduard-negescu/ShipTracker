namespace ShipTracker.Server.Models
{
    public class UpdateVoyageDto
    {
        public string? Name { get; set; }
        public DateTime? VoyageDate { get; set; }
        public DateTime? VoyageStart { get; set; }
        public DateTime? VoyageEnd { get; set; }
        public int? DeparturePortId { get; set; }
        public int? ArrivalPortId { get; set; }
        public int? ShipId { get; set; }
    }
}
