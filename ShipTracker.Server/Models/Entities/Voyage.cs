namespace ShipTracker.Server.Models.Entities
{
    public class Voyage
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly VoyageDate { get; set; }
        public DateOnly VoyageStart { get; set; }
        public DateOnly VoyageEnd { get; set; }

        public int DeparturePortId { get; set; }
        public Port? DeparturePort { get; set; }

        public int ArrivalPortId { get; set; }
        public Port? ArrivalPort { get; set; }

        public int ShipId { get; set; }
        public Ship? Ship { get; set; }
    }
}
