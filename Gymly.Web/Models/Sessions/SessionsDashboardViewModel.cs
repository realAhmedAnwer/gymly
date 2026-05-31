using Gymly.Application.Features.Sessions.Queries.GetSessionsList;

namespace Gymly.Web.Models.Sessions;

public class SessionsDashboardViewModel
{
    public List<SessionDto> Sessions { get; set; } = [];
    public int TotalActiveSessions => Sessions.Count;
}