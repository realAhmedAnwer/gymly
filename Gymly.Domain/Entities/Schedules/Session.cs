namespace Gymly.Domain.Entities.Schedules;

public class Session : BaseEntity
{
    public int ClassId { get; set; }
    public Class? Class { get; set; }
    public int TrainerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
}
