namespace Gymly.Application.Features.Home.Queries.GetDashboardStats;

public class DashboardStatsDto
{
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int InactiveMembers { get; set; }
    public int TrainersCount { get; set; }
    public int ClassesCount { get; set; }
    public int ActiveMemberships { get; set; }
    public int ActivePlans { get; set; }
    public int TodaySessions { get; set; }
    public int TodayCheckIns { get; set; }
    public int TotalBookingsToday { get; set; }
    public List<UpcomingSessionDto> UpcomingSessions { get; set; } = [];
}

public class UpcomingSessionDto
{
    public string ClassName { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}