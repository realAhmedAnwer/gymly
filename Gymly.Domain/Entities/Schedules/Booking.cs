using System;
using System.Collections.Generic;
using System.Text;

namespace Gymly.Domain.Entities.Schedules;

public class Booking : BaseEntity
{
    public int SessionId { get; set; }
    public int MemberId { get; set; }
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;
    public bool IsCancelled { get; set; } = false;
}
